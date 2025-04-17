using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GreenPlayerPiece : PlayerPiece,IPunObservable
{
    RollingDice greenRollingDice;
    PhotonView photonView;
    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        greenRollingDice = GetComponentInParent<GreenHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.dice == greenRollingDice && GameManager.gm.numberOfStepsToMove == 6)
                {
                    GameManager.gm.greenOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.GreenPathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    photonView.RPC("hideSpinners", RpcTarget.All);
                    return;
                }

            }
            if (GameManager.gm.dice == greenRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                photonView.RPC("hideSpinners", RpcTarget.All);
                MoveSteps(pathParent.GreenPathPoint);
            }

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data

            stream.SendNext(GameManager.gm.greenOutPlayers);
        }
        else
        {
            GameManager.gm.greenOutPlayers = (int)stream.ReceiveNext();


        }
    }


}
