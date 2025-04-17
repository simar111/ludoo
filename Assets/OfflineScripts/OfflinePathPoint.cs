using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflinePathPoint : MonoBehaviour
{
    OfflinePathPoint[] pathPointToMoveOn_;
    public OfflinePathObjectParent pathObjectParent;
    public List<OfflinePlayerPiece> PlayerPieceList= new List<OfflinePlayerPiece>();

    private void Start()
    {
        pathObjectParent=GetComponentInParent<OfflinePathObjectParent>();
    }

    public bool AddPlayerPiece(OfflinePlayerPiece playerPiece)
    {

        if(this.name== "CenterPathPoint")
        {
            reduceOnePlayer(playerPiece);
        }

        if (this.name != "PathPoint" && this.name != "PathPoint (47)" && this.name != "PathPoint (8)" && this.name != "PathPoint (13)" && this.name != "PathPoint (21)" && this.name != "PathPoint (26)" && this.name != "PathPoint (34)" && this.name != "PathPoint (39)" && this.name!= "CenterPathPoint")
        {
            if (PlayerPieceList.Count == 1)
            {
                string preePlayerPieceName = PlayerPieceList[0].name;
                string curPlayerPiecename = playerPiece.name;
                curPlayerPiecename = curPlayerPiecename.Substring(0, curPlayerPiecename.Length - 4);

                if (!preePlayerPieceName.Contains(curPlayerPiecename))
                {
                    PlayerPieceList[0].isReady = false;
                    StartCoroutine(revertOnStart(PlayerPieceList[0]));


                    PlayerPieceList[0].numberOfStepsToMove = 0;
                    RemovePlayerPiece(PlayerPieceList[0]);
                    PlayerPieceList.Add(playerPiece);
                    
                    return false;
                }
            }
        }
        addPlayer(playerPiece);
        return true;
    }

    void reduceOnePlayer(OfflinePlayerPiece playerPiece)
    {
        if (playerPiece.name.Contains("Blue"))
        {
            GameManagerOffline.gm.blueOutPlayers -= 1;
            GameManagerOffline.gm.blueCompletePlayers++;
         
        }
        else if (playerPiece.name.Contains("Red"))
        {
            GameManagerOffline.gm.redOutPlayers -= 1;
            GameManagerOffline.gm.redCompletePlayers++;

        }
        else if (playerPiece.name.Contains("Yellow"))
        {
            GameManagerOffline.gm.yellowOutPlayers -= 1;
            GameManagerOffline.gm.yellowCompletePlayers++;
        }
        else if (playerPiece.name.Contains("Green"))
        {
            GameManagerOffline.gm.greenOutPlayers -= 1;
            GameManagerOffline.gm.greenCompletePlayers++;

        }
    }

    IEnumerator revertOnStart(OfflinePlayerPiece playerPiece)
    {
        if (playerPiece.name.Contains("Blue"))
        {
            GameManagerOffline.gm.blueOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjectParent.BluePathPoint;
        }
        else if (playerPiece.name.Contains("Red"))
        {
            GameManagerOffline.gm.redOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjectParent.RedPathPoint;
        }
        else if (playerPiece.name.Contains("Yellow"))
        {
            GameManagerOffline.gm.yellowOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjectParent.YellowPathPoint;
        }
        else if (playerPiece.name.Contains("Green"))
        {
            GameManagerOffline.gm.greenOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjectParent.GreenPathPoint;
        }
        this.GetComponentInParent<AudioSource>().Play();
        for (int i = playerPiece.numberOfStepsAlreadyMove-1;i>=0;i--)
        {
            playerPiece.transform.position = pathPointToMoveOn_[i].transform.position;
            yield return new WaitForSeconds(0.03f);
        }
        this.GetComponentInParent<AudioSource>().Stop();
        playerPiece.transform.position = pathObjectParent.BasePathPoint[BasePointPosition(playerPiece.name)].transform.position;
       
    }
    int BasePointPosition(string name)
    {
       
        for(int i=0;i<pathObjectParent.BasePathPoint.Length;i++)
        {
            if (pathObjectParent.BasePathPoint[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    void addPlayer(OfflinePlayerPiece playerPiece)
    {
        PlayerPieceList.Add(playerPiece);
        RescaleAndRepositioningAllPlayer();
    }

    public void RemovePlayerPiece(OfflinePlayerPiece playerPiece)
    {
        if (PlayerPieceList.Contains(playerPiece))
        {
            PlayerPieceList.Remove(playerPiece);
            RescaleAndRepositioningAllPlayer();
        }
    }

/*    void compled(OfflinePlayerPiece playerPiece)
    {
        if (playerPiece.name.Contains("Blue"))
        {
            GameManagerOffline.gm.blueCompletePlayers += 1;
            GameManagerOffline.gm.blueOutPlayers -= 1;

            if (GameManagerOffline.gm.blueCompletePlayers == 4)
                ShowCeleberation();

        }
        else if (playerPiece.name.Contains("Red"))
        {
            GameManagerOffline.gm.redCompletePlayers += 1;
            GameManagerOffline.gm.redOutPlayers -= 1;

            if (GameManagerOffline.gm.redCompletePlayers == 4)
                ShowCeleberation();

        }
        else if (playerPiece.name.Contains("Yellow"))
        {
            GameManagerOffline.gm.yellowCompletePlayers += 1;
            GameManagerOffline.gm.yellowOutPlayers -= 1;

            if (GameManagerOffline.gm.yellowCompletePlayers == 4)
                ShowCeleberation();

        }
        else if (playerPiece.name.Contains("Green"))
        {
            GameManagerOffline.gm.greenCompletePlayers += 1;
            GameManagerOffline.gm.greenOutPlayers -= 1;

            if (GameManagerOffline.gm.greenCompletePlayers == 4)
                ShowCeleberation();

        }
    }*/


    void ShowCeleberation()
    {

    }

    public void RescaleAndRepositioningAllPlayer()
    {
        int plsCount=PlayerPieceList.Count;
        bool isOdd=(plsCount%2)==0?false:true;
        int extent=plsCount/2;
        int counter = 0;
        int spriteLayer = 0;
        if(isOdd)
        {
            for(int i=-extent; i<=extent; i++)
            {
                PlayerPieceList[counter].transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
                PlayerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount-1]),transform.position.y,0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extent; i < extent; i++)
            {
                PlayerPieceList[counter].transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
                PlayerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for(int i = 0; i < PlayerPieceList.Count; i++)
        {
            PlayerPieceList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }
}
