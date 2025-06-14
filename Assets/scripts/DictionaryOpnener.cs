using UnityEngine;
using UnityEngine.SceneManagement;

public class DictionaryOpnener : MonoBehaviour
{
    public GameObject dictionaryPanel;

    public bool pauseTimeWhileOpen = true;

    bool _isOpen = false;

    void Start()
    {
        dictionaryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            ToggleDictionary();
    }

    public void ToggleDictionary()
    {
        _isOpen = !_isOpen;
        dictionaryPanel.SetActive(_isOpen);

        if (pauseTimeWhileOpen)
            Time.timeScale = _isOpen ? 0f : 1f;
        if (_isOpen)
        {
            MouseManager.Instance.UnlockCursor();
            AudioManager.Instance.PauseBGM();
        }
        else if(SceneManager.GetAllScenes().Length==2)
        {
            AudioManager.Instance.bgmSource.UnPause();
        }
        else if (SceneManager.GetAllScenes().Length == 1)
        {
            MouseManager.Instance.LockCursor();
            AudioManager.Instance.bgmSource.UnPause();
        }
    }

    public void CloseDictionary()
    {
        if (pauseTimeWhileOpen)
            Time.timeScale = 1f;

        ToggleDictionary();
    }
}
