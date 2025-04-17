using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BluePlayerPiece : PlayerPiece, IPunObservable
{

    RollingDice blueRollingDice;

    PhotonView photonView;
    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        blueRollingDice = GetComponentInParent<BlueHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.dice == blueRollingDice && GameManager.gm.numberOfStepsToMove == 6)
                {
                    GameManager.gm.blueOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.BluePathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    photonView.RPC("hideSpinners", RpcTarget.All);
                    return;
                }

            }
            if (GameManager.gm.dice == blueRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                photonView.RPC("hideSpinners", RpcTarget.All);
                MoveSteps(pathParent.BluePathPoint);

            }

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data

            stream.SendNext(GameManager.gm.blueOutPlayers);
        }
        else
        {
            GameManager.gm.blueOutPlayers = (int)stream.ReceiveNext();


        }
    }


}
