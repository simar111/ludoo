using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        ConnectToPhotonServer();
    }

    private void ConnectToPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server.");
        // You can add additional logic here, such as joining a lobby or creating/joining a room.
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected from Photon server: {0}", cause);
        // You can add additional logic here, such as displaying a reconnection UI or returning to the main menu.
    }
}