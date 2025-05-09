using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        SceneManager.LoadScene("MuseumHub");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnEasyPressed()
    {
        GameSettings.IsHard = false;
        SceneManager.LoadScene("GameScene");
    }

    public void OnHardPressed()
    {
        GameSettings.IsHard = true;
        SceneManager.LoadScene("GameScene");
    }
}
