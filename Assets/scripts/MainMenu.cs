using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Difficulty Toggles")]
    public Toggle easyToggle;
    public Toggle hardToggle;

    [Header("Audio")]
    public Slider musicSlider;
    public AudioMixer audioMixer;

    private float savedMusicVolume;

    private bool initIsHard;
    private float initMusicVolume;

    private const float DefaultMusicVolume = 0f;
    private const bool DefaultIsHard = false;

    void Start()
    {
        savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        musicSlider.value = savedMusicVolume;

        float norm = savedMusicVolume / 100f;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(norm, 0.0001f)) * 20f);
    }

    public void PlayGame() => SceneManager.LoadScene("MuseumHub");
    public void QuitGame() => Application.Quit();

    public void OnEasyPressed() { GameSettings.IsHard = false; SceneManager.LoadScene("MusumHub"); }
    public void OnHardPressed() { GameSettings.IsHard = true; SceneManager.LoadScene("MuseumHub"); }

    public void SettingsPressed()
    {
        initIsHard = GameSettings.IsHard;
        initMusicVolume = PlayerPrefs.GetFloat("MusicVolume", savedMusicVolume);

        easyToggle.isOn = !initIsHard;
        hardToggle.isOn = initIsHard;
        musicSlider.value = initMusicVolume;

        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnApplyPressed()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();

        GameSettings.IsHard = hardToggle.isOn;

        float stored = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", stored);
        PlayerPrefs.Save();

        float normalized = stored / 100f;      
        float db = Mathf.Log10(Mathf.Max(normalized, 0.0001f)) * 20f;
        audioMixer.SetFloat("MusicVolume", db);

        initMusicVolume = stored;
        initIsHard = GameSettings.IsHard;
    }

    public void OnBackPressed()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
