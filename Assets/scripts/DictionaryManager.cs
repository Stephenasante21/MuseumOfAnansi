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
public class SentenceEntry
{
    public int id;
    public string templateTwi;      
    public string missingTwi;       
    public string translationDutch; 
}

[System.Serializable]
public class DictionaryContainer
{
    public DictionaryEntry[] entries;
}

[System.Serializable]
public class SentenceContainer
{
    public SentenceEntry[] entries;
}

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance { get; private set; }

    [Header("JSON Assets")]
    public TextAsset wordsJson;      
    public TextAsset sentencesJson;  

    private List<DictionaryEntry> words;
    private List<SentenceEntry> sentences;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAll()
    {
        if (wordsJson != null && !string.IsNullOrEmpty(wordsJson.text))
        {
            var wCont = JsonUtility.FromJson<DictionaryContainer>(wordsJson.text);
            words = new List<DictionaryEntry>(wCont.entries);
        }
        else
        {
            words = new List<DictionaryEntry>();
            Debug.LogWarning("[DictionaryManager] wordsJson is leeg of niet toegewezen.");
        }

        if (sentencesJson != null && !string.IsNullOrEmpty(sentencesJson.text))
        {
            var sCont = JsonUtility.FromJson<SentenceContainer>(sentencesJson.text);
            sentences = new List<SentenceEntry>(sCont.entries);
            Debug.Log(sentences.Count);
        }
        else
        {
            sentences = new List<SentenceEntry>();
            Debug.LogWarning("[DictionaryManager] sentencesJson is leeg of niet toegewezen.");
        }
    }

    public List<DictionaryEntry> GetWords()
    {
        return words;
    }

    public List<SentenceEntry> GetSentences()
    {
        return sentences;
    }

    public DictionaryEntry GetWordById(int id)
    {
        return words.FirstOrDefault(e => e.id == id);
    }
}
