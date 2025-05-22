using TMPro;
using UnityEngine;

public class DictionaryUiController : MonoBehaviour
{
    public Transform contentContainer;     // drag your ScrollView/Viewport/Content here
    public GameObject entryPrefab;         // drag your DictionaryEntryItem prefab here

    void OnEnable()
    {
        // whenever this panel is enabled, rebuild the list
        FoundWordsManager.EntryAdded += FoundWordsManager_EntryAdded;

    }

    void OnDisable()
    {
        // whenever this panel is enabled, rebuild the list
        FoundWordsManager.EntryAdded -= FoundWordsManager_EntryAdded;

    }

    private void FoundWordsManager_EntryAdded(DictionaryEntry newWord)
    {

        Debug.Log($"FoundEntries count: {FoundWordsManager.Instance.FoundEntries.Count}");

        // instantiate under Content
        DictionaryEntryView go = Instantiate(entryPrefab, contentContainer, false).GetComponent<DictionaryEntryView>();

        go.Init(newWord);

    }
}
