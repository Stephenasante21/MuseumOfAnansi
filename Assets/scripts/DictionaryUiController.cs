using TMPro;
using UnityEngine;

public class DictionaryUiController : MonoBehaviour
{
    public Transform contentContainer;     // drag your ScrollView/Viewport/Content here
    public GameObject entryPrefab;         // drag your DictionaryEntryItem prefab here

    void OnEnable()
    {
        FoundWordsManager.EntryAdded += FoundWordsManager_EntryAdded;
    }

    void OnDisable()
    {
        FoundWordsManager.EntryAdded -= FoundWordsManager_EntryAdded;
    }

    private void FoundWordsManager_EntryAdded(DictionaryEntry newWord)
    {

        var view = Instantiate(entryPrefab, contentContainer, false)
                     .GetComponent<DictionaryEntryView>();
        view.Init(newWord);

    }
}
