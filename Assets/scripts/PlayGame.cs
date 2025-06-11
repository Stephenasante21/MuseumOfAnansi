using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayGame : MonoBehaviour
{
    [SerializeField] private string normalSceneName;
    [SerializeField] private string hardSceneName;

    [SerializeField] private GameObject panel;


    void Awake()
    {
        panel.SetActive(false);
    }

    public void Show() { 
        panel.SetActive(true); 
        Time.timeScale = 0f;
        MouseManager.Instance.UnlockCursor();
    }
    public void Hide() { 
        panel.SetActive(false); 
        Time.timeScale = 1f;
        MouseManager.Instance.LockCursor();
    }
    public void OnPlayButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(normalSceneName);
    }

    public void OnHardButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(hardSceneName);
    }
}
