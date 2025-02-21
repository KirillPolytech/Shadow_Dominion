using System.Linq;
using UnityEngine;

namespace WindowsSystem
{
    public abstract class WindowsController : MonoBehaviour
    {
        [SerializeField] protected Window[] windows;

        public Window Current { get; private set; }

        protected void Start()
        {
            Current = windows.FirstOrDefault(x => x.GetType() == typeof(MainWindow));

            //OpenWindow(Current);
        }

        public void OpenWindow(Window window)
        {
            foreach (Window m in windows.Where(x => x.IsOpened))
                m.Close();

            window.Open();

#if UNITY_EDITOR
            Debug.Log($"Window open: {window.GetType()}");
#endif
        }

        public void OpenWindow<T>() where T : Window
        {
            foreach (Window m in windows.Where(x => x.IsOpened))
                m.Close();

            Window window = windows.FirstOrDefault(x => x.GetType() == typeof(T));

            window.Open();

#if UNITY_EDITOR
            Debug.Log($"Window open: {window.GetType()}");
#endif
        }

        public void CloseCurrent()
        {
            if (Current == null)
                return;

            //Window window = windows.Single(x => x.WindowName == Current);
            //window.Close();

            Current = null;
        }
    }
}