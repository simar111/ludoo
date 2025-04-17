using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RollingDice : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer diceAnim;
    [SerializeField] int numberGot;

    Coroutine generateRandomNumber;
    public int outPieces;

    PathObjectParent pathParent;
    PlayerPiece[] currentPlayerPieces;
    PathPoint[] pathPointMoveOn;

    Coroutine MovePlayerPiece;

    PlayerPiece outPlayerPiece;

    public bool isAllowed = true;



    private void Awake()
    {
        pathParent=FindObjectOfType<PathObjectParent>();
        photonView.ObservedComponents.Add(this);

    } 
    private void OnMouseDown()
    {

      
        /*  if (photonView.Controller.UserId == PhotonNetwork.LocalPlayer.UserId)*/
        if (photonView.IsMine)
        {
          
            generateRandomNumber = StartCoroutine(RollDice());
            GameManager.gm.photonView.RPC("RestartDiceTimer", RpcTarget.All);
        }



    }


    [PunRPC]
    void faltu()
    {
        generateRandomNumber = StartCoroutine(RollDice());
    }

    public void mouseRoll()
    {
        generateRandomNumber = StartCoroutine(RollDice());
    }

    [PunRPC]
    void hideMe(int a)
    {
        if(a==1)
        numberSpriteHolder.gameObject.SetActive(false);
        else
            diceAnim.gameObject.SetActive(false);
    }

    [PunRPC]
    void showMe(int a)
    {
        if (a == 1)
            numberSpriteHolder.gameObject.SetActive(true);
        else
            diceAnim.gameObject.SetActive(true);
    }


    [PunRPC]
    void NumberChange(int number)
    {
        numberSpriteHolder.sprite = numberSprites[number];
    }

    IEnumerator RollDice()
    {
        GameManager.gm.transferdice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.gm.canDiceRoll)
        {
            GameManager.gm.canDiceRoll = false;
            photonView.RPC("DiceSound", RpcTarget.All);


            photonView.RPC("hideMe", RpcTarget.AllBuffered,1);

                photonView.RPC("showMe", RpcTarget.AllBuffered,2);
            yield return new WaitForSeconds(0.7f);
            numberGot = Random.Range(0, 6);

                photonView.RPC("NumberChange", RpcTarget.AllBuffered, numberGot);
            numberGot += 1;

            GameManager.gm.numberOfStepsToMove = numberGot;
            GameManager.gm.dice = this;

                photonView.RPC("hideMe", RpcTarget.AllBuffered,2);

                photonView.RPC("showMe", RpcTarget.AllBuffered,1);
            yield return new WaitForEndOfFrame();

            int nummberGot = GameManager.gm.numberOfStepsToMove;




            if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
            {
                outPieces = GameManager.gm.redOutPlayers;
            }
            else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[1])
            {
                outPieces = GameManager.gm.blueOutPlayers;
            }
            else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
            {
                outPieces = GameManager.gm.yellowOutPlayers;
            }
            else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[3])
            {
                outPieces = GameManager.gm.greenOutPlayers;
            }





            if (PlayerCanNotMove())
            {
                yield return new WaitForSeconds(0.3f);
                if (nummberGot != 6)
                {
                    GameManager.gm.transferdice = true;
                    Debug.Log("Do pahr");
                }
                else
                    GameManager.gm.selfDice = true;
            }
            else
            {



                if (outPieces == 0 && nummberGot != 6)
                {
                    yield return new WaitForSeconds(0.3f);
                    GameManager.gm.transferdice = true;
                    Debug.Log("Do hath");
                }
                else
                {
                 /*   GameManager.gm.photonView.RPC("PlayTimer",RpcTarget.All);*/
                    if (numberGot == 6)
                    {
                        photonView.RPC("showSpinners", RpcTarget.All);
                    }
                    else
                    {
                        photonView.RPC("showOnlyReadySpinners", RpcTarget.All);
                        
                    }
                    if (outPieces == 0 && nummberGot==6)
                    {
                        /*MakePlayerReadyToMove(0);*/
                    }
                    else if (outPieces == 1 && nummberGot != 6 && GameManager.gm.canPlayerMove)
                    {
                        int playerPiecePosition = CheckoutPlayer();
                        if (playerPiecePosition >= 0)
                        {
                            GameManager.gm.canPlayerMove = false;
                            /*     if (photonView.IsMine)*/
                            photonView.RPC("hideSpinners", RpcTarget.All);
                            MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.3f);
                            if (nummberGot != 6)
                            {
                                GameManager.gm.transferdice = true;
                                Debug.Log("Do kan");
                            }
                            else
                            {
                                GameManager.gm.selfDice = true;
                            }
                        }

                    }
                    else if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
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
                                GameManager.gm.canPlayerMove = false;
                          /*      if(photonView.IsMine)*/
                                    MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));

                            }
                            else
                            {
                                yield return new WaitForSeconds(0.3f);
                                if (numberGot != 6)
                                {
                                    GameManager.gm.transferdice = true;
                                    Debug.Log("Do naak");
                                }
                                else
                                {
                                    GameManager.gm.selfDice = true;
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
                                GameManager.gm.transferdice = true;
                                Debug.Log("Do baal");
                            }
                            else
                            {
                                GameManager.gm.selfDice = true;
                            }
                        }
                    }
                }
            }


            /*  if (GameManager.gm.numberOfStepsToMove != 6 && outPieces == 0)
              {
                  GameManager.gm.canDiceRoll = true;
                  GameManager.gm.selfDice = false;
                  GameManager.gm.transferdice = true;
  *//*
                  yield return new WaitForSeconds(0.3f);*//*

              }*/

            GameManager.gm.RollingDiceManager();



            if (generateRandomNumber != null)
            {
                StopCoroutine(RollDice());
            }
        }

  
    }

    int outPlayerToMove()
    {
        for(int i = 0; i < 4; i++)
        {
            if (!GameManager.gm.yelloPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return 0;
    }

    int CheckoutPlayer()
    {
        if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
        {
            currentPlayerPieces = GameManager.gm.redPlayerPiece;
            pathPointMoveOn = pathParent.RedPathPoint;
           
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[1])
        {
            currentPlayerPieces = GameManager.gm.bluePlayerPiece;
            pathPointMoveOn = pathParent.BluePathPoint;
           
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
        {
            currentPlayerPieces = GameManager.gm.yelloPlayerPiece;
            pathPointMoveOn = pathParent.YellowPathPoint;
         
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[3])
        {
            currentPlayerPieces = GameManager.gm.greenPlayerPiece;
            pathPointMoveOn = pathParent.GreenPathPoint;
          
        }

        for(int i = 0;i< currentPlayerPieces.Length; i++)
        {
            if (currentPlayerPieces[i].isReady && isPathPointsAvailableToMove(GameManager.gm.numberOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
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
            if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
            {
                currentPlayerPieces = GameManager.gm.redPlayerPiece;
                pathPointMoveOn = pathParent.RedPathPoint;
            }
            else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[1])
            {
                currentPlayerPieces = GameManager.gm.bluePlayerPiece;
                pathPointMoveOn = pathParent.BluePathPoint;
            }
            else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
            {
                currentPlayerPieces = GameManager.gm.yelloPlayerPiece;
                pathPointMoveOn = pathParent.YellowPathPoint;
            }
            else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[3])
            {
                currentPlayerPieces = GameManager.gm.greenPlayerPiece;
                pathPointMoveOn = pathParent.GreenPathPoint;
            }
            for(int i = 0;i< currentPlayerPieces.Length; i++)
            {
                if (currentPlayerPieces[i].isReady)
                {
                    if (isPathPointsAvailableToMove(GameManager.gm.numberOfStepsToMove,currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
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
                return true;
            }
        }
        return false;
    }

    bool isPathPointsAvailableToMove(int numOfSteps, int numOfStepsAlredayMove, PathPoint[] pathPointToMove)
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
        if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
        {
            outPlayerPiece = GameManager.gm.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint; 
            GameManager.gm.redOutPlayers += 1;
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[1])
        {
            outPlayerPiece = GameManager.gm.bluePlayerPiece[position];
            pathPointMoveOn = pathParent.BluePathPoint; 
            GameManager.gm.blueOutPlayers += 1;
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
        {
            outPlayerPiece = GameManager.gm.yelloPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint; 
            GameManager.gm.yellowOutPlayers += 1;
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[3])
        {
            outPlayerPiece = GameManager.gm.greenPlayerPiece[position];
            pathPointMoveOn = pathParent.GreenPathPoint; 
            GameManager.gm.greenOutPlayers += 1;
        }

       outPlayerPiece.isReady = true;
        outPlayerPiece.transform.position = pathPointMoveOn[0].transform.position;
       outPlayerPiece.numberOfStepsAlreadyMove = 1;

        outPlayerPiece.previousPathPoint = pathPointMoveOn[0];
       outPlayerPiece.CurrentPathPoint = pathPointMoveOn[0];
/*        if (outPlayerPiece.CurrentPathPoint.GetComponent<PhotonView>().IsMine)*/
            outPlayerPiece.CurrentPathPoint.GetComponent<PhotonView>().RPC("AddPlayerPiece", RpcTarget.AllBuffered, outPlayerPiece.tag);
  /*      outPlayerPiece.CurrentPathPoint.AddPlayerPiece(outPlayerPiece.tag);*/
        GameManager.gm.AddPathPoint(outPlayerPiece.CurrentPathPoint);

        GameManager.gm.canDiceRoll = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferdice = false;
        
        GameManager.gm.numberOfStepsToMove = 0;

        GameManager.gm.SelfRoll();

    }


    
    void faltuMove(int playerPiecePosition)
    {
        MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));
    }

    [PunRPC]
    public IEnumerator MovePlayer(int position)
    {

        if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
        {
            outPlayerPiece = GameManager.gm.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint;
       
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[1])
        {
            outPlayerPiece = GameManager.gm.bluePlayerPiece[position];
            pathPointMoveOn = pathParent.BluePathPoint;
           
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
        {
            outPlayerPiece = GameManager.gm.yelloPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint;
          
        }
        else if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[3])
        {
            outPlayerPiece = GameManager.gm.greenPlayerPiece[position];
            pathPointMoveOn = pathParent.GreenPathPoint;
         
        }


        GameManager.gm.transferdice = false;
        yield return new WaitForSeconds(0.25f);
      int numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;
        /*    outPlayerPiece.CurrentPathPoint.GetComponent<PhotonView>().RPC("RescaleAndRepositioningAllPlayer",RpcTarget.AllBuffered);*/
        outPlayerPiece.CurrentPathPoint.RescaleAndRepositioningAllPlayer();
        for (int i = outPlayerPiece.numberOfStepsAlreadyMove; i < (outPlayerPiece.numberOfStepsAlreadyMove + numberOfStepsToMove); i++)
        {
           
            if (isPathPointsAvailableToMove(numberOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMove, pathPointMoveOn))
            {
                outPlayerPiece.transform.position = pathPointMoveOn[i].transform.position;
                photonView.RPC("PlayerSound", RpcTarget.All, outPlayerPiece.tag);
                yield return new WaitForSeconds(0.35f);
            }
        }
        if (isPathPointsAvailableToMove(numberOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMove, pathPointMoveOn))
        {

            outPlayerPiece.numberOfStepsAlreadyMove += numberOfStepsToMove;


            GameManager.gm.RemovePathPoint(outPlayerPiece.previousPathPoint);
            outPlayerPiece.previousPathPoint.GetComponent<PhotonView>().RPC("RemovePlayerPiece",RpcTarget.AllBuffered,outPlayerPiece.tag);
            outPlayerPiece.CurrentPathPoint = pathPointMoveOn[outPlayerPiece.numberOfStepsAlreadyMove - 1];
       /*     if(outPlayerPiece.CurrentPathPoint.GetComponent<PhotonView>().IsMine)*/
            outPlayerPiece.CurrentPathPoint.GetComponent<PhotonView>().RPC("AddPlayerPiece", RpcTarget.AllBuffered, outPlayerPiece.tag);
            if (outPlayerPiece.CurrentPathPoint.returnTurn)
            {

                if (outPlayerPiece.numberOfStepsAlreadyMove == 57)
                {
                    GameManager.gm.selfDice = true;

                }
                else
                {
                    if (GameManager.gm.numberOfStepsToMove != 6)
                    {

                        /* GameManager.gm.selfDice = false;*/
                        GameManager.gm.transferdice = true;
                        Debug.Log("Do daat");
                        Debug.Log(GameManager.gm.redOutPlayers + "   " + GameManager.gm.yellowOutPlayers);
                    }
                    else
                    {
                        GameManager.gm.selfDice = true;
                        /*   GameManager.gm.transferdice = false;*/
                    }
                }
            }
            else
            {
                GameManager.gm.selfDice = true;
  
            }


            GameManager.gm.AddPathPoint(outPlayerPiece.CurrentPathPoint);
            outPlayerPiece.previousPathPoint = outPlayerPiece.CurrentPathPoint;


            GameManager.gm.numberOfStepsToMove = 0;

        }

        GameManager.gm.canPlayerMove = true;

        GameManager.gm.RollingDiceManager();

        if (MovePlayerPiece != null)
        {
            StopCoroutine("MovePlayer");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.outPieces);
        }
        else
        {
            // Network player, receive data
            this.outPieces = (int)stream.ReceiveNext();

        }
    }


    [PunRPC]
    void DiceSound()
    {
        this.GetComponent<AudioSource>().Play();
    }


    [PunRPC]
    void PlayerSound(string str)
    {
        GameObject ko=GameObject.FindGameObjectWithTag(str);
        ko.GetComponent<AudioSource>().Play();
    }



    [PunRPC]
    void hideSpinners()
    {
        if (this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<YellowPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<RedPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<BluePlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<GreenPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

    }




    [PunRPC]
    void showSpinners()
    {
        if(this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<YellowPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<RedPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<BluePlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<GreenPlayerPiece>())
            {

                op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

    }


    [PunRPC]
    void showOnlyReadySpinners()
    {
        
        if (this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<YellowPlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<RedPlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<BluePlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<GreenPlayerPiece>())
            {
                if (op.isReady)
                    op.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

    }
}
