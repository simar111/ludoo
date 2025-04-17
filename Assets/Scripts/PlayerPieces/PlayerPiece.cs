using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExitGames.Client.Photon;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[Serializable] public class PlayerPiece : MonoBehaviourPunCallbacks
{
    public bool moveNow;
    public bool isReady;
    public int numberOfStepsToMove;
    public int numberOfStepsAlreadyMove;
    public PathObjectParent pathParent;

    Coroutine MovePlayerPiece;

    public PathPoint previousPathPoint;
    public PathPoint CurrentPathPoint;

/*    public bool isMoving = false;*/

    private void Awake()
    {
        pathParent = FindAnyObjectByType<PathObjectParent>();
/*        PhotonView photonView = GetComponent<PhotonView>();*/

    }


    [PunRPC]
    public void MoveSteps(PathPoint[] pathPointsToMoveon_)
    {
       MovePlayerPiece= StartCoroutine(MovePlayer(pathPointsToMoveon_));
    }

    [PunRPC]
    public void MakePlayerReadyToMove(PathPoint[] pathPointsToMoveon_)
    {
       /* if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
        {
            
            GameManager.gm.redOutPlayers += 1;
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[1])
        {
    
            GameManager.gm.blueOutPlayers += 1;
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
        {
         
            GameManager.gm.yellowOutPlayers += 1;
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[3])
        {
    
            GameManager.gm.greenOutPlayers += 1;
        }*/

        isReady = true;
        transform.position = pathPointsToMoveon_[0].transform.position;
        numberOfStepsAlreadyMove = 1;

        previousPathPoint = pathPointsToMoveon_[0];
        CurrentPathPoint = pathPointsToMoveon_[0];
   /*     if (CurrentPathPoint.GetComponent<PhotonView>().IsMine)*/
            CurrentPathPoint.GetComponent<PhotonView>().RPC("AddPlayerPiece", RpcTarget.AllBuffered, this.tag);
/*        CurrentPathPoint.AddPlayerPiece(this.tag);*/
        GameManager.gm.AddPathPoint(CurrentPathPoint);

        GameManager.gm.canDiceRoll = true ;
        GameManager.gm.selfDice=true ;
        GameManager.gm.transferdice = false ;

    }

    public IEnumerator MovePlayer(PathPoint[] pathPointsToMoveon_)
    {
        GameManager.gm.transferdice = false;
        yield return new WaitForSeconds(0.25f);
        numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;
       
        for (int i = numberOfStepsAlreadyMove; i < (numberOfStepsAlreadyMove + numberOfStepsToMove); i++)
        {
        
          /*  CurrentPathPoint.GetComponent<PhotonView>().RPC("RescaleAndRepositioningAllPlayer",RpcTarget.AllBuffered);*/
          CurrentPathPoint.RescaleAndRepositioningAllPlayer();
            if (isPathPointsAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveon_))
            {
                transform.position = pathPointsToMoveon_[i].transform.position;
                this.GetComponentInParent<PhotonView>().RPC("PlayerSound", RpcTarget.All, this.tag);
                yield return new WaitForSeconds(0.35f);
            }

        }
        if (isPathPointsAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveon_))
        {
           
            numberOfStepsAlreadyMove += numberOfStepsToMove;
          

            GameManager.gm.RemovePathPoint(previousPathPoint);
            previousPathPoint.GetComponent<PhotonView>().RPC("RemovePlayerPiece",RpcTarget.AllBuffered,this.tag);
            CurrentPathPoint = pathPointsToMoveon_[numberOfStepsAlreadyMove - 1];
           /* if (CurrentPathPoint.GetComponent<PhotonView>().IsMine)*/
                CurrentPathPoint.GetComponent<PhotonView>().RPC("AddPlayerPiece", RpcTarget.AllBuffered, this.tag);
            if (CurrentPathPoint.returnTurn)
            {
                if (numberOfStepsAlreadyMove == 57)
                {
                    GameManager.gm.selfDice = true ;
                }
                else
                {
                    if (GameManager.gm.numberOfStepsToMove != 6)
                    {

                        /*  GameManager.gm.selfDice = false;*/
                        GameManager.gm.transferdice = true;
                        Debug.Log("Do shaam");
                        Debug.Log(GameManager.gm.redOutPlayers + "   " + GameManager.gm.yellowOutPlayers);
                    }
                    else
                    {
                        GameManager.gm.selfDice = true;
                        GameManager.gm.transferdice = false;
                    }
                }
            }
            else
            {
                GameManager.gm.selfDice = true;
            }


            GameManager.gm.AddPathPoint(CurrentPathPoint);
            previousPathPoint = CurrentPathPoint;

           
            GameManager.gm.numberOfStepsToMove = 0;

        }
           
        GameManager.gm.canPlayerMove = true;

        GameManager.gm.RollingDiceManager();

        if (MovePlayerPiece != null)
        {
            StopCoroutine("MovePlayer");
        }
    }

    bool isPathPointsAvailableToMove(int numOfSteps,int numOfStepsAlredayMove, PathPoint[] pathPointToMove)
    {
        if (numOfSteps == 0)
        {
            return false;
        }
        int leftNumOfPath=pathPointToMove.Length-numOfStepsAlredayMove;
        if (leftNumOfPath >= numOfSteps)
            return true;
        else
            return false;
    }

/*    public static byte[] Serialize(object obj)
    {


        PlayerPiece piece = (PlayerPiece)obj;
        var json = JsonConvert.SerializeObject(piece);

         return json.ToString().getBytes(theCharset);
    }
*/
    // Deserialize a byte array back to a PlayerPiece object
    public static object Deserialize(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (PlayerPiece)formatter.Deserialize(ms);
        }
    }



}
