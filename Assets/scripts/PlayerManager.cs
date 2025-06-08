using UnityEngine;

/// <summary>
/// Singleton die de laatst gespeelde tijden bewaart voor alle minigames.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private float _okomfoTime = -1f;
    private float _hardOkomfoTime = -1f;
    private float _asantewaaTime = -1f;
    private float _hardAsantewaaTime = -1f;

    public float OkomfoTime { get { return _okomfoTime; } }
    public float HardOkomfoTime { get { return _hardOkomfoTime; } }
    public float AsantewaaTime { get { return _asantewaaTime; } }
    public float HardAsantewaaTime { get { return _hardAsantewaaTime; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Haal reeds opgeslagen tijden uit PlayerPrefs:
        _okomfoTime = PlayerPrefs.GetFloat("OkomfoTime", -1f);
        _hardOkomfoTime = PlayerPrefs.GetFloat("HardOkomfoTime", -1f);
        _asantewaaTime = PlayerPrefs.GetFloat("AsantewaaTime", -1f);
        _hardAsantewaaTime = PlayerPrefs.GetFloat("HardAsantewaaTime", -1f);

        Debug.Log($"[PlayerManager Awake] Ingelezen OkomfoTime = {_okomfoTime}");
    }

    public void SetOkomfoTime(float seconds)
    {
        _okomfoTime = seconds;
        PlayerPrefs.SetFloat("OkomfoTime", _okomfoTime);
        PlayerPrefs.Save();
        Debug.Log($"[PlayerManager] OkomfoTime gezet op {_okomfoTime:F2} s");
    }

    public void SetHardOkomfoTime(float seconds)
    {
        _hardOkomfoTime = seconds;
        PlayerPrefs.SetFloat("HardOkomfoTime", _hardOkomfoTime);
        PlayerPrefs.Save();
        Debug.Log($"[PlayerManager] HardOkomfoTime gezet op {_hardOkomfoTime:F2} s");
    }

    public void SetAsantewaaTime(float seconds)
    {
        _asantewaaTime = seconds;
        PlayerPrefs.SetFloat("AsantewaaTime", _asantewaaTime);
        PlayerPrefs.Save();
        Debug.Log($"[PlayerManager] AsantewaaTime gezet op {_asantewaaTime:F2} s");
    }

    public void SetHardAsantewaaTime(float seconds)
    {
        _hardAsantewaaTime = seconds;
        PlayerPrefs.SetFloat("HardAsantewaaTime", _hardAsantewaaTime);
        PlayerPrefs.Save();
        Debug.Log($"[PlayerManager] HardAsantewaaTime gezet op {_hardAsantewaaTime:F2} s");
    }

    public float GetTotalTimeAllMinigames()
    {
        if (_okomfoTime < 0f && _hardOkomfoTime < 0f &&
            _asantewaaTime < 0f && _hardAsantewaaTime < 0f)
            return -1f;

        float total = 0f;
        if (_okomfoTime > 0f) total += _okomfoTime;
        if (_hardOkomfoTime > 0f) total += _hardOkomfoTime;
        if (_asantewaaTime > 0f) total += _asantewaaTime;
        if (_hardAsantewaaTime > 0f) total += _hardAsantewaaTime;
        return total;
    }
}
