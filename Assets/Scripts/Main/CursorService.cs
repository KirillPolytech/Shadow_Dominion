using UnityEngine;

public class CursorService
{
    public void SetState(CursorLockMode cursorLockMode)
    {
        Cursor.lockState = cursorLockMode;
    }
}
