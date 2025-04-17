using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineRedPlayerPiece : OfflinePlayerPiece
{
    OfflineRollingDice redRollingDice;

    private void Start()
    {
        redRollingDice = GetComponentInParent<OfflineRedHome>().OfflineRollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManagerOffline.gm.dice != null)
        {
            if (!isReady)
            {
                if (GameManagerOffline.gm.dice == redRollingDice && GameManagerOffline.gm.numberOfStepsToMove==6)
                {
                    GameManagerOffline.gm.redOutPlayers += 1;
                   MakePlayerReadyToMove(pathParent.RedPathPoint);
                    isReady = true;
                    GameManagerOffline.gm.numberOfStepsToMove=0;
                    this.transform.GetChild(1).gameObject.SetActive(false);
                    hideSpinners();
                    return;
                }
               
            }
            if (GameManagerOffline.gm.dice == redRollingDice && isReady && GameManagerOffline.gm.canPlayerMove){
                GameManagerOffline.gm.canPlayerMove = false;
                MoveSteps(pathParent.RedPathPoint);
                hideSpinners();
            }
          
        }
      
    }

    void hideSpinners()
    {
        foreach (var op in this.GetComponentInParent<OfflineRedHome>().playerPieces)
        {
            Debug.LogWarning(op.transform.GetChild(1).gameObject.name);
            op.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}
