using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExitGames.Client.Photon;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using static Photon.Pun.Demo.Shared.DocLinks;

[Serializable]
public class PathPoint : MonoBehaviourPunCallbacks,IPunObservable
{
    PathPoint[] pathPointToMoveOn_;
    public PathObjectParent pathObjectParent;
    public List<String> PlayerPieceList = new List<String>();

    public List<PlayerPiece> CollisionWali= new List<PlayerPiece>();

    public bool returnTurn=true;

    private const int MaxChunkSize = 1024; // Adjust the chunk size based on your requirements
    private List<string> receivedChunks = new List<string>();

    /*    private void OnTriggerEnter2D(Collider2D collision)
        {
            CollisionWali.Add(collision.GetComponent<PlayerPiece>());




            if (CollisionWali.Count == 2 && ((!CollisionWali[0].isMoving && !CollisionWali[1].isMoving) || GameManager.gm.numberOfStepsToMove == 1) && this.name != "PathPoint" && this.name != "PathPoint (47)" && this.name != "PathPoint (8)" && this.name != "PathPoint (13)" && this.name != "PathPoint (21)" && this.name != "PathPoint (26)" && this.name != "PathPoint (34)" && this.name != "PathPoint (39)" && this.name != "CenterPathPoint")
            {
                Debug.LogWarning(this.name);
                string preePlayerPieceName = CollisionWali[0].name;
                string curPlayerPiecename = CollisionWali[1].name;
                curPlayerPiecename = curPlayerPiecename.Substring(0, curPlayerPiecename.Length - 4);

                if (!preePlayerPieceName.Contains(curPlayerPiecename))
                {
                    GameManager.gm.transferdice = false;
                    GameManager.gm.selfDice = true;
                    CollisionWali[0].isReady = false;
                    CollisionWali[0].GetComponent<BoxCollider2D>().enabled = false;
                    CollisionWali[0].numberOfStepsToMove = 0;
                    PlayerPiece pink= CollisionWali[0];
                    reduceOnePlayer(CollisionWali[0]);
                    StartCoroutine(revertOnStart(CollisionWali[0]));



                    RemovePlayerPiece(CollisionWali[0]);
                    PlayerPieceList.Add(CollisionWali[0]);

                    CollisionWali[0].numberOfStepsAlreadyMove += GameManager.gm.numberOfStepsToMove;


                }

            }
            else if (CollisionWali.Count >= 2)
            {
                bool flag = true;
                foreach(var op in CollisionWali)
                {
                    if (op.isMoving)
                    {
                        flag = false; break;
                    }
                }
                if(flag || GameManager.gm.numberOfStepsToMove == 1)
                RescaleAndRepositioningAllPlayerCollisionWalli();

            }






        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(CollisionWali.Contains(collision.GetComponent<PlayerPiece>()))
            {
                CollisionWali.Remove(collision.GetComponent<PlayerPiece>());
            }
            RescaleAndRepositioningAllPlayerCollisionWalli();
        }
    */

    void AddMissingElements(List<PlayerPiece> mainList, List<PlayerPiece> comparisonList)
    {
        foreach (PlayerPiece piece in comparisonList)
        {
            if (!mainList.Contains(piece))
            {
                mainList.Add(piece);
            }
        }
    }


    private void Start()
    {
        pathObjectParent=GetComponentInParent<PathObjectParent>();
        this.photonView.ObservedComponents.Add(this);
        /*        RegisterCustomTypes();*/

    }

    /*    void RegisterCustomTypes()
        {
            *//*        PhotonPeer.RegisterType(typeof(PlayerPiece), (byte)'P', PlayerPiece.Serialize, PlayerPiece.Deserialize);*//*
            PhotonPeer.RegisterType(typeof(PlayerPiece), (byte)'P', PlayerPiece.Serialize, PlayerPiece.Deserialize);
        }*/


    /* Self dice and Cut number fix */

