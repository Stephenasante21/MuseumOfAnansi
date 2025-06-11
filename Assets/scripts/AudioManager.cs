using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Background Music Clips")]
    public List<SceneMusic> bgmSettings;

    [Header("SFX Audio Source")]
    public AudioSource sfxSource;

    [Header("BGM Audio Source")]
    public AudioSource bgmSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // when *any* scene goes away, re-play the BGM for
        // whichever scene is now active
        var active = SceneManager.GetActiveScene();
        PlayBGMForScene(active.name);
    }

    public void PlayBGMForScene(string sceneName)
    {
        var setting = bgmSettings.Find(x =>
            x.sceneName.Equals(sceneName, StringComparison.OrdinalIgnoreCase)
        );
        if (setting != null && setting.clip != null)
        {
            if (bgmSource.clip == setting.clip && bgmSource.isPlaying)
                return; // already playing the right BGM

            bgmSource.clip = setting.clip;
            bgmSource.loop = setting.loop;
            bgmSource.volume = setting.volume;
            bgmSource.Play();
        }
        else
        {
            // no BGM entry? just stop
            bgmSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    [Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        public bool loop = true;
    }
}
