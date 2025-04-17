using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OfflineWinning : MonoBehaviour
{
    public GameObject WinningScreen;

    public GameObject RedWinner;
    public GameObject BlueWinner;
    public GameObject GreenWinner;
    public GameObject YellowWinner;


    public GameObject WinnerList;

    byte position = 1;

    byte RedPosition=0;
    byte GreenPosition=0;
    byte YellowPosition=0;
    byte BluePosition=0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManagerOffline.gm.ManageRollingDice[1].isAllowed && GameManagerOffline.gm.blueCompletePlayers==4)
        {
          GameObject WinningTag =  GameManagerOffline.gm.ManageRollingDice[1].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            BluePosition = position;

            GameObject op= Instantiate(BlueWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManagerOffline.gm.BluePlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text=position.ToString();
            position++;
            GameManagerOffline.gm.ManageRollingDice[1].isAllowed=false;
            GameManagerOffline.gm.PlayerRemainingToPlay--;

        }else if (GameManagerOffline.gm.ManageRollingDice[0].isAllowed && GameManagerOffline.gm.redCompletePlayers == 4)
        {
            GameObject WinningTag = GameManagerOffline.gm.ManageRollingDice[0].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(RedWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManagerOffline.gm.RedPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            RedPosition = position;
            position++;
            GameManagerOffline.gm.ManageRollingDice[0].isAllowed = false;
            GameManagerOffline.gm.PlayerRemainingToPlay--;
        }
        else if(GameManagerOffline.gm.ManageRollingDice[2].isAllowed && GameManagerOffline.gm.yellowCompletePlayers == 4)
        {
            GameObject WinningTag = GameManagerOffline.gm.ManageRollingDice[2].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(YellowWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManagerOffline.gm.YellowPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            YellowPosition = position;
            position++;
            GameManagerOffline.gm.ManageRollingDice[2].isAllowed = false;
            GameManagerOffline.gm.PlayerRemainingToPlay--;
        }
        else if (GameManagerOffline.gm.ManageRollingDice[3].isAllowed && GameManagerOffline.gm.greenCompletePlayers == 4)
        {
            GameObject WinningTag = GameManagerOffline.gm.ManageRollingDice[3].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(GreenWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManagerOffline.gm.GreenPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            GreenPosition = position;
            position++;
            GameManagerOffline.gm.ManageRollingDice[3].isAllowed = false;
            GameManagerOffline.gm.PlayerRemainingToPlay--;
        }
        if(GameManagerOffline.gm.PlayerRemainingToPlay==1)
        {
            foreach (var k in GameManagerOffline.gm.ManageRollingDice)
            {
                if (k.isAllowed)
                {
                    if (k.name.Contains("Red"))
                    {
                        GameManagerOffline.gm.redCompletePlayers = 4;
                    }
                    if (k.name.Contains("Blue"))
                    {
                        GameManagerOffline.gm.blueCompletePlayers = 4;
                    }
                    if (k.name.Contains("Green"))
                    {
                        GameManagerOffline.gm.greenCompletePlayers = 4;
                    }
                    if (k.name.Contains("Yellow"))
                    {
                        GameManagerOffline.gm.yellowCompletePlayers = 4;
                    }

                }
            }
                
            WinningScreen.gameObject.SetActive(true);
        }
    }


    public void ReturnToHomeScreen()
    {
        SceneManager.LoadScene("HomeScreen");
    }
}