    [PunRPC]
    public void AddPlayerPiece(string str)
    {
        this.photonView.RPC("returnTrue", RpcTarget.All);
        PlayerPiece playerPiece = GameObject.FindWithTag(str).GetComponent<PlayerPiece>();
        foreach (var i in PlayerPieceList)
        {
            Debug.LogWarning(i);
        }
        if(this.name== "CenterPathPoint")
        {
            this.photonView.RPC("reduceOnePlayer", RpcTarget.AllBuffered, playerPiece.tag);
        }

        if (this.name != "PathPoint" && this.name != "PathPoint (47)" && this.name != "PathPoint (8)" && this.name != "PathPoint (13)" && this.name != "PathPoint (21)" && this.name != "PathPoint (26)" && this.name != "PathPoint (34)" && this.name != "PathPoint (39)" && this.name!= "CenterPathPoint")
        {
            if (PlayerPieceList.Count == 1)
            {
               PlayerPiece myPo= GameObject.FindWithTag(PlayerPieceList[0]).GetComponent<PlayerPiece>();
                string preePlayerPieceName =myPo.name;
                string curPlayerPiecename = playerPiece.name;
                curPlayerPiecename = curPlayerPiecename.Substring(0, curPlayerPiecename.Length - 4);

                if (!preePlayerPieceName.Contains(curPlayerPiecename))
                {
                    this.photonView.RPC("systemCheck", RpcTarget.AllBuffered,myPo.tag);
                    StartCoroutine(revertOnStart(myPo.tag));

                  
                    this.photonView.RPC("RemovePlayerPiece", RpcTarget.AllBuffered, myPo.tag);
                    PlayerPieceList.Add(playerPiece.tag);

                    this.photonView.RPC("returnFalse", RpcTarget.All);

                    return;
                }
            }
        }
        addPlayer(playerPiece);
        this.photonView.RPC("returnTrue", RpcTarget.All);
    }



    [PunRPC]
    void systemCheck(string str)
    {
        PlayerPiece myPo = GameObject.FindWithTag(str).GetComponent<PlayerPiece>();
        myPo.isReady = false;
        myPo.numberOfStepsToMove = 0;
    }

    [PunRPC]
    void returnFalse ()
    {

        returnTurn = false;
    }

    [PunRPC]
    void returnTrue()
    {
        returnTurn = true;
    }


    [PunRPC]
    void reduceOnePlayer(string str)
    {
        PlayerPiece playerPiece = GameObject.FindWithTag(str).GetComponent<PlayerPiece>();
        if (playerPiece.name.Contains("Blue"))
        {
            GameManager.gm.blueOutPlayers -= 1;
            GameManager.gm.blueCompletePlayers++;
        }
        else if (playerPiece.name.Contains("Red"))
        {
            GameManager.gm.redOutPlayers -= 1;
            GameManager.gm.redCompletePlayers++;

        }
        else if (playerPiece.name.Contains("Yellow"))
        {
            GameManager.gm.yellowOutPlayers -= 1;
            GameManager.gm.yellowCompletePlayers++;

        }
        else if (playerPiece.name.Contains("Green"))
        {
            GameManager.gm.greenOutPlayers -= 1;
            GameManager.gm.greenCompletePlayers++;

        }
    }

