using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private Button audioButton;
    [SerializeField] private AudioSource audioSource;
    [Tooltip("Name of the file in StreamingAssets/clips, including its extension (e.g. \"test.mp3\").")]
    public string audioClipName;

    private Coroutine currentRoutine;

    private void Awake()
    {
        audioButton.onClick.AddListener(PlayAudio);
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnDisable()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        audioSource.clip = null; // <-- Dit verwijdert de geladen clip
        audioButton.interactable = true;
    }

    private void PlayAudio()
    {
        if (currentRoutine != null) return;
        currentRoutine = StartCoroutine(PlayFromStreamingAssets());
    }

    public void Stop()
    {
        // stop current clip & coroutine
        if (audioSource.isPlaying) audioSource.Stop();
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
        audioButton.interactable = true;
    }

    private IEnumerator PlayFromStreamingAssets()
    {
        audioButton.interactable = false;
        Debug.Log(audioClipName);
        // Build URL
        string path = Path.Combine(Application.streamingAssetsPath, "clips", audioClipName);
        string url = path.Contains("://") ? path : "file:///" + path;

        // Pick correct AudioType
        AudioType type;
        switch (Path.GetExtension(audioClipName).ToLower())
        {
            case ".mp3": type = AudioType.MPEG; break;
            case ".wav": type = AudioType.WAV; break;
            case ".ogg": type = AudioType.OGGVORBIS; break;
            default: type = AudioType.UNKNOWN; break;
        }

        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, type))
        {
            yield return uwr.SendWebRequest();

            #if UNITY_2020_1_OR_NEWER
            if (uwr.result != UnityWebRequest.Result.Success)
            #else
            if (uwr.isNetworkError || uwr.isHttpError)
            #endif
            {
                Debug.LogError($"AudioPlayer: failed to load {url}\n{uwr.error}");
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitUntil(() => !audioSource.isPlaying);
            }
        }

        audioButton.interactable = true;
        currentRoutine = null;
    }
}
