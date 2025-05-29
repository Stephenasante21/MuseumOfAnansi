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

        if (GameState.AsantewaaPiece && !_sentAsantewaaPiece)
        {
            flowchart.ExecuteBlock("AsantewaaPiece");
            _sentAsantewaaPiece = true;
        }

        if (GameState.HardAsantewaaPiece && !_sentHardAsantewaaPiece)
        {
            flowchart.ExecuteBlock("HardAsantewaaPiece");
            _sentHardAsantewaaPiece = true;
        }

        bool all = GameState.OkomfoPiece
                && GameState.HardOkomfoPiece
                && GameState.AsantewaaPiece
                && GameState.HardAsantewaaPiece;

        if (all && !_sentAllPieces)
        {
            flowchart.ExecuteBlock("AllPieces");
            _sentAllPieces = true;
        }
    }
}
