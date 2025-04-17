using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class OfflineRollingDice1 : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer diceAnim;
    [SerializeField] int numberGot;
    Coroutine generateRandomNumber;
    public int outPieces;
    OfflinePathObjectParent1 pathParent;
    OfflinePlayerPiece1[] currentPlayerPieces;
    OfflinePathPoint1[] pathPointMoveOn;
    Coroutine MovePlayerPiece;
    OfflinePlayerPiece1 outPlayerPiece;
    public bool isAllowed = true;
    private int consecutiveSixes = 0;
    private bool forceNonSix = false;
    private void Awake()
    {
        pathParent = FindObjectOfType<OfflinePathObjectParent1>();
    }
    private void OnMouseDown()
    {
        generateRandomNumber = StartCoroutine(RollDice());
    }
    public void mouseRoll()
    {
        generateRandomNumber = StartCoroutine(RollDice());
    }
    IEnumerator RollDice()
    {
        OfflineManager2.om.transferdice = false;
        yield return new WaitForEndOfFrame();
        if (OfflineManager2.om.canDiceRoll)
        {
            this.GetComponent<AudioSource>().Play();
            OfflineManager2.om.canDiceRoll = false;
            numberSpriteHolder.gameObject.SetActive(false);
            diceAnim.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;
            OfflineManager2.om.numberOfStepsToMove = numberGot;
            OfflineManager2.om.dice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            diceAnim.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
            int nummberGot = OfflineManager2.om.numberOfStepsToMove;

            if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
            {
                outPieces = OfflineManager2.om.redOutPlayers;
            }
            //else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
            //{
            //    outPieces = GameManagerOffline.gm.blueOutPlayers;
            //}
            else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
            {
                outPieces = OfflineManager2.om.yellowOutPlayers;
            }
            //else if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[3])
            //{
            //    outPieces = GameManagerOffline.gm.greenOutPlayers;
            //}

            if (PlayerCanNotMove())
            {
                yield return new WaitForSeconds(0.3f);
                if (nummberGot != 6)
                {
                    OfflineManager2.om.transferdice = true;
                    Debug.Log("Do pahr");
                }
                else
                    OfflineManager2.om.selfDice = true;
            }
            else
            {
                if (outPieces == 0 && nummberGot != 6)
                {
                    yield return new WaitForSeconds(0.3f);
                    OfflineManager2.om.transferdice = true;
                    Debug.Log("Do hath");
                }
                else
                {
                    if (numberGot == 6 && OfflineManager2.om.dice== OfflineManager2.om.ManageRollingDice[0])
                    {
                        showSpinners();
                    }
                    else
                    if(OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
                    {
                        showOnlyReadySpinners();
                    }
                    if (outPieces == 0 && nummberGot == 6)
                    {
                        if (OfflineManager2.om.totalPlayerCanPlay == 1 && OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
                        {
                             MakePlayerReadyToMove(0);
                        }
                    }
                    else if (outPieces == 1 && nummberGot != 6 && OfflineManager2.om.canPlayerMove )
                    {
                        int playerPiecePosition = CheckoutPlayer();
                        if (playerPiecePosition >= 0)
                        {

                            OfflineManager2.om.canPlayerMove = false;

                            hideSpinners();

                            MovePlayerPiece = StartCoroutine(MovePlayer(playerPiecePosition));
                            Debug.Log(MovePlayerPiece);
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.3f);
                            if (nummberGot != 6)
                            {
                                OfflineManager2.om.transferdice = true;
                                Debug.Log("Do kan");
                            }
                            else
                            {
                                OfflineManager2.om.selfDice = true;
                            }
                        }
                    }
                    else if (OfflineManager2.om.totalPlayerCanPlay == 1 && OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
                    {
                        currentPlayerPieces = OfflineManager2.om.yelloPlayerPiece;
                        pathPointMoveOn = pathParent.YellowPathPoint;
                        if (numberGot == 6 && outPieces < 4)
                        {
                            Debug.LogWarning("AI Priority 1: Opening a new yellow piece");
                            MakePlayerReadyToMove(outPlayerToMove());
                        }
                        else
                        {
                            int bestPieceToMove = -1;
                            int bestPriority = -1;
                            int bestStepsMoved = -1;

                            for (int i = 0; i < currentPlayerPieces.Length; i++)
                            {
                                if (currentPlayerPieces[i].isReady && isPathPointsAvailableToMove(numberGot, currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
                                {
                                    int targetStep = currentPlayerPieces[i].numberOfStepsAlreadyMove + numberGot;
                                    OfflinePathPoint1 targetPoint = pathPointMoveOn[targetStep - 1];
                                    // Priority 2: Capture a red opponent's piece on a non-safe point
                                    if (targetPoint.PlayerPieceList.Count == 1 &&
                                        !targetPoint.name.Contains("PathPoint (47)") &&
                                        !targetPoint.name.Contains("PathPoint (8)") &&
                                        !targetPoint.name.Contains("PathPoint (13)") &&
                                        !targetPoint.name.Contains("PathPoint (21)") &&
                                        !targetPoint.name.Contains("PathPoint (26)") &&
                                        !targetPoint.name.Contains("PathPoint (34)") &&
                                        !targetPoint.name.Contains("PathPoint (39)") &&
                                        !targetPoint.name.Contains("CenterPathPoint") &&
                                        targetPoint.PlayerPieceList[0].name.Contains("Red"))
                                    {
                                        bestPieceToMove = i;
                                        bestPriority = 2;
                                        Debug.Log($"AI Priority 2: Capturing red piece with yellow piece {i} at {targetPoint.name}");
                                        break; // Highest priority after opening, no need to check further
                                    }
                                    // Priority 3: Reach the goal (step 57)
                                    else if (targetStep == 57)
                                    {
                                        if (bestPriority < 3)
                                        {
                                            bestPieceToMove = i;
                                            bestPriority = 3;
                                            Debug.Log($"AI Priority 3: Moving yellow piece {i} to goal");
                                        }
                                    }
                                    // Priority 4: Move the yellow piece closest to the goal
                                    else if (bestPriority < 4 && currentPlayerPieces[i].numberOfStepsAlreadyMove > bestStepsMoved)
                                    {
                                        bestStepsMoved = currentPlayerPieces[i].numberOfStepsAlreadyMove;
                                        bestPieceToMove = i;
                                        bestPriority = 4;
                                        Debug.Log($"AI Priority 4: Moving yellow piece {i} closest to goal");
                                    }
                                }
                            }

                            // Execute the best move
                            if (bestPieceToMove >= 0)
                            {
                                Debug.Log($"AI: Executing move for yellow piece {bestPieceToMove}");
                                OfflineManager2.om.canPlayerMove = false;
                                MovePlayerPiece = StartCoroutine(OfflineManager2.om.yelloPlayerPiece[bestPieceToMove].MovePlayer(pathParent.YellowPathPoint));
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.3f);
                                if (numberGot != 6)
                                {
                                    OfflineManager2.om.transferdice = true;
                                    Debug.Log("AI: No moves for yellow, transferring turn (Do naak)");
                                }
                                else
                                {
                                    OfflineManager2.om.selfDice = true;
                                    Debug.Log("AI: Yellow rolled 6, taking another turn");
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
                                OfflineManager2.om.transferdice = true;
                                Debug.Log("Do baal");
                            }
                            else
                            {
                                OfflineManager2.om.selfDice = true;
                            }
                        }
                    }
                }
            }

            OfflineManager2.om.RollingDiceManager();

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
            foreach (var op in GameObject.FindObjectsOfType<OfflineYellowPlayerPiece1>())
            {
                op.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineRedPlayerPiece1>())
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
        bool isSix = (numberGot == 6);
        int stepsToMove = OfflineManager2.om.numberOfStepsToMove;

        if (this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineYellowPlayerPiece1>())
            {
                int stepsNeeded = 57 - op.numberOfStepsAlreadyMove;
                bool canMove = stepsNeeded >= stepsToMove;
                bool shouldShow = (op.isReady && op.numberOfStepsAlreadyMove < 57 && canMove) ||
                                (isSix && op.numberOfStepsAlreadyMove < 57 && stepsNeeded >= 6);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineRedPlayerPiece1>())
            {
                int stepsNeeded = 57 - op.numberOfStepsAlreadyMove;
                bool canMove = stepsNeeded >= stepsToMove;
                bool shouldShow = (op.isReady && op.numberOfStepsAlreadyMove < 57 && canMove) ||
                                (isSix && op.numberOfStepsAlreadyMove < 57 && stepsNeeded >= 6);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineBluePlayerPiece>())
            {
                bool canMoveToHome = (op.numberOfStepsAlreadyMove + stepsToMove) <= 57;
                bool shouldShow = (op.isReady && op.numberOfStepsAlreadyMove < 57 && canMoveToHome) ||
                                (isSix && op.numberOfStepsAlreadyMove < 57);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineGreenPlayerPiece>())
            {
                bool canMoveToHome = (op.numberOfStepsAlreadyMove + stepsToMove) <= 57;
                bool shouldShow = (op.isReady && op.numberOfStepsAlreadyMove < 57 && canMoveToHome) ||
                                (isSix && op.numberOfStepsAlreadyMove < 57);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
    }
    int outPlayerToMove()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!OfflineManager2.om.yelloPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return 0;
    }

    void showSpinners()
    {
        bool isSix = (numberGot == 6);
        int stepsToMove = OfflineManager2.om.numberOfStepsToMove;

        if (this.name.Contains("Yellow"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineYellowPlayerPiece1>())
            {
                int stepsNeeded = 57 - op.numberOfStepsAlreadyMove;
                bool canMove = stepsNeeded >= stepsToMove;
                bool shouldShow = op.numberOfStepsAlreadyMove < 57 &&
                                (op.isReady || isSix) &&
                                (isSix ? stepsNeeded >= 6 : canMove);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
        if (this.name.Contains("Red"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineRedPlayerPiece1>())
            {
                int stepsNeeded = 57 - op.numberOfStepsAlreadyMove;
                bool canMove = stepsNeeded >= stepsToMove;
                bool shouldShow = op.numberOfStepsAlreadyMove < 57 &&
                                (op.isReady || isSix) &&
                                (isSix ? stepsNeeded >= 6 : canMove);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
        if (this.name.Contains("Blue"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineBluePlayerPiece>())
            {
                bool canMoveToHome = (op.numberOfStepsAlreadyMove + stepsToMove) <= 57;
                bool shouldShow = op.numberOfStepsAlreadyMove < 57 &&
                                (op.isReady || isSix) &&
                                (isSix || canMoveToHome);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
        if (this.name.Contains("Green"))
        {
            foreach (var op in GameObject.FindObjectsOfType<OfflineGreenPlayerPiece>())
            {
                bool canMoveToHome = (op.numberOfStepsAlreadyMove + stepsToMove) <= 57;
                bool shouldShow = op.numberOfStepsAlreadyMove < 57 &&
                                (op.isReady || isSix) &&
                                (isSix || canMoveToHome);
                op.transform.GetChild(1).gameObject.SetActive(shouldShow);
            }
        }
    }

    int CheckoutPlayer()
    {
        if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
        {
            currentPlayerPieces = OfflineManager2.om.redPlayerPiece;
            pathPointMoveOn = pathParent.RedPathPoint;
        }
        else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
        {
            currentPlayerPieces = OfflineManager2.om.yelloPlayerPiece;
            pathPointMoveOn = pathParent.YellowPathPoint;
        }

        for (int i = 0; i < currentPlayerPieces.Length; i++)
        {
            if (currentPlayerPieces[i].isReady && isPathPointsAvailableToMove(OfflineManager2.om.numberOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
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
            if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
            {
                currentPlayerPieces = OfflineManager2.om.redPlayerPiece;
                pathPointMoveOn = pathParent.RedPathPoint;
            }
            else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
            {
                currentPlayerPieces = OfflineManager2.om.yelloPlayerPiece;
                pathPointMoveOn = pathParent.YellowPathPoint;
            }
            for (int i = 0; i < currentPlayerPieces.Length; i++)
            {
                if (currentPlayerPieces[i].isReady)
                {
                    if (isPathPointsAvailableToMove(OfflineManager2.om.numberOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMove, pathPointMoveOn))
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

    bool isPathPointsAvailableToMove(int numOfSteps, int numOfStepsAlredayMove, OfflinePathPoint1[] pathPointToMove)
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
        if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
        {
            outPlayerPiece = OfflineManager2.om.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint;
            OfflineManager2.om.redOutPlayers += 1;
        }
        else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
        {
            outPlayerPiece = OfflineManager2.om.yelloPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint;
            OfflineManager2.om.yellowOutPlayers += 1;
        }

        outPlayerPiece.isReady = true;

        // Start the smooth movement coroutine
        StartCoroutine(MovePieceOutOfHouse(outPlayerPiece, pathPointMoveOn[0].transform.position));
    }

    // Coroutine to animate player piece movement out of the house
    private IEnumerator MovePieceOutOfHouse (OfflinePlayerPiece1 piece, Vector3 targetPos)
    {
        float duration = 0.5f; // Animation duration
        float elapsedTime = 0f;

        Vector3 startPos = piece.transform.position; // Initial position

        // Smoothly move piece from startPos to targetPos
        while (elapsedTime < duration)
        {
            piece.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure exact final position
        piece.transform.position = targetPos;

        // Now set its path properties after movement
        piece.numberOfStepsAlreadyMove = 1;
        piece.previousPathPoint = pathPointMoveOn[0];
        piece.CurrentPathPoint = pathPointMoveOn[0];
        piece.CurrentPathPoint.AddPlayerPiece(piece);
        OfflineManager2.om.AddPathPoint(piece.CurrentPathPoint);

        // Allow dice roll after movement completes
        OfflineManager2.om.canDiceRoll = true;
        //OfflineManager2.om.selfDice = true;
        OfflineManager2.om.transferdice = false;
        OfflineManager2.om.numberOfStepsToMove = 0;

        // Roll the dice after animation completes
        OfflineManager2.om.SelfRoll();
    }


    public IEnumerator MovePlayer(int position)
    {
        if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
        {
            outPlayerPiece = OfflineManager2.om.redPlayerPiece[position];
            pathPointMoveOn = pathParent.RedPathPoint;
        }
        else if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
        {
            outPlayerPiece = OfflineManager2.om.yelloPlayerPiece[position];
            pathPointMoveOn = pathParent.YellowPathPoint;
        }

        OfflineManager2.om.transferdice = false;
        yield return new WaitForSeconds(0.25f);
        int numberOfStepsToMove = OfflineManager2.om.numberOfStepsToMove;
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

            OfflineManager2.om.RemovePathPoint(outPlayerPiece.previousPathPoint);
            outPlayerPiece.previousPathPoint.RemovePlayerPiece(outPlayerPiece);
            outPlayerPiece.CurrentPathPoint = pathPointMoveOn[outPlayerPiece.numberOfStepsAlreadyMove - 1];

            if (outPlayerPiece.CurrentPathPoint.AddPlayerPiece(outPlayerPiece))
            {
                if (outPlayerPiece.numberOfStepsAlreadyMove == 57)
                {
                    OfflineManager2.om.selfDice = true;
                }
                else
                {
                    if (OfflineManager2.om.numberOfStepsToMove != 6)
                    {
                        OfflineManager2.om.transferdice = true;
                        Debug.Log("Do daat");
                        Debug.Log(OfflineManager2.om.redOutPlayers + "   " + OfflineManager2.om.yellowOutPlayers);
                    }
                    else
                    {
                        OfflineManager2.om.selfDice = true;
                    }
                }
            }
            else
            {
                OfflineManager2.om.selfDice = true;
            }

            OfflineManager2.om.AddPathPoint(outPlayerPiece.CurrentPathPoint);
            outPlayerPiece.previousPathPoint = outPlayerPiece.CurrentPathPoint;

            OfflineManager2.om.numberOfStepsToMove = 0;
        }

        OfflineManager2.om.canPlayerMove = true;

        OfflineManager2.om.RollingDiceManager();

        if (MovePlayerPiece != null)
        {
            StopCoroutine("MovePlayer");
        }
    }
}