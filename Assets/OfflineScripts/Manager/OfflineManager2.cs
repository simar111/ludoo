using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineManager2 : MonoBehaviour
{
    public static OfflineManager2 om;
    public OfflineRollingDice1 dice;
    public int numberOfStepsToMove;
    public bool canPlayerMove = true;
    public bool canDiceRoll = true;
    public bool transferdice=false;
    public bool selfDice = false;
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
    //public OfflinePlayerPiece[] bluePlayerPiece;
    public OfflinePlayerPiece1[] redPlayerPiece;
    //public OfflinePlayerPiece[] greenPlayerPiece;
    public OfflinePlayerPiece1[] yelloPlayerPiece;
    public int totalPlayerCanPlay;
    public OfflineRollingDice1[] ManageRollingDice;
    OfflineUIManagerTwo offlineUIManagerTwo;
    OfflineUIManagerFour offlineUIManagerFour;
    OfflineUIManagerThree offlineUIManagerThree;
    List<OfflinePathPoint1> playerOnPathPointList = new List<OfflinePathPoint1>();
    public bool isRedPlayerPlaying = false;
    public bool isGreenPlayerPlaying = false;
    public bool isBluePlayerPlaying = false;
    public bool isYellowPlayerPlaying = false;
    public byte PlayerRemainingToPlay=4;
    public GameObject LudoHome;
    private void Awake()
    {
        om = this;
        offlineUIManagerTwo =GameObject.FindGameObjectWithTag("UIManager").GetComponent<OfflineUIManagerTwo>();
        offlineUIManagerFour = GameObject.FindGameObjectWithTag("UIManager").GetComponent<OfflineUIManagerFour>();
        offlineUIManagerThree = GameObject.FindGameObjectWithTag("UIManager").GetComponent<OfflineUIManagerThree>();
        if (offlineUIManagerTwo.enabled)
        {
            //RedPlayerName.text = offlineUIManagerTwo.RedName;
            //BluePlayerName.text = offlineUIManagerTwo.BlueName;
            //GreenPlayerName.text = offlineUIManagerTwo.GreenName;
            //YellowPlayerName.text = offlineUIManagerTwo.YellowName;

            if (offlineUIManagerTwo.gameMode == 1)
            {
                OfflineManager2.om.totalPlayerCanPlay = 1;
                RedRollDiceHome.SetActive(true);
                YellowRollDiceHome.SetActive(true);

                // Hide Red and Yellow player pieces
                //HidePlayers(OfflineManager2.om.redPlayerPiece);
                //HidePlayers(OfflineManager2.om.yelloPlayerPiece);

                // Set up the board for mode 1
                boardSetUP(1);

                //// Set total players to 2
                //OfflineManager2.om.totalPlayerCanPlay = 2;
                // Enable Red and Yellow players
                isRedPlayerPlaying = true;
                isYellowPlayerPlaying = true;

                // Disable dice for Red and Yellow players
                OfflineManager2.om.ManageRollingDice[0].isAllowed = true; // Red player dice
                OfflineManager2.om.ManageRollingDice[1].isAllowed = true; // Yellow player dice
            }
            else
                if (offlineUIManagerTwo.gameMode == 0)
            {
                // Disable the first dice and enable the second dice
                OfflineManager2.om.ManageRollingDice[0].gameObject.SetActive(false);
                OfflineManager2.om.ManageRollingDice[1].gameObject.SetActive(true);

                // Disable Red and Yellow dice homes

                RedRollDiceHome.SetActive(false);
                YellowRollDiceHome.SetActive(false);

                // Hide Red and Yellow player pieces
                HidePlayers(OfflineManager2.om.redPlayerPiece);
                HidePlayers(OfflineManager2.om.yelloPlayerPiece);

                // Set total players to 2
                OfflineManager2.om.totalPlayerCanPlay = 2;

                // Set up the board for mode 0
                boardSetUP(0);

                // Enable Red and Yellow players
                isRedPlayerPlaying = true;
                isYellowPlayerPlaying = true;

                // Disable dice for Red and Yellow players
                OfflineManager2.om.ManageRollingDice[0].isAllowed = false; // Red player dice
                OfflineManager2.om.ManageRollingDice[1].isAllowed = false; // Yellow player dice
            }
            else
            {
                // Disable Red and Yellow dice homes
                RedRollDiceHome.SetActive(true);
                YellowRollDiceHome.SetActive(true);

                // Hide Red and Yellow player pieces
                //HidePlayers(OfflineManager2.om.redPlayerPiece);
                //HidePlayers(OfflineManager2.om.yelloPlayerPiece);

                // Set up the board for mode 1
                boardSetUP(1);

                // Set total players to 2
                OfflineManager2.om.totalPlayerCanPlay = 2;

                // Enable Red and Yellow players
                isRedPlayerPlaying = true;
                isYellowPlayerPlaying = true;

                // Disable dice for Red and Yellow players
                OfflineManager2.om.ManageRollingDice[0].isAllowed = true; // Red player dice
                OfflineManager2.om.ManageRollingDice[1].isAllowed = true; // Yellow player dice
            }
            PlayerRemainingToPlay = 2;
        }
        else if (offlineUIManagerFour.enabled)
        {
            OfflineManager2.om.totalPlayerCanPlay = 4;
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
            OfflineManager2.om.totalPlayerCanPlay = 3;
            if (offlineUIManagerThree.RedName == "PLON")
            {
                HidePlayers(OfflineManager2.om.redPlayerPiece);
                RedRollDiceHome.SetActive(false);
                boardSetUP(0);
                OfflineManager2.om.ManageRollingDice[0].isAllowed = false;
                OfflineManager2.om.ManageRollingDice[1].gameObject.SetActive(true);
                OfflineManager2.om.ManageRollingDice[0].gameObject.SetActive(false);
                isBluePlayerPlaying = true;
                isGreenPlayerPlaying = true;
                isYellowPlayerPlaying = true;
            }
            //else if (offlineUIManagerThree.BlueName == "PLON")
            //{
            //    HidePlayers(OfflineManager2.om.bluePlayerPiece);
            //    BlueRollDiceHome.SetActive(false);
            //    OfflineManager2.om.ManageRollingDice[1].isAllowed = false;
            //    isGreenPlayerPlaying = true;
            //    isRedPlayerPlaying = true;
            //    isYellowPlayerPlaying = true;
            //}
            //else if (offlineUIManagerThree.GreenName == "PLON")
            //{
            //    HidePlayers(GameManagerOffline.gm.greenPlayerPiece);
            //    GreenRollDiceHome.SetActive(false);
            //    GameManagerOffline.gm.ManageRollingDice[3].isAllowed = false;
            //    isBluePlayerPlaying = true;
            //    isRedPlayerPlaying = true;
            //    isYellowPlayerPlaying = true;
            //}
            else if (offlineUIManagerThree.YellowName == "PLON")
            {
                HidePlayers(OfflineManager2.om.yelloPlayerPiece);
                YellowRollDiceHome.SetActive(false);
                OfflineManager2.om.ManageRollingDice[1].isAllowed = false;
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
    void HidePlayers(OfflinePlayerPiece1[] playerPieces)
    {
        for (int i = 0; i < playerPieces.Length; i++)
        {
            playerPieces[i].gameObject.SetActive(false);
        }
    }
    public void AddPathPoint(OfflinePathPoint1 OfflinePathPoint)
    {
        playerOnPathPointList.Add(OfflinePathPoint);
    }
    public void RemovePathPoint(OfflinePathPoint1 OfflinePathPoint)
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
       
        if (OfflineManager2.om.transferdice)
        {
            if (OfflineManager2.om.numberOfStepsToMove != 6)
            {
                ShiftDice();
            }

            OfflineManager2.om.canDiceRoll = true;
        }
        else
        {
            if ( OfflineManager2.om.selfDice)
            {
                OfflineManager2.om.selfDice = false;
                OfflineManager2.om.canDiceRoll = true;
                OfflineManager2.om.SelfRoll();
            }
        }
    }
    public void SelfRoll()
    {
        if (OfflineManager2.om.totalPlayerCanPlay == 1 && OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[1])
        {
            Invoke("roled", 0.6f);
        }
    }
    void roled()
    {
       OfflineManager2.om.ManageRollingDice[1].mouseRoll();
    }
    void ShiftDice()
    {
        if (OfflineManager2.om == null)
        {
            Debug.LogError("OfflineManager2.om is null.");
            return;
        }
        // Check if ManageRollingDice is null or empty
        if (OfflineManager2.om.ManageRollingDice == null || OfflineManager2.om.ManageRollingDice.Length < 2)
        {
            Debug.LogError("ManageRollingDice is null or does not have enough elements.");
            return;
        }
        if (OfflineManager2.om.totalPlayerCanPlay == 1)
        {
            // Logic for single player (not applicable for two players)
            if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[0])
            {
                OfflineManager2.om.ManageRollingDice[0].gameObject.SetActive(false);
                OfflineManager2.om.ManageRollingDice[1].gameObject.SetActive(true); // Switch to Yellow dice
                passout(0); // Handle passout logic for Red player
                OfflineManager2.om.ManageRollingDice[1].mouseRoll(); // Allow Yellow player to roll
            }
            else
            {
                OfflineManager2.om.ManageRollingDice[1].gameObject.SetActive(false);
                OfflineManager2.om.ManageRollingDice[0].gameObject.SetActive(true); // Switch back to Red dice
                passout(1); // Handle passout logic for Yellow player
            }
        }
        else if (OfflineManager2.om.totalPlayerCanPlay == 2)
        {
            // Logic for two players (Red and Yellow)
            int currentDiceIndex = 0;
            int nextDiceIndex = 0;

            // Find the current dice index (0 for Red, 1 for Yellow)
            for (int i = 0; i < 2; i++)
            {
                if (OfflineManager2.om.dice == OfflineManager2.om.ManageRollingDice[i])
                {
                    currentDiceIndex = i;
                    break;
                }
            }

            // Calculate the next dice index
            if (currentDiceIndex == 1) // If current dice is Yellow, switch to Red
            {
                nextDiceIndex = 0;
            }
            else // If current dice is Red, switch to Yellow
            {
                nextDiceIndex = 1;
            }

            // Ensure the next dice is allowed to play
            while (!OfflineManager2.om.ManageRollingDice[nextDiceIndex].isAllowed)
            {
                nextDiceIndex++;
                if (nextDiceIndex > 1) // Only two players (0 and 1)
                {
                    nextDiceIndex = 0;
                }
            }

            // Switch to the next dice
            OfflineManager2.om.ManageRollingDice[currentDiceIndex].gameObject.SetActive(false);
            OfflineManager2.om.ManageRollingDice[nextDiceIndex].gameObject.SetActive(true);
        }
    }

    int passout(int index)
    {
        if (index == 0)
        {
            if (OfflineManager2.om.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 1)
        {
            if (OfflineManager2.om.blueCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 2)
        {
            if (OfflineManager2.om.yellowCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 3)
        {
            if ( OfflineManager2.om.greenCompletePlayers == 4)
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
                //bluePlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                //greenPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
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
