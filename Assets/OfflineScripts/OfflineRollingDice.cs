using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OfflineRollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer diceAnim;
    [SerializeField] int numberGot;

    Coroutine generateRandomNumber;
    public int outPieces;

    OfflinePathObjectParent pathParent;
    OfflinePlayerPiece[] currentPlayerPieces;
    OfflinePathPoint[] pathPointMoveOn;

    Coroutine MovePlayerPiece;

    OfflinePlayerPiece outPlayerPiece;

    public bool isAllowed = true;

    private void Awake()
    {
        pathParent=FindObjectOfType<OfflinePathObjectParent>();
    }
    private void OnMouseDown()
    {
       generateRandomNumber= StartCoroutine(RollDice());
    }

    public void mouseRoll()
    {
        
        generateRandomNumber = StartCoroutine(RollDice());
    }

    IEnumerator RollDice()
    {
        GameManagerOffline.gm.transferdice = false;
        yield return new WaitForEndOfFrame();
        if (GameManagerOffline.gm.canDiceRoll)
        {
            this.GetComponent<AudioSource>().Play();
            GameManagerOffline.gm.canDiceRoll = false;
            numberSpriteHolder.gameObject.SetActive(false);
            diceAnim.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;

            GameManagerOffline.gm.numberOfStepsToMove = numberGot;
            GameManagerOffline.gm.dice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            diceAnim.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            int nummberGot = GameManagerOffline.gm.numberOfStepsToMove;



            if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
            {
                outPieces = GameManagerOffline.gm.redOutPlayers;
            }
            else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
            {
                outPieces = GameManagerOffline.gm.blueOutPlayers;
            }
            else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[2])
            {
                outPieces = GameManagerOffline.gm.yellowOutPlayers;
            }
            else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[3])
            {
                outPieces = GameManagerOffline.gm.greenOutPlayers;
            }


            if (PlayerCanNotMove())
            {
                yield return new WaitForSeconds(0.3f);
                if (nummberGot != 6)
                {
                    GameManagerOffline.gm.transferdice = true;
                    Debug.Log("Do pahr");
                }
                else
                    GameManagerOffline.gm.selfDice = true;
            }
            else
            {


             

                if (outPieces == 0 && nummberGot != 6)
                {
                    yield return new WaitForSeconds(0.3f);
                    GameManagerOffline.gm.transferdice = true;
                    Debug.Log("Do hath");
                }
                else
                {
                    if(numberGot==6)
                    {
                        showSpinners();
                    }
                    else
                    {
                        showOnlyReadySpinners();
                    }
                    if (outPieces == 0 && nummberGot==6)
                    {
                       /* MakePlayerReadyToMove(0);*/
                    
                    }
                    else if (outPieces == 1 && nummberGot != 6 && GameManagerOffline.gm.canPlayerMove)
                    {
                        int playerPiecePosition = CheckoutPlayer();
                        if (playerPiecePosition >= 0)
                        {
                            GameManagerOffline.gm.canPlayerMove = false;

                            hideSpinners();

                            MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));
                            
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.3f);
                            if (nummberGot != 6)
                            {
                                GameManagerOffline.gm.transferdice = true;
                                Debug.Log("Do kan");
                            }
                            else
                            {
                                GameManagerOffline.gm.selfDice = true;
                            }
                        }

                    }
                    else if (GameManagerOffline.gm.totalPlayerCanPlay == 1 && GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[2])
                    {
                        if (numberGot == 6 && outPieces < 4)
                        {
                            MakePlayerReadyToMove(outPlayerToMove());
                        }
                        else
                        {
                            int playerPiecePosition = CheckoutPlayer();
                            if (playerPiecePosition >= 0)
                            {
                                GameManagerOffline.gm.canPlayerMove = false;
                                MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.3f);
                                if (numberGot != 6)
                                {
                                    GameManagerOffline.gm.transferdice = true;
                                    Debug.Log("Do naak");
                                }
                                else
                                {
                                    GameManagerOffline.gm.selfDice = true;
                                }
                            }
                        }

                    }
                    else
                    {
                        if (CheckoutPlayer() < 0)
                        {
                            yield return new WaitForSeconds(0.3f);
                            if (numberGot != 6)
                            {
                                GameManagerOffline.gm.transferdice = true;
                                Debug.Log("Do baal");
                            }
                            else
                            {
                                GameManagerOffline.gm.selfDice = true;
                            }
                        }
                    }
                }
            }


            /*  if (GameManagerOffline.gm.numberOfStepsToMove != 6 && outPieces == 0)
              {
                  GameManagerOffline.gm.canDiceRoll = true;
                  GameManagerOffline.gm.selfDice = false;
                  GameManagerOffline.gm.transferdice = true;
  *//*
                  yield return new WaitForSeconds(0.3f);*//*

              }*/

            GameManagerOffline.gm.RollingDiceManager();

           

            if (generateRandomNumber != null)
            {
                StopCoroutine(RollDice());
            }
        }
    }

    void hideSpinners()
    {
        if (this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineYellowPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineRedPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineBluePlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineGreenPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

    }

    void showOnlyReadySpinners()
    {

        if (this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineYellowPlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineRedPlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineBluePlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineGreenPlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

    }

    int outPlayerToMove()
    {
        for(int i = 0; i < 4; i++)
        {
            if (!GameManagerOffline.gm.yelloPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return 0;
    }


    void showSpinners()
    {
        if (this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineYellowPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineRedPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineBluePlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineGreenPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

    }
    int CheckoutPlayer()
    {
        if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
        {
            currentPlayerPieces = GameManagerOffline.gm.redPlayerPiece;
            pathPointMoveOn = pathParent.RedPathPoint;
           
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
        {
            currentPlayerPieces = GameManagerOffline.gm.bluePlayerPiece;
            pathPointMoveOn = pathParent.BluePathPoint;
           
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[2])
        {
            currentPlayerPieces = GameManagerOffline.gm.yelloPlayerPiece;
            pathPointMoveOn = pathParent.YellowPathPoint;
         
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[3])
        {
            currentPlayerPieces = GameManagerOffline.gm.greenPlayerPiece;
            pathPointMoveOn = pathParent.GreenPathPoint;
          
        }

        for(int i = 0;i< currentPlayerPieces.Length; i++)
        {
            if (currentPlayerPieces[i].isReady && isPathPointsAvailableToMove(GameManagerOffline.gm.numberOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
            {
                return i;
            }
        }
        return -1;

    }

    public bool PlayerCanNotMove()
    {
        if (outPieces > 0)
        {
            bool canNotMove= false;
            if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
            {
                currentPlayerPieces = GameManagerOffline.gm.redPlayerPiece;
                pathPointMoveOn = pathParent.RedPathPoint;
            }
            else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
            {
                currentPlayerPieces = GameManagerOffline.gm.bluePlayerPiece;
                pathPointMoveOn = pathParent.BluePathPoint;
            }
            else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[2])
            {
                currentPlayerPieces = GameManagerOffline.gm.yelloPlayerPiece;
                pathPointMoveOn = pathParent.YellowPathPoint;
            }
            else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[3])
            {
                currentPlayerPieces = GameManagerOffline.gm.greenPlayerPiece;
                pathPointMoveOn = pathParent.GreenPathPoint;
            }
            for(int i = 0;i< currentPlayerPieces.Length; i++)
            {
                if (currentPlayerPieces[i].isReady)
                {
              
                    if (isPathPointsAvailableToMove(GameManagerOffline.gm.numberOfStepsToMove,currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
                    {
                       
                        return false;
                    }

                }
                else
                {
                    if (!canNotMove)
                    {
                        canNotMove = true;
                    }

                }
            }
            if (canNotMove)
            {
                Debug.LogError("lmlmlmlm");
                return true;
            }
        }
        return false;
    }

    bool isPathPointsAvailableToMove(int numOfSteps, int numOfStepsAlredayMove, OfflinePathPoint[] pathPointToMove)
    {
        if (numOfSteps == 0)
        {
            return false;
        }
        int leftNumOfPath = pathPointToMove.Length - numOfStepsAlredayMove;
        if (leftNumOfPath >= numOfSteps)
            return true;
        else
            return false;
    }


    public void MakePlayerReadyToMove(int position)
    {
        if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
        {
            outPlayerPiece = GameManagerOffline.gm.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint; 
            GameManagerOffline.gm.redOutPlayers += 1;
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
        {
            outPlayerPiece = GameManagerOffline.gm.bluePlayerPiece[position];
            pathPointMoveOn = pathParent.BluePathPoint; 
            GameManagerOffline.gm.blueOutPlayers += 1;
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[2])
        {
            outPlayerPiece = GameManagerOffline.gm.yelloPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint; 
            GameManagerOffline.gm.yellowOutPlayers += 1;
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[3])
        {
            outPlayerPiece = GameManagerOffline.gm.greenPlayerPiece[position];
            pathPointMoveOn = pathParent.GreenPathPoint; 
            GameManagerOffline.gm.greenOutPlayers += 1;
        }

       outPlayerPiece.isReady = true;
        outPlayerPiece.transform.position = pathPointMoveOn[0].transform.position;
       outPlayerPiece.numberOfStepsAlreadyMove = 1;

        outPlayerPiece.previousPathPoint = pathPointMoveOn[0];
       outPlayerPiece.CurrentPathPoint = pathPointMoveOn[0];
       outPlayerPiece.CurrentPathPoint.AddPlayerPiece(outPlayerPiece);
        GameManagerOffline.gm.AddPathPoint(outPlayerPiece.CurrentPathPoint);

        GameManagerOffline.gm.canDiceRoll = true;
        GameManagerOffline.gm.selfDice = true;
        GameManagerOffline.gm.transferdice = false;
        
        GameManagerOffline.gm.numberOfStepsToMove = 0;

        GameManagerOffline.gm.SelfRoll();

    }

    public IEnumerator MovePlayer(int position)
    {

        if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
        {
            outPlayerPiece = GameManagerOffline.gm.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint;
       
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
        {
            outPlayerPiece = GameManagerOffline.gm.bluePlayerPiece[position];
            pathPointMoveOn = pathParent.BluePathPoint;
           
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[2])
        {
            outPlayerPiece = GameManagerOffline.gm.yelloPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint;
          
        }
        else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[3])
        {
            outPlayerPiece = GameManagerOffline.gm.greenPlayerPiece[position];
            pathPointMoveOn = pathParent.GreenPathPoint;
         
        }


        GameManagerOffline.gm.transferdice = false;
        yield return new WaitForSeconds(0.25f);
      int numberOfStepsToMove = GameManagerOffline.gm.numberOfStepsToMove;
        outPlayerPiece.CurrentPathPoint.RescaleAndRepositioningAllPlayer();
        for (int i = outPlayerPiece.numberOfStepsAlreadyMove; i < (outPlayerPiece.numberOfStepsAlreadyMove + numberOfStepsToMove); i++)
        {
           
            if (isPathPointsAvailableToMove(numberOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMove, pathPointMoveOn))
            {
                outPlayerPiece.transform.position = pathPointMoveOn[i].transform.position;
                outPlayerPiece.GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(0.35f);
            }
        }
        if (isPathPointsAvailableToMove(numberOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMove, pathPointMoveOn))
        {

            outPlayerPiece.numberOfStepsAlreadyMove += numberOfStepsToMove;


            GameManagerOffline.gm.RemovePathPoint(outPlayerPiece.previousPathPoint);
            outPlayerPiece.previousPathPoint.RemovePlayerPiece(outPlayerPiece);
            outPlayerPiece.CurrentPathPoint = pathPointMoveOn[outPlayerPiece.numberOfStepsAlreadyMove - 1];

            if (outPlayerPiece.CurrentPathPoint.AddPlayerPiece(outPlayerPiece))
            {
                if (outPlayerPiece.numberOfStepsAlreadyMove == 57)
                {
                    GameManagerOffline.gm.selfDice = true;

                }
                else
                {
                    if (GameManagerOffline.gm.numberOfStepsToMove != 6)
                    {

                        /* GameManagerOffline.gm.selfDice = false;*/
                        GameManagerOffline.gm.transferdice = true;
                        Debug.Log("Do daat");
                        Debug.Log(GameManagerOffline.gm.redOutPlayers + "   " + GameManagerOffline.gm.yellowOutPlayers);
                    }
                    else
                    {
                        GameManagerOffline.gm.selfDice = true;
                        /*   GameManagerOffline.gm.transferdice = false;*/
                    }
                }
            }
            else
            {
                GameManagerOffline.gm.selfDice = true;
            }


            GameManagerOffline.gm.AddPathPoint(outPlayerPiece.CurrentPathPoint);
            outPlayerPiece.previousPathPoint = outPlayerPiece.CurrentPathPoint;


            GameManagerOffline.gm.numberOfStepsToMove = 0;

        }

        GameManagerOffline.gm.canPlayerMove = true;

        GameManagerOffline.gm.RollingDiceManager();

        if (MovePlayerPiece != null)
        {
            StopCoroutine("MovePlayer");
        }
    }

}
