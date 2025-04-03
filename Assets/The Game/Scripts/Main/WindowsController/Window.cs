using UnityEngine;

namespace WindowsSystem
{
    [RequireComponent(typeof(Animator))]
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] private bool isOpened;

        public bool IsOpened => isOpened;
        public Animator Animator { get; private set; }

        private void OnEnable()
        {
            Animator = GetComponent<Animator>();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            
            isOpened = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);

            isOpened = false;
        }
    }
}