using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public int maxCorrectAnswers = 5;

    private DictionaryEntry currentEntry;
    private int correctCount = 0;

    void Start()
    {

        closeButton.onClick.AddListener(CloseGame);

        Cursor.lockState = CursorLockMode.None;
        // hide timer at first
        timerText.gameObject.SetActive(false);

        // start timing but don't show until end
        elapsedTime = 0f;
        timerRunning = true;

        SetupQuestion();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void SetupQuestion()
    {
        var list = DictionaryManager.Instance.GetWords();
        currentEntry = list[Random.Range(0, list.Count)];

        questionText.text = $"Kies het Twi-woord voor:\n\"{currentEntry.native}\"";

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
            FoundWordsManager.Instance.AddEntry(
                currentEntry
            );

            // correct
            btn.image.sprite = correctSprite;
            foreach (var b in optionButtons) b.interactable = false;

            correctCount++;
            if (correctCount >= maxCorrectAnswers)
            {
                // stop timer and show results
                timerRunning = false;
                ShowFinalTimeAndEnd();
                return;
            }
            Invoke(nameof(NextQuestion), 2f);
        }
        else
        {
            // wrong
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
        // reveal timer
        timerText.gameObject.SetActive(true);

        // format and set final time
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        timerText.text = $"Tijd: {minutes:00}:{seconds:00}";

        // show end-game message
        questionText.text = $"Gefeliciteerd! Je hebt {correctCount} goed.\nKlik om verder te gaan.";

        // hide options
        foreach (var b in optionButtons) b.gameObject.SetActive(false);

               
        if (elapsedTime <= timeLimit)
        {
            PlayerPrefs.SetInt("OkomfoHardUnlocked", 1);
            PlayerPrefs.Save();
            Debug.Log("OkomfoHard vrijgespeeld!");
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
