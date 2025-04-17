using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OfflineWinning1 : MonoBehaviour
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
        //if(OfflineManager.om.ManageRollingDice[1].isAllowed && GameManagerOffline.gm.blueCompletePlayers==4)
        //{
        //  GameObject WinningTag =  GameManagerOffline.gm.ManageRollingDice[1].transform.parent.GetChild(3).gameObject;
        //    WinningTag.SetActive(true);
        //    BluePosition = position;

        //    GameObject op= Instantiate(BlueWinner, WinnerList.transform);
        //    op.GetComponentInChildren<TMP_Text>().text = position.ToString();
        //    op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManagerOffline.gm.BluePlayerName.text;
        //    WinningTag.GetComponentInChildren<TMP_Text>().text=position.ToString();
        //    position++;
        //    GameManagerOffline.gm.ManageRollingDice[1].isAllowed=false;
        //    GameManagerOffline.gm.PlayerRemainingToPlay--;

        //}else
        if (OfflineManager2.om.ManageRollingDice[0].isAllowed && OfflineManager2.om.redCompletePlayers == 4)
        {
            GameObject WinningTag = OfflineManager2.om.ManageRollingDice[0].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(RedWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = OfflineManager2.om.RedPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            RedPosition = position;
            position++;
            OfflineManager2.om.ManageRollingDice[0].isAllowed = false;
            OfflineManager2.om.PlayerRemainingToPlay--;
        }
        else if (OfflineManager2.om.ManageRollingDice[1].isAllowed && OfflineManager2.om.yellowCompletePlayers == 4)
        {
            GameObject WinningTag = OfflineManager2.om.ManageRollingDice[2].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(YellowWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = OfflineManager2.om.YellowPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            YellowPosition = position;
            position++;
            OfflineManager2.om.ManageRollingDice[2].isAllowed = false;
            OfflineManager2.om.PlayerRemainingToPlay--;
        }
        //else if (GameManagerOffline.gm.ManageRollingDice[3].isAllowed && GameManagerOffline.gm.greenCompletePlayers == 4)
        //{
        //    GameObject WinningTag = GameManagerOffline.gm.ManageRollingDice[3].transform.parent.GetChild(3).gameObject;
        //    WinningTag.SetActive(true);
        //    GameObject op = Instantiate(GreenWinner, WinnerList.transform);
        //    op.GetComponentInChildren<TMP_Text>().text = position.ToString();
        //    op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManagerOffline.gm.GreenPlayerName.text;
        //    WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
        //    GreenPosition = position;
        //    position++;
        //    GameManagerOffline.gm.ManageRollingDice[3].isAllowed = false;
        //    GameManagerOffline.gm.PlayerRemainingToPlay--;
        //}
        if (OfflineManager2.om.PlayerRemainingToPlay==1)
        {
            foreach (var k in OfflineManager2.om.ManageRollingDice)
            {
                if (k.isAllowed)
                {
                    if (k.name.Contains("Red"))
                    {
                        OfflineManager2.om.redCompletePlayers = 4;
                    }
                    //if (k.name.Contains("Blue"))
                    //{
                    //    OfflineManager2.om.blueCompletePlayers = 4;
                    //}
                    //if (k.name.Contains("Green"))
                    //{
                    //    OfflineManager2.om.greenCompletePlayers = 4;
                    //}
                    if (k.name.Contains("Yellow"))
                    {
                        OfflineManager2.om.yellowCompletePlayers = 4;
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
