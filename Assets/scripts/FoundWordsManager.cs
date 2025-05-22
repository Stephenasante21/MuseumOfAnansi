using UnityEngine;
using System.Collections.Generic;
using System;

public class FoundWordsManager : MonoBehaviour
{
    public static FoundWordsManager Instance { get; private set; }

    private List<DictionaryEntry> _found = new();
    public static event Action<DictionaryEntry> EntryAdded;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddEntry(DictionaryEntry foundword)
    {
        if (_found.Exists(e => e == foundword)) return;
        _found.Add(foundword);
        Debug.Log($"[FoundWordsManager] Added: {foundword.foreign} → {foundword.native}");
        EntryAdded?.Invoke(foundword);
    }

    // read-only access
    public IReadOnlyList<DictionaryEntry> FoundEntries => _found;

}
