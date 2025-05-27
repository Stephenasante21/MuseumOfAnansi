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

    [Header("Game Settings")]
    public int optionsCount = 4;  
    public int maxCorrectAnswers = 5;

    private List<SentenceEntry> allSentences;
    private SentenceEntry currentEntry;
    private int correctCount;


    void Start()
    {

        closeButton.onClick.AddListener(CloseGame);

        allSentences = DictionaryManager.Instance.GetSentences();
        if (allSentences == null || allSentences.Count == 0)
            Debug.LogError("[HardOkomfo] Geen zinnen geladen in DictionaryManager!");   

        timerText.gameObject.SetActive(false);
        elapsedTime = 0f;
        timerRunning = true;

        correctCount = 0;
        NextQuestion();
    }

    void Update()
    {
        if (timerRunning)
            elapsedTime += Time.deltaTime;
    }

    void NextQuestion()
    {
        foreach (var b in optionButtons)
        {
            b.image.sprite = normalSprite;
            b.interactable = true;
            b.onClick.RemoveAllListeners();
        }

        currentEntry = allSentences[Random.Range(0, allSentences.Count)];
        questionText.text = currentEntry.templateTwi;

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
            // bouw de volledige zin
            string fullSentence = currentEntry.templateTwi.Replace("___", currentEntry.missingTwi);
            string translation = currentEntry.translationDutch;

            // voeg ‘m toe als een DictionaryEntry via de nieuwe overload
            FoundWordsManager.Instance.AddEntry(fullSentence, translation);

            // feedback, verder spelverloop…
            btn.image.sprite = correctSprite;
            correctCount++;
            timerRunning = correctCount < maxCorrectAnswers;

            questionText.text = fullSentence
                + "\n\nVertaling:\n" + translation;

            if (correctCount >= maxCorrectAnswers)
                Invoke(nameof(ShowFinal), 2f);
            else
                Invoke(nameof(NextQuestion), 2f);
        }
        else
        {
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

        questionText.text = $"Gefeliciteerd!\nJe hebt {correctCount} zinnen goed.\nKlik om verder te gaan.";

        foreach (var b in optionButtons)
            b.gameObject.SetActive(false);
    }

    public void CloseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
