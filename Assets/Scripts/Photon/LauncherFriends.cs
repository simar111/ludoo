using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Xml.Linq;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Com.MyCompany.MyGame
{
    public class LauncherFriends : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        string gameVersion = "1";
        bool isConnecting;
        string roomCode;

        #endregion

        public GameObject ConnectFriends;
        public GameObject Friends;
        public TMP_Text code;
        public CoinMovement[] coin;
        public TMP_InputField joinCode;
        public GameObject PlayButton;
        public TMP_Text MyName;
        private float duration = 1.0f; // Duration for one full move from 0 to 1245

        #region MonoBehaviour CallBacks


        public TMP_Text[] opponents;

        

        public Image player2;
        public Image player3;
        public Image player4;


        public void openWhatsapp()
        {
            string url = "https://api.whatsapp.com/send?text=Hey! 🎲 I've created a room in Ludo Supreme. Join me using the room code: "+roomCode+". Let's play!";
  

            Application.OpenURL(url);
        }

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Debug.Log("Launcher: Awake called.");
        }

        void Start()
        {
            Debug.Log("Launcher: Start called.");
            Debug.Log("Launcher: Not connected to Photon. Connecting...");
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;


            MyName.text = DBManager.username;
            SetPlayerNicknameAndProperties();
        }

        #endregion

        #region Public Methods

        public void CreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                roomCode = GenerateRoomCode();
                code.text = roomCode;
                Debug.Log("Launcher: Creating room with code " + roomCode);
                RoomOptions options = new RoomOptions { MaxPlayers = 4, PublishUserId = true };
                PhotonNetwork.CreateRoom(roomCode, options);
            }
            else
            {
                Debug.LogWarning("Launcher: Not connected to Photon. Cannot create room.");
            }
        }


        public void BreackConnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void JoinRoom()
        {
            if (joinCode.text.Length == 6)
            {
                Debug.Log("Launcher: Joining room with code " + joinCode.text);
                PhotonNetwork.JoinRoom(joinCode.text);
            }
        }

        string GenerateRoomCode()
        {
            return Random.Range(100000, 999999).ToString();
        }

        public void PlayMyGame()
        {
            Debug.Log("Launcher: PlayMyGame called.");
            
           /*     photonView.RPC("coinMove", RpcTarget.All);*/
                photonView.RPC("startDelayAnim", RpcTarget.All);
           
        }

        [PunRPC]
        void startDelayAnim()
        {
            StartCoroutine(delayAnima());
        }

        IEnumerator delayAnima()
        {
            DontDestroyOnLoad(this.gameObject);

            // Wait for the total balance to be updated and ensure it's successful before proceeding
            bool isAmountDeducted = false;
            yield return StartCoroutine(UpdateTotalBalance(-100, result => isAmountDeducted = result)); // Deduct 100 from the total balance

            if (isAmountDeducted)
            {
                yield return new WaitForSeconds(1f);
                // Continue only if the balance update was successful
                PhotonNetwork.LoadLevel("SampleScene");
                photonView.RPC("AssignPieceScript", RpcTarget.All);
                photonView.RPC("AssignOwnershipToAllPlayers", RpcTarget.All);
            }
            else
            {
                Debug.LogError("Failed to deduct amount. Stopping further execution.");
            }
        }

        // Coroutine to update total balance
        IEnumerator UpdateTotalBalance(int amount, System.Action<bool> callback)
        {
            string username = DBManager.username; // Assuming the username is stored in PhotonNetwork.NickName
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("amount", amount); // Sending negative amount (-100) to deduct

            UnityWebRequest www = UnityWebRequest.Post("https://phpstack-1216068-4319747.cloudwaysapps.com/transactions.php", form);
            BasicUI.instance.showLoader();

            yield return www.SendWebRequest();
            BasicUI.instance.hideLoader();
            if (www.downloadHandler.text[0] != '0')
            {
                Debug.LogError("Failed to update balance for " + username + ": " + www.error);
                callback(false); // Indicate failure
            }
            else
            {
                Debug.Log("Balance updated for " + username + ": " + amount);
                callback(true); // Indicate success
            }
        }


        [PunRPC]
        void coinMove()
        {
         
            foreach (var i in coin)
            {
                i.gameObject.SetActive(true);
                i.enabled = true;
            }
        }



        [PunRPC]
        void SetImage()
        {
            Image[] playersp = { player2, player3, player4 };

            foreach (var player in playersp)
            {
                if (player.transform.childCount > 1)
                {
                    // Start from index 1 to keep the first child
                    for (int i = 1; i < player.transform.childCount; i++)
                    {
                        Destroy(player.transform.GetChild(i).gameObject);
                    }
                }
            }
            Player[] NonLocal = new Player[3];

            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0, j = 0; i < players.Length; i++)
            {

                if (!players[i].IsLocal)
                {
                    NonLocal[j] = players[i];
                    j++;
                }
            }
            for (int i = 0;NonLocal[i]!=null && i < NonLocal.Length; i++)
            {
                if (NonLocal[i].CustomProperties.TryGetValue<int>("Image", out int image))
                {

                    playersp[i].sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[image];
                }
                opponents[i].text = NonLocal[i].NickName;


            }

        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher: OnConnectedToMaster called.");
            if (isConnecting)
            {
                isConnecting = false;
                if (!string.IsNullOrEmpty(roomCode))
                {
                    Debug.Log("Launcher: Creating room with code " + roomCode);
                    RoomOptions options = new RoomOptions { MaxPlayers = 4, PublishUserId = true };
                    PhotonNetwork.CreateRoom(roomCode, options);
                }
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            Debug.LogWarningFormat("Launcher: OnDisconnected called with reason {0}", cause);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom called. Player has joined the room.");
            ConnectFriends.SetActive(true);
            Friends.SetActive(false);
            photonView.RPC("SetImage", RpcTarget.All);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("lplplp");
                code.text = roomCode;
            }
            else
            {
                Debug.Log("bnbnb");
                code.text = joinCode.text;
            }

          
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            Debug.Log("Launcher: OnPlayerEnteredRoom called. Player: " + player.NickName);
            Debug.Log(PhotonNetwork.PlayerList.Length);
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PlayButton.SetActive(true);

                }
            }
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            GameManager.gm.PlayerRemainingToPlay--;
            Debug.LogErrorFormat(GameManager.gm.PlayerRemainingToPlay.ToString());
            if (otherPlayer.CustomProperties.TryGetValue("Piece", out object pieceType))
            {
                string pieceTypeName = pieceType as string;

                if (pieceTypeName == "YellowPiece")
                {
                    GameManager.gm.ManageRollingDice[2].isAllowed = false;
                }
                if (pieceTypeName == "RedPiece")
                {
                    GameManager.gm.ManageRollingDice[0].isAllowed = false;
                }
                if (pieceTypeName == "BluePiece")
                {
                    GameManager.gm.ManageRollingDice[1].isAllowed = false;
                }
                if (pieceTypeName == "GreenPiece")
                {
                    GameManager.gm.ManageRollingDice[3].isAllowed = false;
                }
                // Add similar conditions for other pieces if necessary
            }
            /*   LeftPlayers.Add(otherPlayer);*/
        }


        [PunRPC]
        void AssignPieceScript()
        {
            Debug.Log("Launcher: AssignPieceScript called.");

            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                Player player = players[i];
                string piece = "";

                switch (i)
                {
                    case 0:
                        piece = "RedPiece";
                        break;
                    case 1:
                        piece = "YellowPiece";
                        break;
                    case 2:
                        piece = "BluePiece";
                        break;
                    case 3:
                        piece = "GreenPiece";
                        break;
                }

                if (player.IsLocal)
                {
                    Debug.LogFormat("Launcher: Assigning {0} to local player {1}", piece, player.UserId);
                }
                else
                {
                    Debug.LogFormat("Launcher: Assigning {0} to player {1}", piece, player.UserId);
                }

                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Piece", piece } });
            }
        }

        public void AssignOwnershipBasedOnPiece(Player player)
        {
            if (player.CustomProperties.TryGetValue("Piece", out object pieceType))
            {
                string pieceTypeName = pieceType as string;

                if (pieceTypeName == "YellowPiece")
                {
                    TransferOwnership(player, 3);
                    GameManager.gm.ManageRollingDice[2].gameObject.SetActive(true);
                    TransferOwnership(player, 5);
                    GameManager.gm.ManageRollingDice[2].gameObject.SetActive(false);
                }
                if (pieceTypeName == "RedPiece")
                {
                    TransferOwnership(player, 1);
                    TransferOwnership(player, 7);
                }
                if (pieceTypeName == "BluePiece")
                {
                    TransferOwnership(player, 2);
                    GameManager.gm.ManageRollingDice[1].gameObject.SetActive(true);
                    TransferOwnership(player, 8);
                    GameManager.gm.ManageRollingDice[1].gameObject.SetActive(false);
                }
                if (pieceTypeName == "GreenPiece")
                {
                    TransferOwnership(player, 4);
                    GameManager.gm.ManageRollingDice[3].gameObject.SetActive(true);
                    TransferOwnership(player, 100);
                    GameManager.gm.ManageRollingDice[3].gameObject.SetActive(false);
                }
            }
        }

        void TransferOwnership(Player player, int viewID)
        {
            PhotonView targetPhotonView = PhotonView.Find(viewID);
            if (targetPhotonView != null)
            {
                targetPhotonView.TransferOwnership(player);
                Debug.LogFormat("Transferred ownership of PhotonView ID {0} to player {1}", viewID, player.UserId);
            }
            else
            {
                Debug.LogError("PhotonView with ID " + viewID + " not found.");
            }
        }

        [PunRPC]
        void AssignOwnershipToAllPlayers()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                AssignOwnershipBasedOnPiece(player);
            }
        }


        private void SetPlayerNicknameAndProperties()
        {
            // Set a unique player nickname
            PhotonNetwork.NickName = DBManager.username;

            // Define custom properties
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "Image", (int)DBManager.myImage },
            };

            // Set custom properties for the local player
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

            Debug.Log("Launcher: Player nickname and custom properties set.");
        }
        #endregion
    }
}
