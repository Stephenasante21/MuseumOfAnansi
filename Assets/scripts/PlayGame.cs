using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayGame : MonoBehaviour
{
    [SerializeField] private string SceneName;

    public GameObject panel;
    void Awake()
    {
        panel.SetActive(false);
    }

    public void Show() { panel.SetActive(true); Time.timeScale = 0f; }
    public void Hide() { panel.SetActive(false); Time.timeScale = 1f; }
    public void OnStatuePlayPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneName);
    }
}
