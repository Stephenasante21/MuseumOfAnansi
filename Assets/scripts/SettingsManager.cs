using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("UI")]
    public GameObject settingsPanel;
    public Button applyButton;
    public Button quitButton;

    public Slider musicVolumeSlider;
    public Slider ambienceVolumeSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        settingsPanel.SetActive(false);

        applyButton.onClick.AddListener(ApplyAndClose);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf) Close();
            else Open();
        }
    }

    public void Open()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;

        MouseManager.Instance.UnlockCursor();

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol", 1f);
        ambienceVolumeSlider.value = PlayerPrefs.GetFloat("AmbienceVol", 1f);
    }

    public void Close()
    {
        settingsPanel.SetActive(false);Time.timeScale = 1f;

        MouseManager.Instance.LockCursor();
    }

    void ApplyAndClose()
    {
        PlayerPrefs.SetFloat("MusicVol", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("AmbienceVol", ambienceVolumeSlider.value);
        PlayerPrefs.Save();

        AudioListener.volume = musicVolumeSlider.value;

        Close();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
 