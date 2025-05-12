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
        // Alleen load van mixer op basis van PlayerPrefs
        savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        musicSlider.value = savedMusicVolume;

        // meteen naar mixer (genormaliseerd)
        float norm = savedMusicVolume / 100f;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(norm, 0.0001f)) * 20f);
    }

    public void PlayGame() => SceneManager.LoadScene("MuseumHub");
    public void QuitGame() => Application.Quit();

    // Direct naar gameplay met easy/hard
    public void OnEasyPressed() { GameSettings.IsHard = false; SceneManager.LoadScene("MusumHub"); }
    public void OnHardPressed() { GameSettings.IsHard = true; SceneManager.LoadScene("MuseumHub"); }

    public void SettingsPressed()
    {
        // 1) Capture actuele instellingen als INIT
        initIsHard = GameSettings.IsHard;
        initMusicVolume = PlayerPrefs.GetFloat("MusicVolume", savedMusicVolume);

        // 2) Vul UI met INIT
        easyToggle.isOn = !initIsHard;
        hardToggle.isOn = initIsHard;
        musicSlider.value = initMusicVolume;

        // 3) Toon panel
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnApplyPressed()
    {
        // sla difficulty op
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();

        GameSettings.IsHard = hardToggle.isOn;

        // 1) lees slider (0–100) uit en sla precies dát op
        float stored = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", stored);
        PlayerPrefs.Save();

        // 2) normaliseer en zet in de mixer
        float normalized = stored / 100f;      // 0…1
        float db = Mathf.Log10(Mathf.Max(normalized, 0.0001f)) * 20f;
        audioMixer.SetFloat("MusicVolume", db);

        // 3) update je init‐waarden
        initMusicVolume = stored;
        initIsHard = GameSettings.IsHard;
    }

    public void OnBackPressed()
    {
        // sluit zonder te saven
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
