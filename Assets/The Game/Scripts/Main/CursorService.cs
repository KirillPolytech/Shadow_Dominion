using UnityEngine;

public class CursorService
{
    public static void SetState(CursorLockMode cursorLockMode)
    {
        Cursor.lockState = cursorLockMode;
    }
}
