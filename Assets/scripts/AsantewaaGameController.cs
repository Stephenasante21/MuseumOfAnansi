// Assets/Scripts/AsantewaaGameController.cs

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

    // internal data
    private PaintingData _data;
    private int _currentStep;

    // timer
    private float _elapsedTime;
    private bool _timerRunning;

    // store the default prompt color so we can reset each question
    private Color _defaultPromptColor;

    void Start()
    {
        // cache the default color
        _defaultPromptColor = questionText.color;

        // timer setup
        _elapsedTime = 0f;
        _timerRunning = true;
        timerText.gameObject.SetActive(false);

        // close button
        closeButton.onClick.AddListener(CloseGame);

        // spawn painting
        var pool = GameSettings.IsHard ? hardPaintings : easyPaintings;
        var prefab = pool[Random.Range(0, pool.Length)];
        var go = Instantiate(prefab, paintingContainer, false);

        // setup hotspots
        _data = go.GetComponent<PaintingData>();
        for (int i = 0; i < _data.hotspots.Length; i++)
        {
            int idx = i;
            _data.hotspots[idx].onClick.RemoveAllListeners();
            _data.hotspots[idx].onClick.AddListener(() => OnHotspotClicked(idx));
        }

        // start
        _currentStep = 0;
        ShowStep();
    }

    void Update()
    {
        if (_timerRunning)
            _elapsedTime += Time.deltaTime;
    }

    void ShowStep()
    {
        // reset prompt color
        questionText.color = _defaultPromptColor;

        var step = _data.steps[_currentStep];
        questionText.text = $"{step.twiPrompt}";

        // re-enable all buttons
        foreach (var btn in _data.hotspots)
            btn.interactable = true;
    }

    void OnHotspotClicked(int idx)
    {
        var step = _data.steps[_currentStep];

        if (idx == step.correctHotspot)
        {
            // correct — green & advance
            questionText.color = Color.green;
            foreach (var b in _data.hotspots) b.interactable = false;
            Invoke(nameof(AdvanceStep), 1f);
        }
        else
        {
            // wrong — flash red, but keep it clickable
            questionText.color = Color.red;
            // no more: _data.hotspots[idx].interactable = false;

            // reset back after 0.5s
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
            EndGame();
        else
            ShowStep();
    }

    void EndGame()
    {
        _timerRunning = false;

        timerText.gameObject.SetActive(true);
        int minutes = (int)(_elapsedTime / 60f);
        int seconds = (int)(_elapsedTime % 60f);
        timerText.text = $"Tijd: {minutes:00}:{seconds:00}";

        questionText.text = "Gefeliciteerd! Je hebt alles gevonden.";
        questionText.color = _defaultPromptColor;

        foreach (var b in _data.hotspots)
            b.gameObject.SetActive(false);
    }

    public void CloseGame()
    {
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
