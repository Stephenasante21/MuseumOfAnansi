// Assets/Scripts/WordChoiceGameController.cs

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class WordChoiceGameController : MonoBehaviour
{

    [Header("Opties")]
    public Transform optionsParent;  
    public int optionsCount = 8;      


    [Header("UI References")]
    public TMP_Text questionText;
    public Button[] optionButtons;        // 8 buttons
    public Transform answerPlaceholder;   // waar de gekozen knop naartoe verhuist

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    DictionaryEntry currentEntry;

    void Start()
    {
        SetupQuestion();
    }

    void SetupQuestion()
    {
        var list = DictionaryManager.Instance.GetCurrentList();
        currentEntry = list[Random.Range(0, list.Count)];

        // Vraag toont native (English) woord
        questionText.text = $"Kies het Twi-woord voor:\n\"{currentEntry.native}\"";

        // Distractors uit pool (zonder het correcte foreign)
        var pool = list.Select(e => e.foreign).Where(f => f != currentEntry.foreign).ToList();
        var wrongs = pool.OrderBy(_ => Random.value)
                         .Take(optionsCount - 1)
                         .ToList();

        // Bouw de opties-lijst
        var options = new List<string>(wrongs) { currentEntry.foreign };
        options = options.OrderBy(_ => Random.value).ToList();

        // Vul alleen de eerste optionsCount knoppen, de rest *hide*
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
            // Correct: verplaats en kleur groen
            btn.transform.SetParent(answerPlaceholder, false);
            var rt = btn.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            btn.image.sprite = correctSprite;

            // blokkeer alle knoppen
            foreach (var b in optionButtons)
                b.interactable = false;

            // na 1.5s nieuwe vraag
            Invoke(nameof(NextQuestion), 1.5f);
        }
        else
        {
            // Fout: kleur rood en zet knop uit
            btn.image.sprite = wrongSprite;
            btn.interactable = false;
            // geen Invoke, de vraag blijft
        }
    }

    void NextQuestion()
    {
        // 1) Reset alle knoppen terug in OptionsPanel
        foreach (var b in optionButtons)
        {
            b.transform.SetParent(optionsParent, false);
            b.image.sprite = normalSprite;
            b.interactable = true;
        }

        // 2) Stel nieuwe vraag in
        SetupQuestion();
    }
}
