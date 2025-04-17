using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineUIManagerThree : MonoBehaviour
{
   

    public List<TMP_InputField> Players;


    public string RedName="PLON";
    public string BlueName="PLON";
    public string GreenName= "PLON";
    public string YellowName= "PLON";

    public ParentThreeUIManager uIManager;
    
    public static OfflineUIManagerThree instance;


    
    public void loadNextScene()
    {

        for(int i = 0; i < 3; i++)
        {
           if(uIManager.Player[i].selectedIndex==0)
            {
                BlueName = Players[i].text;
            }
           else if(uIManager.Player[i].selectedIndex == 1)
            {
                RedName = Players[i].text;
            }
            else if (uIManager.Player[i].selectedIndex == 2)
            {
                GreenName = Players[i].text;
            }
            else if (uIManager.Player[i].selectedIndex == 3)
            {
                YellowName = Players[i].text;
            }
        }
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("LudoGame");
    }
}
