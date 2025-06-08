using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;            // Zorg dat TextMeshPro namespace hier staat
using System.Linq;
using UnityEngine.SceneManagement;

public class WordChoiceGameController : MonoBehaviour
{
    [Header("Scene Navigation")]
    [SerializeField] private string SceneName;

    [Header("Opties")]
    public Transform optionsParent;
    public int optionsCount = 8;

    [Header("UI References")]
    public TMP_Text questionText;
    public TMP_Text timerText;
    public Button[] optionButtons;
    public Button closeButton;

    [Header("Vorige Tijd (UI)")]
    public TMP_Text previousTimeText;   // <-- Sleep hier straks “PrevTimeText” uit de hiërarchie

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    [Header("Timer")]
    private float elapsedTime;
    private bool timerRunning;
    public float timeLimit = 45f;

    [Header("Game Settings")]
    public int maxCorrectAnswers = 5;

    [Header("Audio Clips")]
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    private AudioSource _audioSource;
    private DictionaryEntry currentEntry;
    private int correctCount = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        closeButton.onClick.AddListener(CloseGame);

        // 1) Lees de "previous time" in vanuit PlayerManager:
        float prevTime = PlayerManager.Instance.OkomfoTime;
        if (prevTime >= 0f)
        {
            previousTimeText.text = $"Vorige tijd: {prevTime:F2} s";
        }
        else
        {
            previousTimeText.text = "Vorige tijd: nog niet gespeeld";
        }

        Cursor.lockState = CursorLockMode.None;

        timerText.gameObject.SetActive(false);
        elapsedTime = 0f;
        timerRunning = true;

        SetupQuestion();
    }

    void Update()
    {
        if (timerRunning)
            elapsedTime += Time.deltaTime;
    }

    void SetupQuestion()
    {
        var list = DictionaryManager.Instance.GetWords();
        currentEntry = list[Random.Range(0, list.Count)];

        questionText.text = $"Kies het Twi woord voor:\n\"{currentEntry.native}\"";

        // Bouw een pool van verkeerde antwoorden
        var pool = list.Select(e => e.foreign)
                       .Where(f => f != currentEntry.foreign)
                       .ToList();

        var wrongs = pool.OrderBy(_ => Random.value)
                         .Take(optionsCount - 1)
                         .ToList();

        var options = new List<string>(wrongs) { currentEntry.foreign };
        options = options.OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < optionButtons.Length; i++)
        {
            var btn = optionButtons[i];
            if (i < optionsCount)
            {
                btn.gameObject.SetActive(true);
                var txt = btn.GetComponentInChildren<TMP_Text>();
                txt.text = options[i];
                btn.image.sprite = normalSprite;
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                int idx = i;
                btn.onClick.AddListener(() => OnOptionSelected(idx));
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    void OnOptionSelected(int idx)
    {
        var btn = optionButtons[idx];
        string chosen = btn.GetComponentInChildren<TMP_Text>().text;

        if (chosen == currentEntry.foreign)
        {
            FoundWordsManager.Instance.AddEntry(currentEntry);

            if (correctSound != null)
                _audioSource.PlayOneShot(correctSound);

            btn.image.sprite = correctSprite;
            foreach (var b in optionButtons) b.interactable = false;

            correctCount++;
            if (correctCount >= maxCorrectAnswers)
            {
                timerRunning = false;
                ShowFinalTimeAndEnd();
                return;
            }
            Invoke(nameof(NextQuestion), 2f);
        }
        else
        {
            if (incorrectSound != null)
                _audioSource.PlayOneShot(incorrectSound);

            btn.image.sprite = wrongSprite;
            btn.interactable = false;
        }
    }

    void NextQuestion()
    {
        foreach (var b in optionButtons)
        {
            b.transform.SetParent(optionsParent, false);
            b.image.sprite = normalSprite;
            b.interactable = true;
        }
        SetupQuestion();
    }

    void ShowFinalTimeAndEnd()
    {
        timerText.gameObject.SetActive(true);

        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        timerText.text = $"Tijd: {minutes:00}:{seconds:00}";

        questionText.text = $"Gefeliciteerd! Je hebt {correctCount} goed.\nKlik om verder te gaan.";

        foreach (var b in optionButtons)
            b.gameObject.SetActive(false);

        if (elapsedTime <= timeLimit)
        {
            PlayerPrefs.SetInt("OkomfoHardUnlocked", 1);
            PlayerPrefs.Save();

            // 2) Schrijf de nieuwe tijd weg in PlayerManager:
            PlayerManager.Instance.SetOkomfoTime(elapsedTime);
            Debug.Log($"[WordChoiceGameController] OkomfoTime opgeslagen: {elapsedTime:F2} s");

            GameState.OkomfoPiece = true;
            Debug.Log("Okomfo statue piece collected!");
        }
    }

    public void CloseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
