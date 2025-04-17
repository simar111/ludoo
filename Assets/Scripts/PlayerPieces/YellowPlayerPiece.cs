using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class YellowPlayerPiece : PlayerPiece, IPunObservable
{
    RollingDice yellowRollingDice;
    PhotonView photonView;
    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        yellowRollingDice = GetComponentInParent<YellowHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.dice == yellowRollingDice && GameManager.gm.numberOfStepsToMove == 6)
                {
                    GameManager.gm.yellowOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.YellowPathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    photonView.RPC("hideSpinners", RpcTarget.All);
                    return;
                }

            }
            if (GameManager.gm.dice == yellowRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                photonView.RPC("hideSpinners", RpcTarget.All);
                MoveSteps(pathParent.YellowPathPoint);

            }

        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data

            stream.SendNext(GameManager.gm.yellowOutPlayers);
         
        }
        else
        {
            GameManager.gm.yellowOutPlayers = (int)stream.ReceiveNext();
           
        }
    }


   

}
