using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    // internal state
    private PaintingData _data;
    private int _currentStep;

    // which painting prefab are we on?
    private int _paintingIndex = 0;
    private GameObject _currentPaintingGO;

    // timer
    private float _elapsedTime;
    private bool _timerRunning;

    // for flashing prompt colors
    private Color _defaultPromptColor;

    void Start()
    {
        // cache default prompt color
        _defaultPromptColor = questionText.color;

        // timer init
        _elapsedTime = 0f;
        _timerRunning = true;
        timerText.gameObject.SetActive(false);

        // close button
        closeButton.onClick.AddListener(CloseGame);

        // Spawn the very first painting (index 0)
        SpawnNextPainting();
    }

    void Update()
    {
        if (_timerRunning)
            _elapsedTime += Time.deltaTime;
    }

    void SpawnNextPainting()
    {
        // 1) destroy old painting (if any)
        if (_currentPaintingGO != null)
            Destroy(_currentPaintingGO);

        // 2) pick the right pool
        var pool = GameSettings.IsHard ? hardPaintings : easyPaintings;

        // clamp index
        if (_paintingIndex < 0) _paintingIndex = 0;
        if (_paintingIndex >= pool.Length) _paintingIndex = pool.Length - 1;

        // 3) instantiate the chosen prefab
        _currentPaintingGO = Instantiate(pool[_paintingIndex], paintingContainer, false);

        // 4) grab its data and wire up buttons
        _data = _currentPaintingGO.GetComponent<PaintingData>();
        for (int i = 0; i < _data.hotspots.Length; i++)
        {
            int idx = i;
            _data.hotspots[i].onClick.RemoveAllListeners();
            _data.hotspots[i].onClick.AddListener(() => OnHotspotClicked(idx));
        }

        // 5) reset step counter & show step #0
        _currentStep = 0;
        ShowStep();
    }

    void ShowStep()
    {
        // reset prompt color
        questionText.color = _defaultPromptColor;

        // display current Twi prompt
        var step = _data.steps[_currentStep];
        questionText.text = step.twiPrompt;

        // re‐enable all hotspots
        foreach (var btn in _data.hotspots)
            btn.interactable = true;
    }

    void OnHotspotClicked(int idx)
    {
        var step = _data.steps[_currentStep];
        if (idx == step.correctHotspot)
        {
            DictionaryEntry entry = new DictionaryEntry();
            entry.native = step.nativeMeaning;
            entry.foreign = step.twiPrompt;

            FoundWordsManager.Instance.AddEntry(entry);

            // correct → flash green, disable all, advance after 1s
            questionText.color = Color.green;
            foreach (var b in _data.hotspots) b.interactable = false;
            Invoke(nameof(AdvanceStep), 1f);
        }
        else
        {
            // wrong → flash red, keep clickable, reset color after .5s
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
            // we’ve finished all hotspots in _this_ painting → move on
            _paintingIndex++;
            if (_paintingIndex < easyPaintings.Length ||
                _paintingIndex < hardPaintings.Length)
            {
                SpawnNextPainting();
            }
            else
            {
                // no more paintings → wrap up
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
        // stop timer
        _timerRunning = false;

        // reveal time
        timerText.gameObject.SetActive(true);
        int m = (int)(_elapsedTime / 60f), s = (int)(_elapsedTime % 60f);
        timerText.text = $"Tijd: {m:00}:{s:00}";

        // final message
        questionText.text = "Gefeliciteerd! Je hebt alles gevonden.";
        questionText.color = _defaultPromptColor;

        paintingContainer.gameObject.SetActive(false);
    }

    public void CloseGame()
    {
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
