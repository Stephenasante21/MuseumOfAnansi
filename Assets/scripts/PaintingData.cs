using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PaintingStep
{
    public string twiPrompt;

    public string nativeMeaning;

    public int correctHotspot;
}


public class PaintingData : MonoBehaviour{
    public Button[] hotspots = new Button[4];

    public PaintingStep[] steps = new PaintingStep[4];
}
