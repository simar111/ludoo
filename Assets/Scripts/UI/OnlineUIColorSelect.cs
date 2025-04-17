using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineUIColorSelect : MonoBehaviour
{
    public List<GameObject> pieces;
    public int selectedIndex;


    public void changeIndex(int number)
    {
        selectedIndex = number;
        changeBorder();
    }


    public void changeBorder()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (selectedIndex == i)
            {
                pieces[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                pieces[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
