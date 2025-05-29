using Fungus;
using UnityEngine;

public class GameStatePublisher : MonoBehaviour
{
    [Tooltip("Drag your Flowchart here")]
    [SerializeField] private Flowchart flowchart;

    bool _sentOkomfoPiece;
    bool _sentHardOkomfoPiece;
    bool _sentAsantewaaPiece;
    bool _sentHardAsantewaaPiece;
    bool _sentAllPieces;

    void Update()
    {
        // Push each static flag into the matching Fungus Boolean variable:
        flowchart.SetBooleanVariable("OkomfoPiece", GameState.OkomfoPiece);
        flowchart.SetBooleanVariable("HardOkomfoPiece", GameState.HardOkomfoPiece);
        flowchart.SetBooleanVariable("AsantewaaPiece", GameState.AsantewaaPiece);
        flowchart.SetBooleanVariable("HardAsantewaaPiece", GameState.HardAsantewaaPiece);

        if (GameState.OkomfoPiece && !_sentOkomfoPiece)
        {
            flowchart.ExecuteBlock("OkomfoPiece");
            _sentOkomfoPiece = true;
        }

        if (GameState.HardOkomfoPiece && !_sentHardOkomfoPiece)
        {
            flowchart.ExecuteBlock("HardOkomfoPiece");
            _sentHardOkomfoPiece = true;
        }

        if (GameState.HardOkomfoPiece && !_sentAsantewaaPiece)
        {
            flowchart.ExecuteBlock("HardOkomfoPiece");
            _sentAsantewaaPiece = true;
        }

        if (GameState.AsantewaaPiece && !_sentHardAsantewaaPiece)
        {
            flowchart.ExecuteBlock("AsantewaaPiece");
            _sentHardAsantewaaPiece = true;
        }

        if (GameState.HardAsantewaaPiece && !_sentAllPieces)
        {
            flowchart.ExecuteBlock("HardAsantewaaPiece");
            _sentAllPieces = true;
        }
    }
}