    IEnumerator revertOnStart(string str)
    {
        PlayerPiece playerPiece = GameObject.FindWithTag(str).GetComponent<PlayerPiece>();
        if (playerPiece.name.Contains("Blue"))
        {
          /*  GameManager.gm.blueOutPlayers -= 1;*/
            pathPointToMoveOn_ = pathObjectParent.BluePathPoint;
        }
        else if (playerPiece.name.Contains("Red"))
        {
         /*   GameManager.gm.redOutPlayers -= 1;*/
            pathPointToMoveOn_ = pathObjectParent.RedPathPoint;
        }
        else if (playerPiece.name.Contains("Yellow"))
        {
           /* GameManager.gm.yellowOutPlayers -= 1;*/
            pathPointToMoveOn_ = pathObjectParent.YellowPathPoint;
        }
        else if (playerPiece.name.Contains("Green"))
        {
           /* GameManager.gm.greenOutPlayers -= 1;*/
            pathPointToMoveOn_ = pathObjectParent.GreenPathPoint;
        }
/*        playerPiece.GetComponent<BoxCollider2D>().enabled = false;*/
        for (int i = playerPiece.numberOfStepsAlreadyMove-1;i>=0;i--)
        {
            playerPiece.transform.position = pathPointToMoveOn_[i].transform.position;
            yield return new WaitForSeconds(0.03f);
        }

        /*        playerPiece.GetComponent<BoxCollider2D>().enabled = true;*/
        yield return new WaitForSeconds(0.03f);
        playerPiece.transform.position = pathObjectParent.BasePathPoint[BasePointPosition(playerPiece.name)].transform.position;

    }
    int BasePointPosition(string name)
    {
       
        for(int i=0;i<pathObjectParent.BasePathPoint.Length;i++)
        {
            if (pathObjectParent.BasePathPoint[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    void addPlayer(PlayerPiece playerPiece)
    {
        PlayerPieceList.Add(playerPiece.tag);
        /*    this.photonView.RPC("RescaleAndRepositioningAllPlayer",RpcTarget.AllBuffered);*/
        RescaleAndRepositioningAllPlayer();

    }

    [PunRPC]
    public void RemovePlayerPiece(string str)
    {
        PlayerPiece playerPiece = GameObject.FindWithTag(str).GetComponent<PlayerPiece>();
        if (PlayerPieceList.Contains(playerPiece.tag))
        {
            PlayerPieceList.Remove(playerPiece.tag);
            /*this.photonView.RPC("RescaleAndRepositioningAllPlayer", RpcTarget.AllBuffered);*/
            RescaleAndRepositioningAllPlayer();
        }
    }
/*
    void compled(PlayerPiece playerPiece)
    {
        if (playerPiece.name.Contains("Blue"))
        {
            GameManager.gm.blueCompletePlayers += 1;
            GameManager.gm.blueOutPlayers -= 1;

            if (GameManager.gm.blueCompletePlayers == 4)
                ShowCeleberation();

        }
        else if (playerPiece.name.Contains("Red"))
        {
            GameManager.gm.redCompletePlayers += 1;
            GameManager.gm.redOutPlayers -= 1;

            if (GameManager.gm.redCompletePlayers == 4)
                ShowCeleberation();

        }
        else if (playerPiece.name.Contains("Yellow"))
        {
            GameManager.gm.yellowCompletePlayers += 1;
            GameManager.gm.yellowOutPlayers -= 1;

            if (GameManager.gm.yellowCompletePlayers == 4)
                ShowCeleberation();

        }
        else if (playerPiece.name.Contains("Green"))
        {
            GameManager.gm.greenCompletePlayers += 1;
            GameManager.gm.greenOutPlayers -= 1;

            if (GameManager.gm.greenCompletePlayers == 4)
                ShowCeleberation();

        }
    }*/


    void ShowCeleberation()
    {

    }

    [PunRPC]
    public void RescaleAndRepositioningAllPlayer()
    {
        int plsCount=PlayerPieceList.Count;
        bool isOdd=(plsCount%2)==0?false:true;
        int extent=plsCount/2;
        int counter = 0;
        int spriteLayer = 0;
        if (isOdd)
        {
            for(int i=-extent; i<=extent; i++)
            {
                GameObject.FindWithTag(PlayerPieceList[counter]).transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
                GameObject.FindWithTag(PlayerPieceList[counter]).transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount-1]),transform.position.y,0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extent; i < extent; i++)
            {
                GameObject.FindWithTag(PlayerPieceList[counter]).transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
                GameObject.FindWithTag(PlayerPieceList[counter]).transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for(int i = 0; i < PlayerPieceList.Count; i++)
        {
            GameObject.FindWithTag(PlayerPieceList[i]).GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }


    public void RescaleAndRepositioningAllPlayerCollisionWalli()
    {
        int plsCount = CollisionWali.Count;
        bool isOdd = (plsCount % 2) == 0 ? false : true;
        int extent = plsCount / 2;
        int counter = 0;
        int spriteLayer = 0;
        if (isOdd)
        {
            for (int i = -extent; i <= extent; i++)
            {
                CollisionWali[counter].transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
                CollisionWali[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extent; i < extent; i++)
            {
                CollisionWali[counter].transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
                CollisionWali[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for (int i = 0; i < CollisionWali.Count; i++)
        {
            CollisionWali[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }

/*    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            // Convert the list to a JSON string
            string json = JsonConvert.SerializeObject(PlayerPieceList, jsonSettings);
            // Send the JSON string over the network
            stream.SendNext(json);
        }
        else
        {
            // Receive the JSON string from the network
            string json = (string)stream.ReceiveNext();
            // Deserialize the JSON string back to the list
            PlayerPieceList = JsonConvert.DeserializeObject<List<string>>(json, jsonSettings);
        }
    }
*/


    public static byte[] Serialize(object obj)
    {
        PathPoint piece = (PathPoint)obj;
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, piece);
            return ms.ToArray();
        }
    }


    public static object Deserialize(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (PathPoint)formatter.Deserialize(ms);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {/*
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(GameManager.gm.redOutPlayers);
            stream.SendNext(GameManager.gm.blueOutPlayers);
            stream.SendNext(GameManager.gm.yellowOutPlayers);
            stream.SendNext(GameManager.gm.greenOutPlayers);
        }
        else
        {
            // Network player, receive data
            GameManager.gm.redOutPlayers = (int)stream.ReceiveNext();
            GameManager.gm.blueOutPlayers = (int)stream.ReceiveNext();
            GameManager.gm.greenOutPlayers = (int)stream.ReceiveNext();
            GameManager.gm.yellowOutPlayers = (int)stream.ReceiveNext();

        }*/
    }
}
