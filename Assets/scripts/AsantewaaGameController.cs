using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class AsantewaaGameController: MonoBehaviour
{
    [SerializeField] private string SceneName;

    public GameObject[] easyPaintings;
    public GameObject[] hardPaintings;

    public Transform paintingContainer;
    public TMP_Text questionText;

    PaintingData _data;
    int _currentStep;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        var pool = GameSettings.IsHard ? hardPaintings : easyPaintings;

        var prefab = pool[Random.Range(0, pool.Length)];

        var go = Instantiate(prefab, paintingContainer, false);


        _data = go.GetComponent<PaintingData>();

        for (int i = 0; i < _data.hotspots.Length; i++)
        {
            int idx = i; 
            _data.hotspots[idx].onClick.RemoveAllListeners();
            _data.hotspots[idx].onClick.AddListener(() => OnHotspotClicked(idx));
        }

        _currentStep = 0;
        ShowStep();
    }
    void ShowStep()
    {
        var step = _data.steps[_currentStep];
        questionText.text = $"{step.twiPrompt}";

        foreach (var btn in _data.hotspots)
            btn.interactable = true;
    }

    void OnHotspotClicked(int idx)
    {
        var step = _data.steps[_currentStep];

        if (idx == step.correctHotspot)
        {
            foreach (var b in _data.hotspots)
                b.interactable = false;

            Invoke(nameof(AdvanceStep), 1f);
        }
        else
        {
            _data.hotspots[idx].interactable = false;
        }
    }

    void AdvanceStep()
    {
        _currentStep++;
        if (_currentStep >= _data.steps.Length)
        {
            // all done!
            CloseGame();
        }
        else
        {
            ShowStep();
        }
    }

    public void CloseGame()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(SceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
