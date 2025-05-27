using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEngine.EventSystems.EventTrigger;

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

    public void AddEntry(DictionaryEntry entry)
    {
        if (_found.Exists(e => e.id == entry.id && entry.id != 0)) return;
        if (_found.Exists(e => e.foreign == entry.foreign)) return;
        _found.Add(entry);
        EntryAdded?.Invoke(entry);
    }

    
    public void AddEntry(string foreign, string native)
    {
        var tmp = new DictionaryEntry
        {
            id = 0,                  
            foreign = foreign,
            native = native,
            audioclip = null
        };
        AddEntry(tmp);
    }

    // read-only access
    public IReadOnlyList<DictionaryEntry> FoundEntries => _found;

}
