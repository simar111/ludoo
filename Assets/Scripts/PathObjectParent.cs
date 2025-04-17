using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PathObjectParent : MonoBehaviour
{
    public PathPoint[] CommonPathPoint;
    public PathPoint[] RedPathPoint;
    public PathPoint[] BluePathPoint;
    public PathPoint[] GreenPathPoint;
    public PathPoint[] YellowPathPoint;
    public PathPoint[] BasePathPoint;

    [Header("Scale and Position")]
    public float[] scales;
    public float[] positionDifference;




    /*  private void Update()
      {
          tM.text = GameManager.gm.redOutPlayers + "   " + GameManager.gm.yellowOutPlayers;
      }*/

    /*    private void Update()
        {

                for (int i = 0; i < 4; i++)
                {
                if (BasePathPoint[i].PlayerPieceList.Count==1 && BasePathPoint[i].PlayerPieceList[0].Contains("Red"))
                {
                    GameManager.gm.redOutPlayers -= 1;
                }
                }
            for (int i = 4; i < 8; i++)
            {
                if (BasePathPoint[i].PlayerPieceList.Count == 1 && BasePathPoint[i].PlayerPieceList[0].Contains("Blue"))
                {
                    GameManager.gm.blueOutPlayers -= 1;
                }
            }
            for (int i = 8; i < 12; i++)
            {
                if (BasePathPoint[i].PlayerPieceList.Count == 1 && BasePathPoint[i].PlayerPieceList[0].Contains("Yellow"))
                {
                    GameManager.gm.yellowOutPlayers -= 1;
                }
            }
            for (int i = 12; i < 16; i++)
            {
                if (BasePathPoint[i].PlayerPieceList.Count == 1 && BasePathPoint[i].PlayerPieceList[0].Contains("Green"))
                {
                    GameManager.gm.greenOutPlayers -= 1;
                }
            }

        }*/
}
