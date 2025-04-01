using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WindowsSystem
{
    public abstract class WindowsController : MonoBehaviour
    {
        [SerializeField] protected Window[] windows;

        public Window Current { get; private set; }
        
        private const string OpenTransitionName = "Open";
        private const string ClosedStateName = "Closed";
    
        public Animator initiallyOpen;

        private int _openParameterId;
        private Animator _open;
        private GameObject _previouslySelected;


        protected void Start()
        {
            Current = windows.FirstOrDefault(x => x.GetType() == typeof(MainWindow));
            
            _openParameterId = Animator.StringToHash(OpenTransitionName);

            if (initiallyOpen == null)
                return;

            OpenPanel(initiallyOpen);

            //OpenWindow(Current);
        }

        public void OpenWindow(Window window)
        {
            if (_open == window.Animator)
                return;

            window.Animator.gameObject.SetActive(true);
            GameObject newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

            window.Animator.transform.SetAsLastSibling();

            CloseCurrent();

            _previouslySelected = newPreviouslySelected;

            _open = window.Animator;
            _open.SetBool(_openParameterId, true);

            GameObject go = FindFirstEnabledSelectable(anim.gameObject);

            SetSelected(go);
        }

        public void OpenWindow<T>() where T : Window
        {
            foreach (Window m in windows.Where(x => x.IsOpened))
                m.Close();

            Window window = windows.FirstOrDefault(x => x.GetType() == typeof(T));

            window.Open();
            Current = window;

#if UNITY_EDITOR
            Debug.Log($"Window open: {window.GetType()}");
#endif
        }

        public void CloseCurrent()
        {
            if (_open == null)
                return;

            _open.SetBool(_openParameterId, false);
            SetSelected(_previouslySelected);
            StartCoroutine(DisablePanelDelayed(_open));
            _open = null;
        }
        
        private IEnumerator DisablePanelDelayed(Animator anim)
        {
            bool closedStateReached = false;
            bool wantToClose = true;
            while (!closedStateReached && wantToClose)
            {
                if (!anim.IsInTransition(0))
                    closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(ClosedStateName);

                wantToClose = !anim.GetBool(_openParameterId);

                yield return new WaitForEndOfFrame();
            }

            if (wantToClose)
                anim.gameObject.SetActive(false);
        }

        private void SetSelected(GameObject go)
        {
            EventSystem.current.SetSelectedGameObject(go);
        }
        
        private GameObject FindFirstEnabledSelectable(GameObject gameObject)
        {
            GameObject go = null;
            var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
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