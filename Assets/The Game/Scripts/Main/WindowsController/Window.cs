using UnityEngine;

namespace WindowsSystem
{
    [RequireComponent(typeof(Animator))]
    public abstract class Window : MonoBehaviour
    {
        private const string OpenTransitionName = "Open";
        private const string ClosedStateName = "Closed";

        [SerializeField] private bool isOpened;

        public bool IsOpened => isOpened;
        public Animator Animator { get; private set; }
        private int _openParameterId;
        private int _closeParameterId;

        private void OnEnable()
        {
            Animator = GetComponent<Animator>();

            _openParameterId = Animator.StringToHash(OpenTransitionName);
            _closeParameterId = Animator.StringToHash(OpenTransitionName);
        }

        public void Open()
        {
            gameObject.SetActive(true);

            //_animator.SetBool(_openParameterId, false);
            
            isOpened = true;
        }

        public void Close()
        {
            //_animator.SetBool(_closeParameterId, false);

            //gameObject.SetActive(false);

            isOpened = false;
        }
    }
}