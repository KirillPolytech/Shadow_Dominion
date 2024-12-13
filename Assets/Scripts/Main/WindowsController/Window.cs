using DG.Tweening;
using UnityEngine;

namespace WindowsSystem
{
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] private bool isOpened;
        [SerializeField] private float fadeInTime = 0.25f;
        [SerializeField] private float fadeOutTime = 0.25f;

        public bool IsOpened => isOpened;

        public void Open()
        {
            transform.DOScale(1, fadeOutTime);

            isOpened = true;
        }

        public void Close()
        {
            transform.DOScale(0, fadeInTime);

            isOpened = false;
        }
    }
}