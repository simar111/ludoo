using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class RedPlayerPiece : PlayerPiece,IPunObservable
{
    RollingDice redRollingDice;
    PhotonView photonView;
    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        redRollingDice = GetComponentInParent<RedHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.dice == redRollingDice && GameManager.gm.numberOfStepsToMove==6)
                {
                    GameManager.gm.redOutPlayers += 1;
                   MakePlayerReadyToMove(pathParent.RedPathPoint);
                    photonView.RPC("hideSpinners", RpcTarget.All);
                    GameManager.gm.numberOfStepsToMove=0;
                    return;
                }
               
            }
            if (GameManager.gm.dice == redRollingDice && isReady && GameManager.gm.canPlayerMove){
                GameManager.gm.canPlayerMove = false;
                photonView.RPC("hideSpinners", RpcTarget.All);
                MoveSteps(pathParent.RedPathPoint);
           
            }
          
        }
    }




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data

            stream.SendNext(GameManager.gm.redOutPlayers);
            
        }
        else
        {
            GameManager.gm.redOutPlayers = (int)stream.ReceiveNext();
            
        }
    }

 

}
