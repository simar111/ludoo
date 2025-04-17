using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OfflinePlayerPiece : MonoBehaviour
{
    public bool moveNow;
    public bool isReady;
    public int numberOfStepsToMove;
    public int numberOfStepsAlreadyMove;
    public OfflinePathObjectParent pathParent;

    Coroutine MovePlayerPiece;

    public OfflinePathPoint previousPathPoint;
    public OfflinePathPoint CurrentPathPoint;


    private void Awake()
    {
        pathParent = FindAnyObjectByType<OfflinePathObjectParent>();
    }

    private void Start()
    {
       
    }

 

    public void MoveSteps(OfflinePathPoint[] pathPointsToMoveon_)
    {
       MovePlayerPiece= StartCoroutine(MovePlayer(pathPointsToMoveon_));
    }
    public void MakePlayerReadyToMove(OfflinePathPoint[] pathPointsToMoveon_)
    {
       /* if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
        {
            
            GameManagerOffline.gm.redOutPlayers += 1;
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
        {
    
            GameManagerOffline.gm.blueOutPlayers += 1;
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[2])
        {
         
            GameManagerOffline.gm.yellowOutPlayers += 1;
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[3])
        {
    
            GameManagerOffline.gm.greenOutPlayers += 1;
        }*/

        isReady = true;
        transform.position = pathPointsToMoveon_[0].transform.position;
        numberOfStepsAlreadyMove = 1;


        previousPathPoint = pathPointsToMoveon_[0];
        CurrentPathPoint = pathPointsToMoveon_[0];
        CurrentPathPoint.AddPlayerPiece(this);
        GameManagerOffline.gm.AddPathPoint(CurrentPathPoint);

        GameManagerOffline.gm.canDiceRoll = true ;
        GameManagerOffline.gm.selfDice=true ;
        GameManagerOffline.gm.transferdice = false ;

    }
    public IEnumerator MovePlayer(OfflinePathPoint[] pathPointsToMoveon_)
    {
        GameManagerOffline.gm.transferdice = false;
        yield return new WaitForSeconds(0.25f);
        numberOfStepsToMove = GameManagerOffline.gm.numberOfStepsToMove;
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
          

            GameManagerOffline.gm.RemovePathPoint(previousPathPoint);
            previousPathPoint.RemovePlayerPiece(this);
            CurrentPathPoint = pathPointsToMoveon_[numberOfStepsAlreadyMove - 1];

           if(CurrentPathPoint.AddPlayerPiece(this))
            {
                if (numberOfStepsAlreadyMove == 57)
                {
                    GameManagerOffline.gm.selfDice = true ;
                }
                else
                {
                    if (GameManagerOffline.gm.numberOfStepsToMove != 6)
                    {

                        /*  GameManagerOffline.gm.selfDice = false;*/
                        GameManagerOffline.gm.transferdice = true;
                        Debug.Log("Do shaam");
                        Debug.Log(GameManagerOffline.gm.redOutPlayers + "   " + GameManagerOffline.gm.yellowOutPlayers);
                    }
                    else
                    {
                        GameManagerOffline.gm.selfDice = true;
                        GameManagerOffline.gm.transferdice = false;
                    }
                }
            }
            else
            {
                GameManagerOffline.gm.selfDice = true;
            }


            GameManagerOffline.gm.AddPathPoint(CurrentPathPoint);
            previousPathPoint = CurrentPathPoint;

           
            GameManagerOffline.gm.numberOfStepsToMove = 0;

        }
           
        GameManagerOffline.gm.canPlayerMove = true;

        GameManagerOffline.gm.RollingDiceManager();

        if (MovePlayerPiece != null)
        {
            StopCoroutine("MovePlayer");
        }
    }

    bool isPathPointsAvailableToMove(int numOfSteps,int numOfStepsAlredayMove, OfflinePathPoint[] pathPointToMove)
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
