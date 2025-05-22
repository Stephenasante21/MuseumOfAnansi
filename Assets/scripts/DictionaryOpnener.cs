using UnityEngine;

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
    }

    public void CloseDictionary()
    {
        if (!_isOpen) return;

        _isOpen = false;
        dictionaryPanel.SetActive(false);

        if (pauseTimeWhileOpen)
            Time.timeScale = 1f;
    }

}
