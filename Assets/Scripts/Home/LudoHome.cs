using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LudoHome : MonoBehaviour
{
    public PlayerPiece[] playerPieces;
    public RollingDice rollingDice;


    public GameObject speaker;

    private void Start()
    {
            Instantiate(speaker, this.transform);
    }

    [PunRPC]
    public void hideSpinners()
    {
        foreach (var op in this.playerPieces)
        {
            Debug.LogWarning(op.transform.GetChild(1).gameObject.name);
            op.transform.GetChild(1).gameObject.SetActive(false);
        }
    }



}
