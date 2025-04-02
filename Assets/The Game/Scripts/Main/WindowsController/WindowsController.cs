using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WindowsSystem
{
    public abstract class WindowsController : MonoBehaviour
    {
        [SerializeField] protected Window[] windows;

        public Window Current { get; private set; }
        
        private const string OpenTransitionName = "Open";
        private const string ClosedStateName = "Closed";
    
        public Window initiallyOpen;

        private int _openParameterId;
        private GameObject _previouslySelected;
        
        protected void Start()
        {
            _openParameterId = Animator.StringToHash(OpenTransitionName);
            
            if (initiallyOpen)
                OpenWindow(initiallyOpen);
        }

        public void OpenWindow(Window window)
        {
            window.Open();
            GameObject newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

            window.Animator.transform.SetAsLastSibling();

            CloseCurrent();

            _previouslySelected = newPreviouslySelected;

            Current = window;
            Current.Animator.SetBool(_openParameterId, true);

            GameObject go = FindFirstEnabledSelectable(window.Animator.gameObject);

            SetSelected(go);
        }

        public void OpenWindow<T>() where T : Window
        {
            Window window = windows.FirstOrDefault(x => x.GetType() == typeof(T));
            
            OpenWindow(window);

            window.Open();
            Current = window;

#if UNITY_EDITOR
            Debug.Log($"Window open: {window.GetType()}");
#endif
        }

        public void CloseCurrent()
        {
            if (!Current)
                return;

            Current.Animator.SetBool(_openParameterId, false);
            SetSelected(_previouslySelected);
            StartCoroutine(DisablePanelDelayed(Current));
            Current = null;
        }
        
        private IEnumerator DisablePanelDelayed(Window window)
        {
            bool closedStateReached = false;
            bool wantToClose = true;
            while (!closedStateReached && wantToClose)
            {
                if (!window.Animator.IsInTransition(0))
                    closedStateReached = window.Animator.GetCurrentAnimatorStateInfo(0).IsName(ClosedStateName);

                wantToClose = !window.Animator.GetBool(_openParameterId);

                yield return new WaitForEndOfFrame();
            }

            if (wantToClose)
                window.Close();
        }

        private void SetSelected(GameObject go)
        {
            EventSystem.current.SetSelectedGameObject(go);
        }
        
        private GameObject FindFirstEnabledSelectable(GameObject obj)
        {
            GameObject go = null;
            var selectables = obj.GetComponentsInChildren<Selectable>(true);
            foreach (var selectable in selectables)
            {
                if (selectable.IsActive() && selectable.IsInteractable())
                {
                    go = selectable.gameObject;
                    break;
                }
            }

            return go;
        }
    }
}