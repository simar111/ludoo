using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentThreeUIManager : MonoBehaviour
{
    public List<ThreePlayerUIManager> Player;

    public void fixPosition(int setNumber)
    {
        if(setNumber==0)
        {
            if (Player[1].selectedIndex == Player[0].selectedIndex)
            {
               
                while(Player[1].selectedIndex == Player[0].selectedIndex || Player[1].selectedIndex == Player[2].selectedIndex)
                {
                    Player[1].selectedIndex++;
                    if (Player[1].selectedIndex == 4)
                        Player[1].selectedIndex = 0;
                }
                if(Player[1].selectedIndex != Player[0].selectedIndex &&  Player[1].selectedIndex != Player[2].selectedIndex)
                {
                    Player[1].changeBorder();
                }
            }
            if (Player[2].selectedIndex == Player[0].selectedIndex)
            {

                while (Player[2].selectedIndex == Player[0].selectedIndex || Player[1].selectedIndex == Player[2].selectedIndex)
                {
                    Player[2].selectedIndex++;
                    if (Player[2].selectedIndex == 4)
                        Player[2].selectedIndex = 0;
                }
                if (Player[2].selectedIndex != Player[0].selectedIndex && Player[1].selectedIndex != Player[2].selectedIndex)
                {
                    Player[2].changeBorder();
                }
            }
        }
        else if (setNumber==1)
        {

            if (Player[2].selectedIndex == Player[1].selectedIndex)
            {
                while (Player[2].selectedIndex == Player[1].selectedIndex || Player[2].selectedIndex == Player[0].selectedIndex)
                {
                    Player[2].selectedIndex++;
                    if (Player[2].selectedIndex == 4)
                        Player[2].selectedIndex = 0;
                }
                if (Player[2].selectedIndex != Player[1].selectedIndex && Player[2].selectedIndex != Player[0].selectedIndex)
                {
                    Player[2].changeBorder();
                }
            }
            if (Player[0].selectedIndex == Player[1].selectedIndex)
            {
                while (Player[0].selectedIndex == Player[1].selectedIndex || Player[2].selectedIndex == Player[0].selectedIndex)
                {
                    Player[0].selectedIndex++;
                    if (Player[0].selectedIndex == 4)
                        Player[0].selectedIndex = 0;
                }
                if (Player[0].selectedIndex != Player[1].selectedIndex && Player[2].selectedIndex != Player[0].selectedIndex)
                {
                    Player[0].changeBorder();
                }
            }
        }
        else if (setNumber == 2)
        {

            if (Player[0].selectedIndex == Player[2].selectedIndex)
            {
                while (Player[0].selectedIndex == Player[2].selectedIndex || Player[0].selectedIndex == Player[1].selectedIndex)
                {
                    Player[0].selectedIndex++;
                    if (Player[0].selectedIndex == 4)
                        Player[0].selectedIndex = 0;
                }
                if (Player[0].selectedIndex != Player[2].selectedIndex && Player[0].selectedIndex != Player[1].selectedIndex)
                {
                    Player[0].changeBorder();
                }
            }
            if (Player[1].selectedIndex == Player[2].selectedIndex)
            {
                while (Player[1].selectedIndex == Player[2].selectedIndex || Player[0].selectedIndex == Player[1].selectedIndex)
                {
                    Player[1].selectedIndex++;
                    if (Player[1].selectedIndex == 4)
                        Player[1].selectedIndex = 0;
                }
                if (Player[1].selectedIndex != Player[2].selectedIndex && Player[0].selectedIndex != Player[1].selectedIndex)
                {
                    Player[1].changeBorder();
                }
            }
        }
    }
}
