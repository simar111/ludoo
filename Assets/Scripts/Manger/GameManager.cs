using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager gm;
    public RollingDice dice;

    public int numberOfStepsToMove;
    public bool canPlayerMove = true;
    public bool canDiceRoll = true;
    public bool transferdice = false;
    public bool selfDice = false;

    public int redOutPlayers = 4;
    public int greenOutPlayers = 4;
    public int blueOutPlayers = 4;
    public int yellowOutPlayers = 4;

    public int redCompletePlayers;
    public int greenCompletePlayers;
    public int blueCompletePlayers;
    public int yellowCompletePlayers;

    public GameObject RedRollDiceHome;
    public GameObject BlueRollDiceHome;
    public GameObject YellowRollDiceHome;
    public GameObject GreenRollDiceHome;


    public PlayerPiece[] bluePlayerPiece;
    public PlayerPiece[] redPlayerPiece;
    public PlayerPiece[] greenPlayerPiece;
    public PlayerPiece[] yelloPlayerPiece;

    public int totalPlayerCanPlay;

    public RollingDice[] ManageRollingDice;

    List<PathPoint> playerOnPathPointList = new List<PathPoint>();


    public bool isRedPlayerPlaying = false;
    public bool isGreenPlayerPlaying = false;
    public bool isBluePlayerPlaying = false;
    public bool isYellowPlayerPlaying = false;

    public byte PlayerRemainingToPlay = 4;


    public GameObject Board;

    public GameObject OrangeCanvasTemp;
    public GameObject RedCanvasTemp;
    public GameObject LudoPath;
    public GameObject LudoHome;

    private Coroutine diceTimerCoroutine;

    public GameObject RedImage;
    public GameObject BlueImage;
    public GameObject YellowImage;
    public GameObject GreenImage;

    public TMP_Text RedPlayerName;
    public TMP_Text BluePlayerName;
    public TMP_Text YellowPlayerName;
    public TMP_Text GreenPlayerName;
    private void Awake()
    {
        gm = this;


        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            BlueRollDiceHome.SetActive(false);
            GreenRollDiceHome.SetActive(false);
            HidePlayers(GameManager.gm.bluePlayerPiece);
            HidePlayers(GameManager.gm.greenPlayerPiece);
            GameManager.gm.totalPlayerCanPlay = 2;


            if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
            {

                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
                for (int i = 0; i < 4; i++)
                {
                    Debug.Log("lp0");
                    redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);

                }
            }

            GameManager.gm.ManageRollingDice[1].isAllowed = false;
            GameManager.gm.ManageRollingDice[3].isAllowed = false;
            RedPlayerName.text = PhotonNetwork.PlayerList[0].NickName;
            YellowPlayerName.text = PhotonNetwork.PlayerList[1].NickName;



            if (PhotonNetwork.PlayerList[0].CustomProperties.TryGetValue("Image", out object imageObj))
            {
                int image = (int)imageObj;
                RedImage.GetComponent<Image>().sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[image];
            }
            if (PhotonNetwork.PlayerList[1].CustomProperties.TryGetValue("Image", out object imageObjp))
            {
                int image = (int)imageObjp;
                YellowImage.GetComponent<Image>().sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[image];
            }


            PlayerRemainingToPlay = 2;
            /* if (pieceTypeName == "RedPiece")
             {
                 TransferOwnership(player, 1);
                 TransferOwnership(player, 7);
             }*/

        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {

            GameManager.gm.totalPlayerCanPlay = 4;
            PlayerRemainingToPlay = 4;
            RedPlayerName.text = PhotonNetwork.PlayerList[0].NickName;
            YellowPlayerName.text = PhotonNetwork.PlayerList[1].NickName;
            BluePlayerName.text = PhotonNetwork.PlayerList[2].NickName;
            GreenPlayerName.text = PhotonNetwork.PlayerList[3].NickName;



            if (PhotonNetwork.PlayerList[0].CustomProperties.TryGetValue("Image", out object imageObj))
            {
                int image = (int)imageObj;
                RedImage.GetComponent<Image>().sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[image];
            }
            if (PhotonNetwork.PlayerList[1].CustomProperties.TryGetValue("Image", out object imageObjp))
            {
                int image = (int)imageObjp;
                YellowImage.GetComponent<Image>().sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[image];
            }
            if (PhotonNetwork.PlayerList[2].CustomProperties.TryGetValue("Image", out object imageObjl))
            {
                int image = (int)imageObjl;
                BlueImage.GetComponent<Image>().sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[image];
            }
            if (PhotonNetwork.PlayerList[1].CustomProperties.TryGetValue("Image", out object imageObjpn))
            {
                int image = (int)imageObjpn;
                GreenImage.GetComponent<Image>().sprite = Resources.Load<SpriteCollection>("NewSpriteCollection").sprites[image];
            }


            if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
            {

                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
                for (int i = 0; i < 4; i++)
                {

                    redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    bluePlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    greenPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);

                }
            }
            else
            if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[2].UserId)
            {

                boardSetUP(0);
            }
            else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[3].UserId)
            {
                boardSetUP(0);
                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
                for (int i = 0; i < 4; i++)
                {
                    redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
                    yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
                    bluePlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
                    greenPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);

                }
            }
            /* if (pieceTypeName == "RedPiece")
             {
                 TransferOwnership(player, 1);
                 TransferOwnership(player, 7);
             }*/

        }


      


        // Add similar conditions for other pieces if necessary
    }
        

    

    private void Start()
    {
        /*  GameManager.gm.dice = GameManager.gm.ManageRollingDice[0];*/
      /*  diceTimerCoroutine=StartCoroutine(DiceTimer(GameManager.gm.ManageRollingDice[0]));*/



        
    }


    [PunRPC]
    void reduceOnePlayer(string str)
    {

        if (str.Contains("Blue"))
        {
            GameManager.gm.blueOutPlayers -= 1;
            GameManager.gm.blueCompletePlayers++;
        }
        else if (str.Contains("Red"))
        {
            GameManager.gm.redOutPlayers -= 1;
            GameManager.gm.redCompletePlayers++;

        }
        else if (str.Contains("Yellow"))
        {
            GameManager.gm.yellowOutPlayers -= 1;
            GameManager.gm.yellowCompletePlayers++;

        }
        else if (str.Contains("Green"))
        {
            GameManager.gm.greenOutPlayers -= 1;
            GameManager.gm.greenCompletePlayers++;

        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            photonView.RPC("reduceOnePlayer", RpcTarget.All, "Red");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            photonView.RPC("reduceOnePlayer", RpcTarget.All, "Yellow");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            photonView.RPC("reduceOnePlayer", RpcTarget.All, "Green");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            photonView.RPC("reduceOnePlayer", RpcTarget.All, "Blue");
        }
    }


    void HidePlayers(PlayerPiece[] playerPieces)
    {
        for (int i = 0; i < playerPieces.Length; i++)
        {
            playerPieces[i].gameObject.SetActive(false);
        }
    }
    /*
        void Start()
        {
            // Find all GameObjects in the scene
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            Debug.Log("lplpp");
            // List to store game objects with Rigidbody component
            List<GameObject> objectsWithRigidbodies = new List<GameObject>();

            // Iterate through all game objects and check for Rigidbody component
            foreach (GameObject obj in allObjects)
            {
                if (obj.GetComponent<Rigidbody>() != null)
                {
                    objectsWithRigidbodies.Add(obj);
                    Debug.Log("Found Rigidbody on: " + obj.name);
                }
            }

            // Optionally, do something with the list of objects with Rigidbody components
            // For example, print the names of these objects
            foreach (GameObject obj in objectsWithRigidbodies)
            {
                Debug.Log("GameObject with Rigidbody: " + obj.name);
            }
        }*/




    public void AddPathPoint(PathPoint pathPoint)
    {
        playerOnPathPointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playerOnPathPointList.Contains(pathPoint))
        {
            playerOnPathPointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path Not found to be romved");
        }
    }


    public void RollingDiceManager()
    {
       
        if (GameManager.gm.transferdice)
        {
            if (GameManager.gm.numberOfStepsToMove != 6)
            {
                ShiftDice();
            }

            /* for(int i = 0; i < 4; i++)
             {
                 if (i == 3)
                 {
                     nextDice = 0;
                 }
                 else
                 {
                     nextDice = i + 1;
                 }
                 if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[i]){

                     GameManager.gm.ManageRollingDice[i].gameObject.SetActive(false);
                     GameManager.gm.ManageRollingDice[nextDice].gameObject.SetActive(true);
                 }
             }*/
            GameManager.gm.canDiceRoll = true;
        }
        else
        {
            if (GameManager.gm.selfDice)
            {
                GameManager.gm.selfDice = false;
                GameManager.gm.canDiceRoll = true;
                GameManager.gm.SelfRoll();
            }
        }
    }

    public void SelfRoll()
    {
        if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
        {
            Invoke("roled", 0.6f);
        }
    }

    void roled()
    {
        GameManager.gm.ManageRollingDice[2].mouseRoll();
    }

    [PunRPC]
    void getYellow()
    {
        GameManager.gm.ManageRollingDice[0].gameObject.SetActive(false);
        GameManager.gm.ManageRollingDice[2].gameObject.SetActive(true);
   /*     GameManager.gm.dice = GameManager.gm.ManageRollingDice[2];*/
  /*      diceTimerCoroutine = StartCoroutine(DiceTimer(GameManager.gm.ManageRollingDice[2]));*/
    }

    [PunRPC]
    void getRed()
    {

        GameManager.gm.ManageRollingDice[0].gameObject.SetActive(true);
        GameManager.gm.ManageRollingDice[2].gameObject.SetActive(false);
    /*    GameManager.gm.dice = GameManager.gm.ManageRollingDice[0];*/
       /* diceTimerCoroutine = StartCoroutine(DiceTimer(GameManager.gm.ManageRollingDice[0]));*/
    }

    void ShiftDice()
    {
        int nextDice;
        if (GameManager.gm.totalPlayerCanPlay == 1)
        {
            if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
            {
                GameManager.gm.ManageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.ManageRollingDice[2].gameObject.SetActive(true);
                passout(0);
                GameManager.gm.ManageRollingDice[2].mouseRoll();
            }
            else
            {

                GameManager.gm.ManageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.ManageRollingDice[2].gameObject.SetActive(false);
                passout(2);

            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 2)
        {
            if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
            {/*
                if(photonView.IsMine)*/
                photonView.RPC("getYellow", RpcTarget.AllBuffered);
                passout(0);
            }
            else
            {
                /* if (photonView.IsMine)*/
                photonView.RPC("getRed", RpcTarget.AllBuffered);
                passout(2);

            }
        }

        else if (GameManager.gm.totalPlayerCanPlay >2)
        {
            int currentDiceIndex = 0;
            int nextDiceIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[i])
                {
                    currentDiceIndex = i;
                    break;
                }

            }



            if (currentDiceIndex == 3)
            {
                nextDiceIndex = 0;
            }
            else
            {
                nextDiceIndex = currentDiceIndex + 1;
            }


            while (!GameManager.gm.ManageRollingDice[nextDiceIndex].isAllowed)
            {

                nextDiceIndex++;
                if (nextDiceIndex > 3)
                {
                    nextDiceIndex = 0;
                }
            }


            photonView.RPC("changeoDice", RpcTarget.All, currentDiceIndex, nextDiceIndex);
        }
   
    }

    [PunRPC]
    void changeoDice(int i, int nextDice)
    {

        GameManager.gm.ManageRollingDice[i].gameObject.SetActive(false);
        GameManager.gm.ManageRollingDice[nextDice].gameObject.SetActive(true);
       /* GameManager.gm.dice = GameManager.gm.ManageRollingDice[nextDice];*/
 /*       diceTimerCoroutine = StartCoroutine(DiceTimer(GameManager.gm.ManageRollingDice[nextDice]));*/
    }

    int passout(int index)
    {
        if (index == 0)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 1)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 2)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 3)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        return index;
    }



    public void boardSetUP(int number)
    {
        if (number == 0)
        {
            Board.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoPath.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoHome.transform.localEulerAngles = new Vector3(0, 0, -90f);
   /*         OrangeCanvasTemp.transform.localEulerAngles = new Vector3(0, 180, 0);
            RedCanvasTemp.transform.localEulerAngles = new Vector3(0, 180, 0);*/


            for (int i = 0; i < 4; i++)
            {
                redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                bluePlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                greenPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

            var temp = RedRollDiceHome.transform.localPosition;
            var rot = RedRollDiceHome.transform.localEulerAngles;
            GameObject klp = CloneTransformHierarchy(RedRollDiceHome);

            ExchangeProperties(klp.transform, RedRollDiceHome.transform);
            RedRollDiceHome.transform.localPosition = GreenRollDiceHome.transform.localPosition;
            RedRollDiceHome.transform.localEulerAngles = GreenRollDiceHome.transform.localEulerAngles;
            ExchangeProperties(RedRollDiceHome.transform, GreenRollDiceHome.transform);
            GreenRollDiceHome.transform.localPosition = YellowRollDiceHome.transform.localPosition;
            GreenRollDiceHome.transform.localEulerAngles = YellowRollDiceHome.transform.localEulerAngles;
            ExchangeProperties(GreenRollDiceHome.transform, YellowRollDiceHome.transform);
            YellowRollDiceHome.transform.localPosition = BlueRollDiceHome.transform.localPosition;
            YellowRollDiceHome.transform.localEulerAngles = BlueRollDiceHome.transform.localEulerAngles;
            ExchangeProperties(YellowRollDiceHome.transform, BlueRollDiceHome.transform);
            BlueRollDiceHome.transform.localPosition = temp;
            BlueRollDiceHome.transform.localEulerAngles = rot;
            ExchangeProperties(BlueRollDiceHome.transform, klp.transform);
            Destroy(klp.gameObject);



    /*        BlueRollDiceHome.transform.GetChild(0).GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);
            GreenRollDiceHome.transform.GetChild(0).GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);
            RedRollDiceHome.transform.GetChild(0).GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);
            YellowRollDiceHome.transform.GetChild(0).GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);*/
        }
    }


    public static GameObject CloneTransformHierarchy(GameObject original)
    {
        // Create a new GameObject with the same name as the original
        GameObject clone = new GameObject(original.name);

        // Copy the transform properties (position, rotation, scale) from the original to the clone
        clone.transform.position = original.transform.position;
        clone.transform.rotation = original.transform.rotation;
        clone.transform.localScale = original.transform.localScale;

        // Recursively clone the children of the original
        foreach (Transform child in original.transform)
        {
            GameObject childClone = CloneTransformHierarchy(child.gameObject);
            // Set the cloned child as a child of the current clone
            childClone.transform.SetParent(clone.transform);
        }

        return clone;
    }

    public void ExchangeProperties(Transform parent1, Transform parent2)
    {


        for (int i = 0; i < parent1.childCount && i < parent2.childCount; i++)
        {
            Debug.Log(parent1.GetChild(i).name);
            parent1.GetChild(i).localPosition = parent2.GetChild(i).localPosition;
            parent1.GetChild(i).localEulerAngles = parent2.GetChild(i).localEulerAngles;
            ExchangeProperties(parent1.GetChild(i), parent2.GetChild(i));
        }
    }


    public void ReturnHomeScreen()
    {
        Destroy(GameObject.FindGameObjectWithTag("Launcher"));

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("HomeScreen");

    }


    private IEnumerator DiceTimer(RollingDice rd)
    {
        Debug.LogWarning("Pink");
     
        // Get the Timer child with the Image component
       
        Transform timerTransform = rd.transform.parent.GetChild(0).GetChild(0).Find("Timer");
        if (timerTransform == null)
        {
            Debug.LogError("Timer not found!");
            yield break;
        }

        Image timerImage = timerTransform.GetComponent<Image>();

       
        if (timerImage == null)
        {
            Debug.LogError("Image component not found on Timer!");
            yield break;
        }

        // Start the timer
        timerImage.fillAmount = 0f;
        float duration = 20f;
        float elapsedTime = 0f;
        timerImage.fillAmount = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            timerImage.fillAmount = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        timerImage.fillAmount = 1f;
        GameManager.gm.dice = rd;
        photonView.RPC("RestartDiceTimer", RpcTarget.All);

        ShiftDice();
   
    }



    [PunRPC]
    public void RestartDiceTimer()
    {
        if (diceTimerCoroutine != null)
        {
            StopCoroutine(diceTimerCoroutine);
            Transform timerTransform = GameManager.gm.dice.transform.parent.GetChild(0).GetChild(0).Find("Timer");
            Image timerImage = timerTransform.GetComponent<Image>();
            timerImage.fillAmount = 0;
        }
       /* diceTimerCoroutine = StartCoroutine(DiceTimer());*/
    }
}
