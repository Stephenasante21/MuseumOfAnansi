using TMPro;
using UnityEngine;

public class DictionaryEntryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TwiText, NativeText;
    [SerializeField] private AudioPlayer audioplayer;
    public void Init(DictionaryEntry entry)
    {
        TwiText.text = entry.foreign;
        NativeText.text = entry.native;
        audioplayer.audioClipName = entry.audioclip;
    }

}
