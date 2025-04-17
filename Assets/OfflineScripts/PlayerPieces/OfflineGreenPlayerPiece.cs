using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineGreenPlayerPiece : OfflinePlayerPiece
{
    OfflineRollingDice greenRollingDice;

    private void Start()
    {
        greenRollingDice = GetComponentInParent<OfflineGreenHome>().OfflineRollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManagerOffline.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManagerOffline.gm.dice == greenRollingDice && GameManagerOffline.gm.numberOfStepsToMove == 6)
                {
                    GameManagerOffline.gm.greenOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.GreenPathPoint);
                    isReady = true;
                    GameManagerOffline.gm.numberOfStepsToMove = 0;

                    hideSpinners();
                    return;
                }

            }
            if (GameManagerOffline.gm.dice == greenRollingDice && isReady && GameManagerOffline.gm.canPlayerMove)
            {
                GameManagerOffline.gm.canPlayerMove = false;

                hideSpinners();
                MoveSteps(pathParent.GreenPathPoint);

            }


        }

       
    }


    void hideSpinners()
    {
        foreach (var op in this.GetComponentInParent<OfflineGreenHome>().playerPieces)
        {
            Debug.LogWarning(op.transform.GetChild(1).gameObject.name);
            op.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
