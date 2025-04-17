using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        string gameVersion = "1";
        bool isConnecting;

        #endregion


        public CoinMovement coin1;
        public CoinMovement coin2;


        public RectTransform rectTransform;
        private float duration = 1.0f; // Duration for one full move from 0 to 1245


        public TMP_Text myName;
        public TMP_Text opponent;


        public List<Player> LeftPlayers; 

        #region MonoBehaviour CallBacks

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Debug.Log("Launcher: Awake called.");

        }

       public void StartMyGame()
        {
            Debug.Log("Launcher: Start called.");
            StartCoroutine(AnimateRectTransformPosY());
            SetPlayerNicknameAndProperties();
            myName.text = DBManager.username;
            Connect();
        }

        #endregion

        #region Public Methods

        public void BreackConnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Launcher: Already connected to Photon.");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.Log("Launcher: Connecting to Photon.");
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Launcher: OnJoinRandomFailed called. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2,PublishUserId=true });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom called. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                photonView.RPC("SetImage", RpcTarget.All);

                photonView.RPC("coinMove", RpcTarget.All);
             
                photonView.RPC("startDelayAnim", RpcTarget.All);


            }


        }

        [PunRPC]
        void SetImage()
        {

            if (rectTransform.transform.childCount > 1)
            {
                // Start from index 1 to keep the first child
                for (int i = 1; i < rectTransform.transform.childCount; i++)
                {
                    Destroy(rectTransform.transform.GetChild(i).gameObject);
                }
            }

            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {

                if (!players[i].IsLocal)
                {
                    if (players[i].CustomProperties.TryGetValue<int>("Image", out int Image))
                        rectTransform.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[Image];
                    opponent.text = players[i].NickName;
                }
            }

        }



        IEnumerator AnimateRectTransformPosY()
        {
            while (true && rectTransform!=null)
            {
                // Start from 0 and move to 1245
                yield return MoveRectTransformPosY(0, 1245, duration);

                // Reset position to 0
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
                
               
           }
        }

        IEnumerator MoveRectTransformPosY(float startY, float endY, float duration)
        {
            float elapsedTime = 0;
            Vector2 startPos = new Vector2(rectTransform.anchoredPosition.x, startY);
            Vector2 endPos = new Vector2(rectTransform.anchoredPosition.x, endY);

            while (elapsedTime < duration && rectTransform!=null)
            {
                // Calculate the new position based on the elapsed time
                float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
                
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);

                // Increment the elapsed time by the frame's time
                elapsedTime += Time.deltaTime;

                // Yield until the next frame
                yield return null;
            }

            // Ensure the final position is exactly the end position
            rectTransform.anchoredPosition = endPos;
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
                // Continue only if the balance update was successful
                yield return new WaitForSeconds(1f);
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




        #endregion



        [PunRPC]
        void coinMove()
        {
            rectTransform.gameObject.GetComponentInParent<AudioSource>().Pause();
            rectTransform.gameObject.GetComponent<AudioSource>().Play();
            StopCoroutine(AnimateRectTransformPosY());
            StopCoroutine("MoveRectTransformPosY");


            StopAllCoroutines();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
            coin1.gameObject.SetActive(true);
            coin2.gameObject.SetActive(true);
            coin1.enabled = true;
            coin2.enabled = true;
        }

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher: OnConnectedToMaster called.");
            if (isConnecting)
            {
                Debug.Log("Launcher: Joining random room.");
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
 
            isConnecting = false;

        }


        public override void OnPlayerEnteredRoom(Player player)
        {
            Debug.Log("Launcher: OnJoinedRoom called. Now this client is in a room.");
/*            photonView.RPC("setYellow", RpcTarget.AllBuffered, player);*/
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


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            photonView.RPC("RemovePlayer", RpcTarget.All);
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
/*            LeftPlayers.Add(otherPlayer);*/
        }


        [PunRPC]
        void RemovePlayer()
        {
            GameManager.gm.PlayerRemainingToPlay--;

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
                // Add similar conditions for other pieces if necessary
            }
        }


        void TransferOwnership(Player player, int viewID)
        {
            PhotonView targetPhotonView = PhotonView.Find(viewID);
            if (targetPhotonView != null)
            {
                targetPhotonView.TransferOwnership(player);
                Debug.LogFormat("Transferred ownership of PhotonView ID {0} to player {1}", viewID, player.UserId);
    /*            Debug.LogFormat("PhotonView ID {0} is now owned by {1}", viewID, targetPhotonView.Owner.UserId);*/
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

        #endregion



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
    }


   





}
