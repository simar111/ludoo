using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OfflinePlayerPiece1 : MonoBehaviour
{
    public bool moveNow;
    public bool isReady;
    public int numberOfStepsToMove;
    public int numberOfStepsAlreadyMove;
    public OfflinePathObjectParent1 pathParent;
    Coroutine MovePlayerPiece;
    public OfflinePathPoint1 previousPathPoint;
    public OfflinePathPoint1 CurrentPathPoint;
    private void Awake()
    {
        pathParent = FindAnyObjectByType<OfflinePathObjectParent1>();
    }
    private void Start()
    {
       
    }
    public void MoveSteps(OfflinePathPoint1[] pathPointsToMoveon_)
    {
       MovePlayerPiece= StartCoroutine(MovePlayer(pathPointsToMoveon_));
    }
    public void MoveStepscom(OfflinePathPoint1[] pathPointsToMoveon_)
    {
        MovePlayerPiece = StartCoroutine(MovePlayer(pathPointsToMoveon_));
    }
    public void MakePlayerReadyToMove(OfflinePathPoint1[] pathPointsToMoveon_)
    {
        //if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
        //{
            
        //    OfflineManager2.om.redOutPlayers += 1;
        //}
        //else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
        //{
    
        //    GameManagerOffline.gm.blueOutPlayers += 1;
        //}
        //else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
        //{

        //    OfflineManager2.om.yellowOutPlayers += 1;
        //}
        //else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[3])
        //{

        //    OfflineManager2.om.greenOutPlayers += 1;
        //}

        isReady = true;
        transform.position = pathPointsToMoveon_[0].transform.position;
        numberOfStepsAlreadyMove = 1;
        previousPathPoint = pathPointsToMoveon_[0];
        CurrentPathPoint = pathPointsToMoveon_[0];
        CurrentPathPoint.AddPlayerPiece(this);
        OfflineManager2.om.AddPathPoint(CurrentPathPoint);
        OfflineManager2.om.canDiceRoll = true ;
        OfflineManager2.om.selfDice=true ;
        OfflineManager2.om.transferdice = false ;
    }
    public IEnumerator MovePlayer(OfflinePathPoint1[] pathPointsToMoveon_)
    {
        //OfflineManager2.om.canDiceRoll=false;
        //OfflineManager2.om.selfDice = false;
        OfflineManager2.om.transferdice = false;
        yield return new WaitForSeconds(0.25f);
        numberOfStepsToMove = OfflineManager2.om.numberOfStepsToMove;
        for (int i = numberOfStepsAlreadyMove; i < (numberOfStepsAlreadyMove + numberOfStepsToMove); i++)
        {
            CurrentPathPoint.RescaleAndRepositioningAllPlayer();
            if (isPathPointsAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveon_))
            {
                transform.position = pathPointsToMoveon_[i].transform.position;
                this.GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(0.35f);
            }
        }
        if (isPathPointsAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveon_))
        {
            numberOfStepsAlreadyMove += numberOfStepsToMove;
            OfflineManager2.om.RemovePathPoint(previousPathPoint);
            previousPathPoint.RemovePlayerPiece(this);
            CurrentPathPoint = pathPointsToMoveon_[numberOfStepsAlreadyMove - 1];
           if(CurrentPathPoint.AddPlayerPiece(this))
            {
                if (numberOfStepsAlreadyMove == 57)
                {
                    OfflineManager2.om.selfDice = true ;
                }
                else
                {
                    if (OfflineManager2.om.numberOfStepsToMove != 6)
                    {

                        /*  GameManagerOffline.gm.selfDice = false;*/
                        OfflineManager2.om.transferdice = true;
                        Debug.Log("Do shaam");
                        Debug.Log(OfflineManager2.om.redOutPlayers + "   " + OfflineManager2.om.yellowOutPlayers);
                    }
                    else
                    {
                        OfflineManager2.om.selfDice = true;
                        OfflineManager2.om.transferdice = false;
                    }
                }
            }
            else
            {
                OfflineManager2.om.selfDice = true;
            }
            OfflineManager2.om.AddPathPoint(CurrentPathPoint);
            previousPathPoint = CurrentPathPoint;
            OfflineManager2.om.numberOfStepsToMove = 0;
        }
        OfflineManager2.om.canPlayerMove = true;
        OfflineManager2.om.RollingDiceManager();
        if (MovePlayerPiece != null)
        {
            StopCoroutine("MovePlayer");
        }
    }
    bool isPathPointsAvailableToMove(int numOfSteps,int numOfStepsAlredayMove, OfflinePathPoint1[] pathPointToMove)
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
}
