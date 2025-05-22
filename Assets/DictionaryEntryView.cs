using TMPro;
using UnityEngine;

public class DictionaryEntryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TwiText, NativeText;

    public void Init(DictionaryEntry entry)
    {
        TwiText.text = entry.foreign;
        NativeText.text = entry.native;
    }

}
