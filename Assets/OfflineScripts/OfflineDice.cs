using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OfflineDice : MonoBehaviour
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
    public bool isAIPlayer = false; // New field to identify AI player

    private void Awake()
    {
        pathParent = FindObjectOfType<OfflinePathObjectParent>();
    }

    private void OnMouseDown()
    {
        // Only allow human player to click
        if (!isAIPlayer)
        {
            generateRandomNumber = StartCoroutine(RollDice());
        }
    }

    public void mouseRoll()
    {
        generateRandomNumber = StartCoroutine(RollDice());
    }

    IEnumerator RollDice()
    {
        OfflineManager.om.transferdice = false;
        yield return new WaitForEndOfFrame();
        if (GameManagerOffline.gm.canDiceRoll)
        {
            this.GetComponent<AudioSource>().Play();
            OfflineManager.om.canDiceRoll = false;
            numberSpriteHolder.gameObject.SetActive(false);
            diceAnim.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;

            GameManagerOffline.gm.numberOfStepsToMove = numberGot;
            OfflineManager.om.dice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            diceAnim.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            int nummberGot = GameManagerOffline.gm.numberOfStepsToMove;

            // Modified for 2 players (Red for user, Yellow for AI)
            if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[0])
            {
                outPieces = OfflineManager.om.redOutPlayers;
            }
            else if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[2])
            {
                outPieces = OfflineManager.om.yellowOutPlayers;
            }

            if (PlayerCanNotMove())
            {
                yield return new WaitForSeconds(0.3f);
                if (nummberGot != 6)
                {
                    OfflineManager.om.transferdice = true;
                }
                else
                    OfflineManager.om.selfDice = true;
            }
            else
            {
                if (outPieces == 0 && nummberGot != 6)
                {
                    yield return new WaitForSeconds(0.3f);
                    OfflineManager.om.transferdice = true;
                }
                else
                {
                    if (numberGot == 6)
                    {
                        showSpinners();
                    }
                    else
                    {
                        showOnlyReadySpinners();
                    }

                    // AI Logic for Yellow player
                    if (isAIPlayer && OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[2])
                    {
                        yield return new WaitForSeconds(1f); // Add delay for AI thinking
                        if (numberGot == 6 && outPieces < 4)
                        {
                            MakePlayerReadyToMove(outPlayerToMove());
                        }
                        else if (outPieces > 0)
                        {
                            int playerPiecePosition = CheckoutPlayer();
                            if (playerPiecePosition >= 0)
                            {
                                OfflineManager.om.canPlayerMove = false;
                                hideSpinners();
                                MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.3f);
                                if (numberGot != 6)
                                {
                                    OfflineManager.om.transferdice = true;
                                }
                                else
                                {
                                    OfflineManager.om.selfDice = true;
                                }
                            }
                        }
                    }
                    // Human player logic
                    else if (!isAIPlayer)
                    {
                        if (outPieces == 0 && nummberGot == 6)
                        {
                            // Wait for player input
                        }
                        else if (outPieces == 1 && nummberGot != 6 && OfflineManager.om.canPlayerMove)
                        {
                            int playerPiecePosition = CheckoutPlayer();
                            if (playerPiecePosition >= 0)
                            {
                                OfflineManager.om.canPlayerMove = false;
                                hideSpinners();
                                MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.3f);
                                if (nummberGot != 6)
                                {
                                    OfflineManager.om.transferdice = true;
                                }
                                else
                                {
                                    OfflineManager.om.selfDice = true;
                                }
                            }
                        }
                    }
                }
            }

            OfflineManager.om.RollingDiceManager();

            if (generateRandomNumber != null)
            {
                StopCoroutine(RollDice());
            }
        }
    }

    void hideSpinners()
    {
        // Modified for 2 players
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
    }

    void showOnlyReadySpinners()
    {
        // Modified for 2 players
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
    }

    void showSpinners()
    {
        // Modified for 2 players
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
    }

    int CheckoutPlayer()
    {
        // Modified for 2 players
        if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[0])
        {
            currentPlayerPieces = OfflineManager.om.redPlayerPiece;
            pathPointMoveOn = pathParent.RedPathPoint;
        }
        else if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[2])
        {
            currentPlayerPieces = OfflineManager.om.yellowPlayerPiece;
            pathPointMoveOn = pathParent.YellowPathPoint;
        }

        for (int i = 0; i < currentPlayerPieces.Length; i++)
        {
            if (currentPlayerPieces[i].isReady && isPathPointsAvailableToMove(OfflineManager.om.numberOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
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
            bool canNotMove = false;
            // Modified for 2 players
            if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[0])
            {
                currentPlayerPieces = OfflineManager.om.redPlayerPiece;
                pathPointMoveOn = pathParent.RedPathPoint;
            }
            else if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[2])
            {
                currentPlayerPieces = OfflineManager.om.yellowPlayerPiece;
                pathPointMoveOn = pathParent.YellowPathPoint;
            }

            for (int i = 0; i < currentPlayerPieces.Length; i++)
            {
                if (currentPlayerPieces[i].isReady)
                {
                    if (isPathPointsAvailableToMove(OfflineManager.om.numberOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
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
        // Modified for 2 players
        if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[0])
        {
            outPlayerPiece = OfflineManager.om.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint;
            OfflineManager.om.redOutPlayers += 1;
        }
        else if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[2])
        {
            outPlayerPiece = OfflineManager.om.yellowPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint;
            OfflineManager.om.yellowOutPlayers += 1;
        }

        outPlayerPiece.isReady = true;
        outPlayerPiece.transform.position = pathPointMoveOn[0].transform.position;
        outPlayerPiece.numberOfStepsAlreadyMove = 1;

        outPlayerPiece.previousPathPoint = pathPointMoveOn[0];
        outPlayerPiece.CurrentPathPoint = pathPointMoveOn[0];
        outPlayerPiece.CurrentPathPoint.AddPlayerPiece(outPlayerPiece);
        OfflineManager.om.AddPathPoint(outPlayerPiece.CurrentPathPoint);

        OfflineManager.om.canDiceRoll = true;
        OfflineManager.om.selfDice = true;
        OfflineManager.om.transferdice = false;

        OfflineManager.om.numberOfStepsToMove = 0;

        OfflineManager.om.SelfRoll();
    }

    public IEnumerator MovePlayer(int position)
    {
        // Modified for 2 players
        if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[0])
        {
            outPlayerPiece = OfflineManager.om.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint;
        }
        else if (OfflineManager.om.dice == OfflineManager.om.ManageRollingDice[2])
        {
            outPlayerPiece = OfflineManager.om.yellowPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint;
        }

        OfflineManager.om.transferdice = false;
        yield return new WaitForSeconds(0.25f);
        int numberOfStepsToMove = OfflineManager.om.numberOfStepsToMove;
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

            OfflineManager.om.RemovePathPoint(outPlayerPiece.previousPathPoint);
            outPlayerPiece.previousPathPoint.RemovePlayerPiece(outPlayerPiece);
            outPlayerPiece.CurrentPathPoint = pathPointMoveOn[outPlayerPiece.numberOfStepsAlreadyMove - 1];

            if (outPlayerPiece.CurrentPathPoint.AddPlayerPiece(outPlayerPiece))
            {
                if (outPlayerPiece.numberOfStepsAlreadyMove == 57)
                {
                    OfflineManager.om.selfDice = true;
                }
                else
                {
                    if (OfflineManager.om.numberOfStepsToMove != 6)
                    {
                        OfflineManager.om.transferdice = true;
                    }
                    else
                    {
                        OfflineManager.om.selfDice = true;
                    }
                }
            }
            else
            {
                OfflineManager.om.selfDice = true;
            }

            OfflineManager.om.AddPathPoint(outPlayerPiece.CurrentPathPoint);
            outPlayerPiece.previousPathPoint = outPlayerPiece.CurrentPathPoint;

            OfflineManager.om.numberOfStepsToMove = 0;
        }

        OfflineManager.om.canPlayerMove = true;

        OfflineManager.om.RollingDiceManager();

        if (MovePlayerPiece != null)
        {
            StopCoroutine("MovePlayer");
        }
    }

    // Helper method for AI to choose which piece to move out
    int outPlayerToMove()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!OfflineManager.om.yellowPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return 0;
    }
}