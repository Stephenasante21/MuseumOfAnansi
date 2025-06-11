using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCursorState(bool visible, CursorLockMode lockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = lockMode;

    }

    public void LockCursor()
    {
        SetCursorState(false, CursorLockMode.Locked);
    }

    public void UnlockCursor()
    {
        SetCursorState(true, CursorLockMode.None);
    }
}
