using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineBluePlayerPiece : OfflinePlayerPiece
{

    OfflineRollingDice blueRollingDice;

    private void Start()
    {
        blueRollingDice = GetComponentInParent<OfflineBlueHome>().OfflineRollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManagerOffline.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManagerOffline.gm.dice == blueRollingDice && GameManagerOffline.gm.numberOfStepsToMove == 6)
                {
                    GameManagerOffline.gm.blueOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.BluePathPoint);
                    isReady = true;
                    GameManagerOffline.gm.numberOfStepsToMove = 0;
                    hideSpinners();

                    return;
                }

            }
            if (GameManagerOffline.gm.dice == blueRollingDice && isReady && GameManagerOffline.gm.canPlayerMove)
            {
                GameManagerOffline.gm.canPlayerMove = false;
                hideSpinners();
                MoveSteps(pathParent.BluePathPoint);

            }

           
        }

       
    }

    void hideSpinners()
    {
       foreach(var op in this.GetComponentInParent<OfflineBlueHome>().playerPieces)
        {
            Debug.LogWarning(op.transform.GetChild(1).gameObject.name);
            op.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
