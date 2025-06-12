using UnityEngine;

public class StatuePieceController : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces = new GameObject[4];

    void Start()
    {
        // bij start staan ze allemaal uit
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null) pieces[i].SetActive(false);
        }
    }

    void Update()
    {
        // zodra de corresponderende GameState-flag true wordt, zetten we dat stuk aan
        if (GameState.OkomfoPiece && !pieces[0].activeSelf)
            pieces[0].SetActive(true);

        if (GameState.HardOkomfoPiece && !pieces[1].activeSelf)
            pieces[1].SetActive(true);

        if (GameState.AsantewaaPiece && !pieces[2].activeSelf)
            pieces[2].SetActive(true);

        if (GameState.HardAsantewaaPiece && !pieces[3].activeSelf)
            pieces[3].SetActive(true);
    }
}
