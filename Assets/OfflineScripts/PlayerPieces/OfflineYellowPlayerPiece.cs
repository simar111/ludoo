using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineYellowPlayerPiece : OfflinePlayerPiece
{
    OfflineRollingDice yellowRollingDice;

    private void Start()
    {
        yellowRollingDice = GetComponentInParent<OfflineYellowHome>().OfflineRollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManagerOffline.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManagerOffline.gm.dice == yellowRollingDice && GameManagerOffline.gm.numberOfStepsToMove == 6)
                {
                    GameManagerOffline.gm.yellowOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.YellowPathPoint);
                    GameManagerOffline.gm.numberOfStepsToMove = 0;
                    isReady = true;
                    this.transform.GetChild(1).gameObject.SetActive(false);
                    hideSpinners();
                    return;
                }

            }
            if (GameManagerOffline.gm.dice == yellowRollingDice && isReady && GameManagerOffline.gm.canPlayerMove)
            {
                GameManagerOffline.gm.canPlayerMove = false;
                MoveSteps(pathParent.YellowPathPoint);
                hideSpinners();
            }

        }
      
    }

    void hideSpinners()
    {
        foreach (var op in this.GetComponentInParent<OfflineYellowHome>().playerPieces)
        {
            Debug.LogWarning(op.transform.GetChild(1).gameObject.name);
            op.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
