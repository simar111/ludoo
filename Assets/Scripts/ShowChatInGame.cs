using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class ShowChatInGame : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputField;
    public TMP_Text player0Text;
    public TMP_Text player1Text;
    public TMP_Text player2Text;
    public TMP_Text player3Text;
    public Image img0;
    public Image img1;
    public Image img2;
    public Image img3;


    void Start()
    {
        if (inputField == null || player0Text == null || player1Text == null || player2Text == null || player3Text == null)
        {
            Debug.LogError("Please assign the inputField, sendButton, and player TMP_Texts in the inspector.");
            return;
        }

    }

    public void OnSendButtonPressed()
    {
        string message = inputField.text;
        Debug.LogWarning(message); 
        photonView.RPC("SendMessageToAll", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId, message);

        // Clear the input field
        inputField.text = string.Empty;
    }

    [PunRPC]
    void SendMessageToAll(string userId, string message)
    {
        if (userId == PhotonNetwork.PlayerList[0].UserId)
        {
            UpdatePlayerText(player0Text, img0, message);
            StartCoroutine(HideTextAfterDelay(player0Text, img0));
        }
        else if (userId == PhotonNetwork.PlayerList[1].UserId)
        {
            UpdatePlayerText(player1Text, img1, message);
            StartCoroutine(HideTextAfterDelay(player1Text, img1));
        }
        else if (userId == PhotonNetwork.PlayerList[2].UserId)
        {
            UpdatePlayerText(player2Text, img2, message);
            StartCoroutine(HideTextAfterDelay(player2Text, img2));
        }
        else if (userId == PhotonNetwork.PlayerList[3].UserId)
        {
            UpdatePlayerText(player3Text, img3, message);
            StartCoroutine(HideTextAfterDelay(player3Text, img3));
        }
    }

    void UpdatePlayerText(TMP_Text playerText, Image playerImage, string message)
    {
        playerImage.gameObject.SetActive(true);
        playerText.text = message;
    }

    IEnumerator HideTextAfterDelay(TMP_Text playerText, Image playerImage)
    {
        yield return new WaitForSeconds(2f);
        playerText.text = string.Empty;
        playerImage.gameObject.SetActive(false);
    }
}
