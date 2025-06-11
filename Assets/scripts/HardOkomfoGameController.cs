using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HardOkomfoGameController : MonoBehaviour
{
    [Header("Scene Navigation")]
    [SerializeField] private string SceneName;

    [Header("UI References")]
    public TMP_Text questionText;
    public Button[] optionButtons;
    public Button closeButton;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    [Header("Timer")]
    public TMP_Text timerText;
    private float elapsedTime;
    private bool timerRunning;
    public float timeLimit = 45f;

    [Header("Game Settings")]
    public int optionsCount = 4;
    public int maxCorrectAnswers = 5;

    [Header("Audio Clips")]
    [Tooltip("Play this when the player selects a correct answer.")]
    public AudioClip correctSound;

    [Tooltip("Play this when the player selects a wrong answer.")]
    public AudioClip incorrectSound;

    private AudioSource _audioSource;

    private List<SentenceEntry> allSentences;
    private SentenceEntry currentEntry;
    private int correctCount;

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
        closeButton.onClick.AddListener(CloseGame);

        allSentences = DictionaryManager.Instance.GetSentences();
        if (allSentences == null || allSentences.Count == 0)
        {
            Debug.LogError("[HardOkomfo] Geen zinnen geladen in DictionaryManager!");
        }

        timerText.gameObject.SetActive(false);
        elapsedTime = 0f;
        timerRunning = true;

        correctCount = 0;
        NextQuestion();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void NextQuestion()
    {
        // Reset sprites, listeners, and interactability
        foreach (var b in optionButtons)
        {
            b.image.sprite = normalSprite;
            b.interactable = true;
            b.onClick.RemoveAllListeners();
        }

        // Pick a random sentence
        currentEntry = allSentences[Random.Range(0, allSentences.Count)];
        questionText.text = currentEntry.templateTwi;

        // Build a list of wrong options, then insert the correct one
        var pool = allSentences
            .Select(e => e.missingTwi)
            .Where(w => w != currentEntry.missingTwi)
            .OrderBy(_ => Random.value)
            .Take(optionsCount - 1)
            .ToList();

        var options = pool
            .Append(currentEntry.missingTwi)
            .OrderBy(_ => Random.value)
            .ToList();

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < optionsCount)
            {
                var btn = optionButtons[i];
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<TMP_Text>().text = options[i];

                // Re‐wire listener
                int idx = i;
                btn.onClick.AddListener(() => OnOptionSelected(idx));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnOptionSelected(int idx)
    {
        var btn = optionButtons[idx];
        var pick = btn.GetComponentInChildren<TMP_Text>().text;

        if (pick == currentEntry.missingTwi)
        {
            // Build the full sentence
            string fullSentence = currentEntry.templateTwi.Replace("___", currentEntry.missingTwi);
            string translation = currentEntry.translationDutch;

            // Add to FoundWordsManager
            FoundWordsManager.Instance.AddEntry(fullSentence, translation);

            // Play correct‐sound
            if (correctSound != null)
            {
                _audioSource.PlayOneShot(correctSound);
            }

            // Show correct sprite
            btn.image.sprite = correctSprite;
            correctCount++;
            timerRunning = correctCount < maxCorrectAnswers;

            // Display the completed sentence + translation
            questionText.text = fullSentence + "\n\nVertaling:\n" + translation;

            if (correctCount >= maxCorrectAnswers)
            {
                // Delay before showing final results
                Invoke(nameof(ShowFinal), 2f);
            }
            else
            {
                // Move on to the next question after a short pause
                Invoke(nameof(NextQuestion), 2f);
            }
        }
        else
        {
            // Play incorrect‐sound
            if (incorrectSound != null)
            {
                _audioSource.PlayOneShot(incorrectSound);
            }

            // Show wrong sprite and disable this button
            btn.image.sprite = wrongSprite;
            btn.interactable = false;
        }
    }

    void ShowFinal()
    {
        timerText.gameObject.SetActive(true);
        int m = (int)(elapsedTime / 60f);
        int s = (int)(elapsedTime % 60f);
        timerText.text = $"Tijd: {m:00}:{s:00}";

        questionText.text = $"Gefeliciteerd!\nJe hebt {correctCount} zinnen goed.";

        // Hide all options
        foreach (var b in optionButtons)
        {
            b.gameObject.SetActive(false);
        }

        if (elapsedTime <= timeLimit)
        {
            GameState.HardOkomfoPiece = true;
            Debug.Log("✅ Hard Okomfo statue piece collected!");
        }
    }

    public void CloseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
