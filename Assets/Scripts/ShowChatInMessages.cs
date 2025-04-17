using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Voice.Unity.Demos;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using System.Linq;

public class ShowChatInMessages : MonoBehaviourPunCallbacks
{
    public TMP_InputField messageInputField;

    public Button sendChatButton;

    private List<string> chatHistory = new List<string>();
    public GameObject chatScrollView;
    public GameObject chatMessageTemplate;

    int i = 0;

    void Start()
    {
        if (messageInputField == null || sendChatButton == null || chatScrollView == null  || chatMessageTemplate == null)
        {
            Debug.LogError("Please assign all required references in the inspector.");
            return;
        }
        chatScrollView.GetComponent<RectTransform>().SetHeight(chatScrollView.GetComponent<RectTransform>().anchoredPosition.y);
    }

   public void OnSendChatButtonClicked()
    {
        string chatMessage = messageInputField.text;
        if (!string.IsNullOrEmpty(chatMessage))
        {
            Debug.Log(chatMessage);
            photonView.RPC("BroadcastChatMessage", RpcTarget.All, PhotonNetwork.NickName, chatMessage);
        }
        messageInputField.text = string.Empty;
    }


    [PunRPC]
    void BroadcastChatMessage(string senderId, string chatMessage)
    {
        chatHistory.Add(chatMessage);

        GameObject newChatMessage = AddChatMessageToScrollView(senderId,chatMessage);

        // Check if the message is from someone else (not the local player)
        if (senderId != PhotonNetwork.NickName)
        {
            // Rotate the chat message by 180 degrees
            RectTransform rectTransform = newChatMessage.GetComponent<RectTransform>();
            rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            Debug.LogWarning(newChatMessage.GetComponentInChildren<TMP_Text>().gameObject.name);
            newChatMessage.GetComponentInChildren<TMP_Text>().gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void UpdatePlayerChatDisplay(TMP_Text playerChatText, Image playerChatImage, string chatMessage)
    {
        playerChatImage.gameObject.SetActive(true);
        playerChatText.text = chatMessage;
    }

    IEnumerator HidePlayerChatAfterDelay(TMP_Text playerChatText, Image playerChatImage)
    {
        yield return new WaitForSeconds(2f);
        playerChatText.text = string.Empty;
        playerChatImage.gameObject.SetActive(false);
    }

    GameObject AddChatMessageToScrollView(string senderId, string chatMessage)
    {
        // Instantiate the chat message and set its parent to the ScrollView's content
        GameObject newChatMessage = Instantiate(chatMessageTemplate, chatScrollView.transform);

        // Set the text of the chat message
        TMP_Text chatMessageText = newChatMessage.GetComponentInChildren<TMP_Text>();
        chatMessageText.text = chatMessage;

        // Set the position of the new chat message
        RectTransform rectTransform = newChatMessage.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 110 + i * 150, 0);

        // Access the Image component within the chatMessageTemplate
        Image profileImage = rectTransform.gameObject.transform.GetChild(2).GetComponent<Image>();

        // Get the player by senderId
        Player senderPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => p.NickName == senderId);

        if (senderPlayer != null && senderPlayer.CustomProperties.TryGetValue<int>("Image", out int imageIndex))
        {
            profileImage.sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[imageIndex];
        }

        // Update the layout manually
        Canvas.ForceUpdateCanvases();

        // Increment the index for the next message
        i++;

        if (i > 11)
        {
            chatScrollView.GetComponent<RectTransform>().SetHeight(chatScrollView.GetComponent<RectTransform>().anchoredPosition.y + 150 * i - 12 * 150);
        }

        // Return the instantiated chat message GameObject
        return newChatMessage;
    }

}
