using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Winning : MonoBehaviour
{
    public GameObject WinningScreen;

    public GameObject RedWinner;
    public GameObject BlueWinner;
    public GameObject GreenWinner;
    public GameObject YellowWinner;

    public GameObject WinnerList;

    byte position = 1;

    byte RedPosition = 0;
    byte GreenPosition = 0;
    byte YellowPosition = 0;
    byte BluePosition = 0;

    int totalPrizeMoney = 100; // Example total prize money to be distributed

    void Start()
    {
    }

    void Update()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (GameManager.gm.ManageRollingDice[1].isAllowed && GameManager.gm.blueCompletePlayers == 4)
        {
            HandleWinning(GameManager.gm.BluePlayerName.text, BlueWinner, 1, playerCount);
            GameManager.gm.ManageRollingDice[1].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;
        }
        else if (GameManager.gm.ManageRollingDice[0].isAllowed && GameManager.gm.redCompletePlayers == 4)
        {
            HandleWinning(GameManager.gm.RedPlayerName.text, RedWinner, 0, playerCount);
            GameManager.gm.ManageRollingDice[0].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;
        }
        else if (GameManager.gm.ManageRollingDice[2].isAllowed && GameManager.gm.yellowCompletePlayers == 4)
        {
            HandleWinning(GameManager.gm.YellowPlayerName.text, YellowWinner, 2, playerCount);
            GameManager.gm.ManageRollingDice[2].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;
        }
        else if (GameManager.gm.ManageRollingDice[3].isAllowed && GameManager.gm.greenCompletePlayers == 4)
        {
            HandleWinning(GameManager.gm.GreenPlayerName.text, GreenWinner, 3, playerCount);
            GameManager.gm.ManageRollingDice[3].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;
        }

        if (GameManager.gm.PlayerRemainingToPlay == 1)
        {
            foreach (var k in GameManager.gm.ManageRollingDice)
            {
                if (k.isAllowed)
                {
                    if (k.name.Contains("Red"))
                        GameManager.gm.redCompletePlayers = 4;
                    if (k.name.Contains("Blue"))
                        GameManager.gm.blueCompletePlayers = 4;
                    if (k.name.Contains("Green"))
                        GameManager.gm.greenCompletePlayers = 4;
                    if (k.name.Contains("Yellow"))
                        GameManager.gm.yellowCompletePlayers = 4;
                }
            }

            WinningScreen.gameObject.SetActive(true);
        }
    }

    void HandleWinning(string playerName, GameObject winnerPrefab, int currentPosition, int playerCount)
    {
        GameObject WinningTag = GameManager.gm.ManageRollingDice[currentPosition].transform.parent.GetChild(3).gameObject;
        WinningTag.SetActive(true);

        GameObject op = Instantiate(winnerPrefab, WinnerList.transform);
        op.GetComponentInChildren<TMP_Text>().text = position.ToString();
        op.transform.GetChild(2).GetComponent<TMP_Text>().text = playerName;
        WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
        int prize = CalculatePrize(position, playerCount);
        position++;
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(UpdateBalance(playerName, prize));
        }
    }

    int CalculatePrize(int position, int playerCount)
    {
        if (playerCount == 2)
        {
            // 1st player gets all, 2nd gets nothing
            return position == 1 ? 2*totalPrizeMoney : 0;
        }
        else if (playerCount == 4)
        {
            if (position == 1)
                return (int)(2*totalPrizeMoney * 0.5); // 50% to 1st
            else if (position == 2)
                return (int)(2*totalPrizeMoney * 0.3); // 30% to 2nd
            else if (position == 3)
                return (int)(2*totalPrizeMoney * 0.2); // 20% to 3rd
            else
                return 0; // 4th gets nothing
        }
        return 0; // Default case
    }

    IEnumerator UpdateBalance(string username, int amount)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("amount", amount);

        UnityWebRequest www = UnityWebRequest.Post("https://phpstack-1216068-4319747.cloudwaysapps.com/transactions.php", form);
        BasicUI.instance.showLoader();
        yield return www.SendWebRequest();
        BasicUI.instance.hideLoader();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to update balance for " + username + ": " + www.error);
        }
        else
        {
            Debug.Log("Balance updated for " + username + ": " + amount);
        }

    }

    public void ReturnToHomeScreen()
    {
        PhotonNetwork.Disconnect();
        foreach (var op in GameObject.FindGameObjectsWithTag("Launcher"))
        {
            Destroy(op.gameObject);
        }

        // Load the home screen scene
        SceneManager.LoadScene("HomeScreen");
    }
}
