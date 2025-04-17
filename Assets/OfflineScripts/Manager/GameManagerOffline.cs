using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerOffline : MonoBehaviour
{
    public static GameManagerOffline gm;
    public OfflineRollingDice dice;

    public int numberOfStepsToMove;
    public bool canPlayerMove = true;
    public bool canDiceRoll = true;
    public bool transferdice=false;
    public bool selfDice=false;

    public int redOutPlayers;
    public int greenOutPlayers;
    public int blueOutPlayers;
    public int yellowOutPlayers;


    public TMP_Text RedPlayerName;
    public TMP_Text BluePlayerName;
    public TMP_Text YellowPlayerName;
    public TMP_Text GreenPlayerName;

    public GameObject LudoPath;


    public int redCompletePlayers;
    public int greenCompletePlayers;
    public int blueCompletePlayers;
    public int yellowCompletePlayers;

    public GameObject RedRollDiceHome;
    public GameObject BlueRollDiceHome;
    public GameObject YellowRollDiceHome;
    public GameObject GreenRollDiceHome;


    public GameObject PlayersUI;
    public GameObject Board;

    public GameObject OrangeCanvasTemp;


    public OfflinePlayerPiece[] bluePlayerPiece;
    public OfflinePlayerPiece[] redPlayerPiece;
    public OfflinePlayerPiece[] greenPlayerPiece;
    public OfflinePlayerPiece[] yelloPlayerPiece;

    public int totalPlayerCanPlay;

    public OfflineRollingDice[] ManageRollingDice;

    OfflineUIManagerTwo offlineUIManagerTwo;
    OfflineUIManagerFour offlineUIManagerFour;
    OfflineUIManagerThree offlineUIManagerThree;
    List<OfflinePathPoint> playerOnPathPointList = new List<OfflinePathPoint>();


    public bool isRedPlayerPlaying = false;
    public bool isGreenPlayerPlaying = false;
    public bool isBluePlayerPlaying = false;
    public bool isYellowPlayerPlaying = false;

    public byte PlayerRemainingToPlay=4;

    public GameObject LudoHome;
    private void Awake()
    {
        gm = this;

    

        offlineUIManagerTwo =GameObject.FindGameObjectWithTag("UIManager").GetComponent<OfflineUIManagerTwo>();
        offlineUIManagerFour = GameObject.FindGameObjectWithTag("UIManager").GetComponent<OfflineUIManagerFour>();
        offlineUIManagerThree = GameObject.FindGameObjectWithTag("UIManager").GetComponent<OfflineUIManagerThree>();


        if (offlineUIManagerTwo.enabled)
        {
            RedPlayerName.text = offlineUIManagerTwo.RedName;
            BluePlayerName.text = offlineUIManagerTwo.BlueName;
            GreenPlayerName.text = offlineUIManagerTwo.GreenName;
            YellowPlayerName.text = offlineUIManagerTwo.YellowName;
            if (offlineUIManagerTwo.gameMode == 0)
            {
                GameManagerOffline.gm.ManageRollingDice[0].gameObject.SetActive(false);
                GameManagerOffline.gm.ManageRollingDice[1].gameObject.SetActive(true);
                RedRollDiceHome.SetActive(false);
                YellowRollDiceHome.SetActive(false);
                HidePlayers(GameManagerOffline.gm.redPlayerPiece);
                HidePlayers(GameManagerOffline.gm.yelloPlayerPiece);
                GameManagerOffline.gm.totalPlayerCanPlay = 2;
                boardSetUP(0);
                isBluePlayerPlaying = true;
                isGreenPlayerPlaying = true;
                GameManagerOffline.gm.ManageRollingDice[0].isAllowed = false;
                GameManagerOffline.gm.ManageRollingDice[2].isAllowed = false;
            }
            else
            {
                BlueRollDiceHome.SetActive(false);
                GreenRollDiceHome.SetActive(false);
                HidePlayers(GameManagerOffline.gm.bluePlayerPiece);
                HidePlayers(GameManagerOffline.gm.greenPlayerPiece);
                boardSetUP(1);
                GameManagerOffline.gm.totalPlayerCanPlay = 2;
                isRedPlayerPlaying = true;
                isYellowPlayerPlaying = true;
                GameManagerOffline.gm.ManageRollingDice[1].isAllowed = false;
                GameManagerOffline.gm.ManageRollingDice[3].isAllowed = false;
            }
            PlayerRemainingToPlay = 2;
        }
        else if (offlineUIManagerFour.enabled)
        {
            GameManagerOffline.gm.totalPlayerCanPlay = 4;
            RedPlayerName.text = offlineUIManagerFour.RedName;
            BluePlayerName.text = offlineUIManagerFour.BlueName;
            GreenPlayerName.text = offlineUIManagerFour.GreenName;
            YellowPlayerName.text = offlineUIManagerFour.YellowName;
            isBluePlayerPlaying = true;
            isGreenPlayerPlaying = true;
            isRedPlayerPlaying = true;
            isYellowPlayerPlaying = true;
            PlayerRemainingToPlay = 4;
        }
        else if(offlineUIManagerThree.enabled)
        {
            PlayerRemainingToPlay = 3;
            GameManagerOffline.gm.totalPlayerCanPlay = 3;
            if (offlineUIManagerThree.RedName == "PLON")
            {
                HidePlayers(GameManagerOffline.gm.redPlayerPiece);
                RedRollDiceHome.SetActive(false);
                boardSetUP(0);
                GameManagerOffline.gm.ManageRollingDice[0].isAllowed = false;
                GameManagerOffline.gm.ManageRollingDice[1].gameObject.SetActive(true);
                GameManagerOffline.gm.ManageRollingDice[0].gameObject.SetActive(false);
                isBluePlayerPlaying = true;
                isGreenPlayerPlaying = true;
                isYellowPlayerPlaying = true;
            }
            else if (offlineUIManagerThree.BlueName == "PLON")
            {
                HidePlayers(GameManagerOffline.gm.bluePlayerPiece);
                BlueRollDiceHome.SetActive(false);
                GameManagerOffline.gm.ManageRollingDice[1].isAllowed = false;
                isGreenPlayerPlaying = true;
                isRedPlayerPlaying = true;
                isYellowPlayerPlaying = true;
            }
            else if (offlineUIManagerThree.GreenName == "PLON")
            {
                HidePlayers(GameManagerOffline.gm.greenPlayerPiece);
                GreenRollDiceHome.SetActive(false);
                GameManagerOffline.gm.ManageRollingDice[3].isAllowed = false;
                isBluePlayerPlaying = true;
                isRedPlayerPlaying = true;
                isYellowPlayerPlaying = true;
            }
            else if (offlineUIManagerThree.YellowName == "PLON")
            {
                HidePlayers(GameManagerOffline.gm.yelloPlayerPiece);
                YellowRollDiceHome.SetActive(false);
                GameManagerOffline.gm.ManageRollingDice[2].isAllowed = false;
                isBluePlayerPlaying = true;
                isGreenPlayerPlaying = true;
                isRedPlayerPlaying = true;
            }
            RedPlayerName.text = offlineUIManagerThree.RedName;
            BluePlayerName.text = offlineUIManagerThree.BlueName;
            GreenPlayerName.text = offlineUIManagerThree.GreenName;
            YellowPlayerName.text = offlineUIManagerThree.YellowName;
        }
    }


    public int BasePointPosition(string name)
    {

        for (int i = 0; i < LudoPath.GetComponent<OfflinePathObjectParent>().BasePathPoint.Length; i++)
        {
            if (LudoPath.GetComponent<OfflinePathObjectParent>().BasePathPoint[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    void HidePlayers(OfflinePlayerPiece[] playerPieces)
    {
        for (int i = 0; i < playerPieces.Length; i++)
        {
            playerPieces[i].gameObject.SetActive(false);
        }
    }

    public void AddPathPoint(OfflinePathPoint OfflinePathPoint)
    {
        playerOnPathPointList.Add(OfflinePathPoint);
    }

    public void RemovePathPoint(OfflinePathPoint OfflinePathPoint)
    {
        if (playerOnPathPointList.Contains(OfflinePathPoint))
        {
            playerOnPathPointList.Remove(OfflinePathPoint);
        }
        else
        {
            Debug.Log("Path Not found to be romved");
        }
    }


    public void RollingDiceManager()
    {
       
        if (GameManagerOffline.gm.transferdice)
        {
            if (GameManagerOffline.gm.numberOfStepsToMove != 6)
            {
                ShiftDice();
            }
          
            GameManagerOffline.gm.canDiceRoll = true;
        }
        else
        {
            if (GameManagerOffline.gm.selfDice)
            {
                GameManagerOffline.gm.selfDice = false;
                GameManagerOffline.gm.canDiceRoll = true;
                GameManagerOffline.gm.SelfRoll();
            }
        }
    }

    public void SelfRoll()
    {
        if (GameManagerOffline.gm.totalPlayerCanPlay == 1 && GameManagerOffline.gm.dice== GameManagerOffline.gm.ManageRollingDice[2])
        {
            Invoke("roled", 0.6f);
        }
    }

    void roled()
    {
        GameManagerOffline.gm.ManageRollingDice[2].mouseRoll();
    }

    void ShiftDice()
    {
        int nextDice;
        if (GameManagerOffline.gm.totalPlayerCanPlay == 1)
        {
            if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
            {
                GameManagerOffline.gm.ManageRollingDice[0].gameObject.SetActive(false);
                GameManagerOffline.gm.ManageRollingDice[2].gameObject.SetActive(true);
                passout(0);
                GameManagerOffline.gm.ManageRollingDice[2].mouseRoll();
            }
            else
            {

                GameManagerOffline.gm.ManageRollingDice[0].gameObject.SetActive(true);
                GameManagerOffline.gm.ManageRollingDice[2].gameObject.SetActive(false);
                passout(2);

            }
        }
        /*else if (GameManagerOffline.gm.totalPlayerCanPlay == 2)
        {

            if (offlineUIManagerTwo.gameMode == 0)
            {
                if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[1])
                {
                    GameManagerOffline.gm.ManageRollingDice[1].gameObject.SetActive(false);
                    GameManagerOffline.gm.ManageRollingDice[3].gameObject.SetActive(true);
                    passout(0);
                }
                else
                {

                    GameManagerOffline.gm.ManageRollingDice[1].gameObject.SetActive(true);
                    GameManagerOffline.gm.ManageRollingDice[3].gameObject.SetActive(false);
                    passout(2);

                }
            }
            else
            {


                if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[0])
                {
                    GameManagerOffline.gm.ManageRollingDice[0].gameObject.SetActive(false);
                    GameManagerOffline.gm.ManageRollingDice[2].gameObject.SetActive(true);
                    passout(0);
                }
                else
                {

                    GameManagerOffline.gm.ManageRollingDice[0].gameObject.SetActive(true);
                    GameManagerOffline.gm.ManageRollingDice[2].gameObject.SetActive(false);
                    passout(2);

                }

            }
            
        }
        else if (GameManagerOffline.gm.totalPlayerCanPlay == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                
                if (i == 3)
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }
                if(i==3 && offlineUIManagerThree.RedName == "PLON")
                {
                    nextDice = 1;
                }
               if(!GameManagerOffline.gm.ManageRollingDice[i].isAllowed && offlineUIManagerThree.RedName!="PLON")
                {
                    GameManagerOffline.gm.ManageRollingDice[i].gameObject.SetActive(false);
                    GameManagerOffline.gm.ManageRollingDice[nextDice].gameObject.SetActive(true);
                    return;
                }
                i = passout(i);
                if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[i])
                {

                    GameManagerOffline.gm.ManageRollingDice[i].gameObject.SetActive(false);
                    GameManagerOffline.gm.ManageRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }*/
        else if (GameManagerOffline.gm.totalPlayerCanPlay > 1)
        {
            int currentDiceIndex=0;
            int nextDiceIndex = 0;
            for(int i=0;i<4;i++)
            {
                if(GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[i])
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


            while (!GameManagerOffline.gm.ManageRollingDice[nextDiceIndex].isAllowed)
            {
            
                nextDiceIndex++;
                if (nextDiceIndex > 3)
                {
                    nextDiceIndex = 0;
                }
            }


                GameManagerOffline.gm.ManageRollingDice[currentDiceIndex].gameObject.SetActive(false);
            GameManagerOffline.gm.ManageRollingDice[nextDiceIndex].gameObject.SetActive(true);


            /*for (int i = 0; i < 4; i++)
            {
               
                *//*i = passout(i);
*//*
                // Check if the current dice is allowed to play
                if (!GameManagerOffline.gm.ManageRollingDice[i].isAllowed)
                {
                    Debug.LogWarning(GameManagerOffline.gm.ManageRollingDice[i].name);
                }

                if (GameManagerOffline.gm.dice == GameManagerOffline.gm.ManageRollingDice[i])
                {
                    
                }
            }*/
        }

    }

    int passout(int index)
    {
        if (index == 0)
        {
            if (GameManagerOffline.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 1)
        {
            if (GameManagerOffline.gm.blueCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 2)
        {
            if (GameManagerOffline.gm.yellowCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 3)
        {
            if (GameManagerOffline.gm.greenCompletePlayers == 4)
            {
                return 0;
            }
        }
        return index;
    }



    public void boardSetUP(int number)
    {
        if(number==0)
        {
           



            Board.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoPath.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoHome.transform.localEulerAngles = new Vector3(0, 0, -90f);
            OrangeCanvasTemp.transform.localEulerAngles = new Vector3(0, 180, 0);

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


        for (int i = 0; i < parent1.childCount && i< parent2.childCount; i++)
        {
            Debug.Log(parent1.GetChild(i).name);
            parent1.GetChild(i).position = parent2.GetChild(i).position;
            parent1.GetChild(i).localEulerAngles = parent2.GetChild(i).localEulerAngles;

        }
    }


    public void ReturnHomeScreen()
    {
        Destroy(GameObject.FindGameObjectWithTag("UIManager"));
        SceneManager.LoadScene("HomeScreen");

    }

}
