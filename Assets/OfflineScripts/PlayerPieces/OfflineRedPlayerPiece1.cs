using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineRedPlayerPiece1 : OfflinePlayerPiece1
{
    OfflineRollingDice1 redRollingDice;

    private void Start()
    {
        redRollingDice = GetComponentInParent<OfflineRedHome1>().OfflineRollingDice;
    }

    private void OnMouseDown()
    {
        if (OfflineManager2.om.dice != null)
        {
            if (!isReady)
            {
                if (OfflineManager2.om.dice == redRollingDice && OfflineManager2.om.numberOfStepsToMove == 6)
                {
                    OfflineManager2.om.redOutPlayers += 1;
                    // Start animation coroutine and wait for completion
                    StartCoroutine(AnimatePieceOutAndSetReady());
                    Debug.Log("hello");
                    this.transform.GetChild(1).gameObject.SetActive(false);
                    hideSpinners();
                    return;
                }
            }

            // Ensure the piece moves properly when it is ready
            if (OfflineManager2.om.dice == redRollingDice && isReady && OfflineManager2.om.canPlayerMove)
            {
                OfflineManager2.om.canPlayerMove = false;
                MoveSteps(pathParent.RedPathPoint);
                hideSpinners();
            }
        }
    }

    private IEnumerator AnimatePieceOutAndSetReady()
    {
        isReady = false; // Prevent multiple clicks
        // Move the piece out of the house smoothly
        yield return StartCoroutine(MovePieceOutOfHouse(transform, pathParent.RedPathPoint[0].transform.position));
        // Ensure the piece is correctly placed at the first path point
        transform.position = pathParent.RedPathPoint[0].transform.position;
        // Mark the piece as ready
        isReady = true;
        // Update path positions
        numberOfStepsAlreadyMove = 1;
        previousPathPoint = pathParent.RedPathPoint[0];
        CurrentPathPoint = pathParent.RedPathPoint[0];
        CurrentPathPoint.AddPlayerPiece(this);
        OfflineManager2.om.AddPathPoint(CurrentPathPoint);
        // Allow dice rolling again
        OfflineManager2.om.canDiceRoll = true;
        OfflineManager2.om.selfDice = true;
        OfflineManager2.om.transferdice = false;
        Debug.Log(CurrentPathPoint.name);
        // Reset move count
        //OfflineManager2.om.numberOfStepsToMove = 0;

        // Ensure the player can move when rolling a valid dice number
        //OfflineManager2.om.canPlayerMove = true;

        // Allow rolling again
        //OfflineManager2.om.SelfRoll();
    }

    private IEnumerator MovePieceOutOfHouse(Transform piece, Vector3 targetPos)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPos = piece.position;

        while (elapsedTime < duration)
        {
            piece.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        piece.position = targetPos; // Ensure exact position
    }

    void hideSpinners()
    {
        foreach (var op in this.GetComponentInParent<OfflineRedHome1>().playerPieces)
        {
            Debug.LogWarning(op.transform.GetChild(1).gameObject.name);
            op.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
