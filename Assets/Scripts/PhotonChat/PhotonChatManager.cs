using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    #region Setup
    bool isConnected;
    ChatClient chatClient;
    [SerializeField] string userID;

    public void UserNameOnValueChange(string valueIn)
    {
        userID = valueIn;
    }

  private void Start()
    {
        ChatConnectOnClick();
    }


    public void ChatConnectOnClick()
    {
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
        Debug.Log("CONNECTING...");
    }
    #endregion Setup



    [SerializeField] GameObject chatPanel;
    [SerializeField] TMP_InputField chatField;
    [SerializeField] TMP_Text chatDisplay;
    [SerializeField] GameObject contentArea;
    string privatereceiver = "";
    string currentChat;


    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }

    public void SubmitPublicChatOnClick()
    {
        if (privatereceiver == "") {
            chatClient.PublishMessage("RegionalChannel", currentChat);
            chatField.text = "";
            currentChat = "";
        }
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.LogWarning("Pata Nahi kya hoga");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.LogWarning("About to connect");
    }

    public void OnConnected()
    {
        Debug.Log("CONNECTED");

        chatClient.Subscribe(new string[] { "RegionalChannel" });
    }
    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {

     
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log(messages[i]);
           string msgs = string.Format("{0} : {1}", senders[i], messages[i]);
            TMP_Text op=Instantiate(chatDisplay, contentArea.transform);
            op.text += "\n" + msgs;
            Debug.Log(msgs);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        chatPanel.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }



    // Update is called once per frame
    void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
        }

        if (chatField.text != "" && Input.GetKey(KeyCode.Return)) {
            SubmitPublicChatOnClick();
        }
    }
}
