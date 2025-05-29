using Fungus;
using UnityEngine;

public class GameStatePublisher : MonoBehaviour
{
    [Tooltip("Drag your Flowchart here")]
    [SerializeField] private Flowchart flowchart;

    bool _sentOkomfo;

    void Update()
    {
        // Push each static flag into the matching Fungus Boolean variable:
        flowchart.SetBooleanVariable("OkomfoPiece", GameState.OkomfoPiece);
        flowchart.SetBooleanVariable("HardOkomfoPiece", GameState.HardOkomfoPiece);
        flowchart.SetBooleanVariable("AsantewaaPiece", GameState.AsantewaaPiece);
        flowchart.SetBooleanVariable("HardAsantewaaPiece", GameState.HardAsantewaaPiece);

        if (GameState.OkomfoPiece && !_sentOkomfo)
        {
            flowchart.ExecuteBlock("OkomfoPiece");
            _sentOkomfo = true;
        }

        if (GameState.HardOkomfoPiece && !_sentOkomfo)
        {
            flowchart.ExecuteBlock("HardOkomfoPiece");
            _sentOkomfo = true;
        }

        if (GameState.HardOkomfoPiece && !_sentOkomfo)
        {
            flowchart.ExecuteBlock("HardOkomfoPiece");
            _sentOkomfo = true;
        }

        if (GameState.AsantewaaPiece && !_sentOkomfo)
        {
            flowchart.ExecuteBlock("AsantewaaPiece");
            _sentOkomfo = true;
        }

        if (GameState.HardAsantewaaPiece && !_sentOkomfo)
        {
            flowchart.ExecuteBlock("HardAsantewaaPiece");
            _sentOkomfo = true;
        }
    }
}
