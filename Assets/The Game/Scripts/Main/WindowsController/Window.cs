using UnityEngine;

namespace WindowsSystem
{
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] private bool isOpened;

        public bool IsOpened => isOpened;

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