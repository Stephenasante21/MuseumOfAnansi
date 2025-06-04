using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class AsantewaaGameController : MonoBehaviour
{
    [Header("Scene Navigation")]
    [SerializeField] private string SceneName;

    [Header("Paintings")]
    public GameObject[] easyPaintings;
    public GameObject[] hardPaintings;
    public Transform paintingContainer;

    [Header("UI References")]
    public TMP_Text questionText;
    public TMP_Text timerText;
    public Button closeButton;

    [Header("Audio Clips")]
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    private AudioSource _audioSource;

    private PaintingData _data;
    private int _currentStep;

    private int _paintingIndex = 0;
    private GameObject _currentPaintingGO;

    private float elapsedTime;
    private bool _timerRunning;
    public float timeLimit = 45f;

    private Color _defaultPromptColor;

    private void Awake()
    {
        // Ensure there is an AudioSource on this GameObject
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        _defaultPromptColor = questionText.color;

        elapsedTime = 0f;
        _timerRunning = true;
        timerText.gameObject.SetActive(false);

        closeButton.onClick.AddListener(CloseGame);

        SpawnNextPainting();
    }

    void Update()
    {
        if (_timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void SpawnNextPainting()
    {
        // Destroy the previous painting, if any
        if (_currentPaintingGO != null)
            Destroy(_currentPaintingGO);

        var pool = GameSettings.IsHard ? hardPaintings : easyPaintings;

        // Clamp index
        if (_paintingIndex < 0) _paintingIndex = 0;
        if (_paintingIndex >= pool.Length) _paintingIndex = pool.Length - 1;

        // Instantiate the new painting prefab
        _currentPaintingGO = Instantiate(pool[_paintingIndex], paintingContainer, false);

        // Cache its PaintingData and wire up hotspot callbacks
        _data = _currentPaintingGO.GetComponent<PaintingData>();
        for (int i = 0; i < _data.hotspots.Length; i++)
        {
            int idx = i;
            _data.hotspots[i].onClick.RemoveAllListeners();
            _data.hotspots[i].onClick.AddListener(() => OnHotspotClicked(idx));
        }

        _currentStep = 0;
        ShowStep();
    }

    void ShowStep()
    {
        questionText.color = _defaultPromptColor;

        var step = _data.steps[_currentStep];
        questionText.text = DictionaryManager.Instance.GetWordById(step.wordId).foreign;

        foreach (var btn in _data.hotspots)
            btn.interactable = true;
    }

    void OnHotspotClicked(int idx)
    {
        var step = _data.steps[_currentStep];

        if (idx == step.correctHotspot)
        {
            // Play correct sound
            if (correctSound != null)
            {
                _audioSource.PlayOneShot(correctSound);
            }

            // Mark as found
            var entry = DictionaryManager.Instance.GetWordById(step.wordId);
            FoundWordsManager.Instance.AddEntry(entry);

            questionText.color = Color.green;
            foreach (var b in _data.hotspots)
                b.interactable = false;

            // Proceed after a short delay
            Invoke(nameof(AdvanceStep), 1f);
        }
        else
        {
            // Play incorrect sound
            if (incorrectSound != null)
            {
                _audioSource.PlayOneShot(incorrectSound);
            }

            questionText.color = Color.red;
            Invoke(nameof(ResetPromptColor), 0.5f);
        }
    }

    void ResetPromptColor()
    {
        questionText.color = _defaultPromptColor;
    }

    void AdvanceStep()
    {
        _currentStep++;

        if (_currentStep >= _data.steps.Length)
        {
            // Move to next painting
            _paintingIndex++;

            var pool = GameSettings.IsHard ? hardPaintings : easyPaintings;
            if (_paintingIndex < pool.Length)
            {
                SpawnNextPainting();
            }
            else
            {
                EndGame();
            }
        }
        else
        {
            ShowStep();
        }
    }

    void EndGame()
    {
        _timerRunning = false;

        timerText.gameObject.SetActive(true);
        int m = (int)(elapsedTime / 60f);
        int s = (int)(elapsedTime % 60f);
        timerText.text = $"Tijd: {m:00}:{s:00}";

        questionText.text = "Gefeliciteerd! Je hebt alles gevonden.";
        questionText.color = _defaultPromptColor;

        paintingContainer.gameObject.SetActive(false);

        if (elapsedTime <= timeLimit)
        {
            GameState.AsantewaaPiece = true;
            Debug.Log("Asantewaa statue piece collected!");
        }
    }

    public void CloseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(SceneName);
    }
}

