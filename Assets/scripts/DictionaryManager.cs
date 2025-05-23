using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DictionaryEntry
{
    public int id;
    public string foreign;
    public string native;
    public string audioclip;
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
    public TextAsset sentencesJson; 

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
        var wordsContainer = JsonUtility.FromJson<DictionaryContainer>(wordsJson.text);
        words = new List<DictionaryEntry>(wordsContainer.entries);

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

    public DictionaryEntry GetWordById(int id)
    {
        return words.Where(e => e.id == id).FirstOrDefault();
    }
}
