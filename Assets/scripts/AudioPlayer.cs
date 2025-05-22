using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private Button audioButton;
    [SerializeField] private AudioSource audioSource;
    public string audioClipName;

    private Coroutine currentRoutine;

    private void Awake()
    {
        audioButton.onClick.AddListener(PlayAudio);
    }

    private void PlayAudio()
    {
        if (currentRoutine != null)
            return;

        currentRoutine = StartCoroutine(PlayFromStreamingAssets());
    }

    private IEnumerator PlayFromStreamingAssets()
    {
        audioButton.interactable = false;

        var filePath = Path.Combine(Application.streamingAssetsPath, "clips", audioClipName);
        var url = filePath.Contains("://") ? filePath : "file://" + filePath;

        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"AudioPlayer: Unable to load clip at {url}\n{uwr.error}");
            }
            else
            {
                var clip = DownloadHandlerAudioClip.GetContent(uwr);
                audioSource.PlayOneShot(clip);

                yield return new WaitForSeconds(clip.length);
            }
        }

        audioButton.interactable = true;
        currentRoutine = null;
    }
}
