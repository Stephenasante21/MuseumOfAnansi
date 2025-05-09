using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DictionaryEntry
{
    public string foreign;
    public string native;
}

[System.Serializable]
public class DictionaryContainer
{
    public DictionaryEntry[] entries;
}

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance { get; private set; }

    [Header("JSON Assets (TextAsset)")]
    public TextAsset wordsJson;
    public TextAsset sentencesJson; // mag leeg blijven voorlopig

    [HideInInspector]
    public List<DictionaryEntry> words;
    [HideInInspector]
    public List<DictionaryEntry> sentences;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAll();
        }
        else Destroy(gameObject);
    }

    void LoadAll()
    {
        // 1) parse woorden
        var wordsContainer = JsonUtility.FromJson<DictionaryContainer>(wordsJson.text);
        words = new List<DictionaryEntry>(wordsContainer.entries);

        // 2) parse zinnen (indien aanwezig)
        if (sentencesJson != null && !string.IsNullOrEmpty(sentencesJson.text))
        {
            var sentencesContainer = JsonUtility.FromJson<DictionaryContainer>(sentencesJson.text);
            sentences = new List<DictionaryEntry>(sentencesContainer.entries);
        }
        else
        {
            sentences = new List<DictionaryEntry>();
        }
    }

    public List<DictionaryEntry> GetCurrentList()
    {
        if (GameSettings.IsHard && sentences.Count > 0)
            return sentences;
        return words;
    }
}
