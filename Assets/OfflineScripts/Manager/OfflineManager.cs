using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineManager : MonoBehaviour
{
    public static OfflineManager om;
    public OfflineDice dice;
    public GameObject OrangeCanvasTemp;
    public int numberOfStepsToMove;
    public bool canPlayerMove = true;
    public bool canDiceRoll = true;
    public bool transferdice = false;
    public bool selfDice = false;
    public GameObject RedRollDiceHome;    // User's dice
    public GameObject YellowRollDiceHome; // AI's dice
    public int redOutPlayers;    // User's pieces out
    public int yellowOutPlayers; // AI's pieces out

    public TMP_Text RedPlayerName;
    public TMP_Text YellowPlayerName;

    public GameObject LudoPath;
    public GameObject LudoHome;

    public int redCompletePlayers;    // User's completed pieces
    public int yellowCompletePlayers; // AI's completed pieces

    public GameObject PlayersUI;
    public GameObject Board;

    public OfflinePlayerPiece[] redPlayerPiece;    // User's pieces
    public OfflinePlayerPiece[] yellowPlayerPiece; // AI's pieces
    public int totalPlayerCanPlay = 2; // Only two players: User and AI

    public OfflineRollingDice[] ManageRollingDice;

    List<OfflinePathPoint> playerOnPathPointList = new List<OfflinePathPoint>();

    public bool isRedPlayerPlaying = true;    // User's turn
    public bool isYellowPlayerPlaying = false; // AI's turn

    private void Awake()
    {
        om = this;

        // Initialize player names
        RedPlayerName.text = "User";
        YellowPlayerName.text = "AI";

        // Set up initial game state
        RedRollDiceHome.SetActive(true);
        YellowRollDiceHome.SetActive(false);

        // Set up dice for 2-player mode (User and AI)
        if (ManageRollingDice != null && ManageRollingDice.Length >= 2)
        {
            ManageRollingDice[0].gameObject.name = "RedDice";
            ManageRollingDice[1].gameObject.name = "YellowDice";

            // Configure AI dice
            var yellowDice = ManageRollingDice[1].GetComponent<OfflineDice>();
            if (yellowDice != null)
            {
                yellowDice.isAIPlayer = true;
            }
        }

        // Start with user's turn
        isRedPlayerPlaying = true;
        isYellowPlayerPlaying = false;
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

    public void AddPathPoint(OfflinePathPoint pathPoint)
    {
        playerOnPathPointList.Add(pathPoint);
    }

    public void RemovePathPoint(OfflinePathPoint pathPoint)
    {
        if (playerOnPathPointList.Contains(pathPoint))
        {
            playerOnPathPointList.Remove(pathPoint);
        }
    }

    public void RollingDiceManager()
    {
        if (transferdice)
        {
            if (numberOfStepsToMove != 6)
            {
                SwitchTurn();
            }
            canDiceRoll = true;
        }
        else if (selfDice)
        {
            selfDice = false;
            canDiceRoll = true;
            SelfRoll();
        }
    }

    public void SelfRoll()
    {
        if (isYellowPlayerPlaying) // AI's turn
        {
            Invoke("AITurn", 1f);
        }
    }

    void AITurn()
    {
        if (!canDiceRoll) return;
        ManageRollingDice[1].mouseRoll();
        StartCoroutine(AIMove());
    }

    IEnumerator AIMove()
    {
        yield return new WaitForSeconds(1f);
      
        if (numberOfStepsToMove == 6 && yellowOutPlayers < 4)
        {
            int pieceToMove = GetFirstInHomePiece();
            if (pieceToMove != -1)
            {
                ManageRollingDice[1].MakePlayerReadyToMove(pieceToMove);
                yield break;
            }
        }

        int bestPieceToMove = GetBestPieceToMove();
        if (bestPieceToMove != -1)
        {
            ManageRollingDice[1].StartCoroutine(ManageRollingDice[1].MovePlayer(bestPieceToMove));
        }
        else
        {
            SwitchTurn();
        }
    }

    int GetFirstInHomePiece()
    {
        for (int i = 0; i < yellowPlayerPiece.Length; i++)
        {
            if (!yellowPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return -1;
    }

    int GetBestPieceToMove()
    {
        for (int i = 0; i < yellowPlayerPiece.Length; i++)
        {
            if (yellowPlayerPiece[i].isReady && CanPieceMove(yellowPlayerPiece[i]))
            {
                return i;
            }
        }
        return -1;
    }

    bool CanPieceMove(OfflinePlayerPiece piece)
    {
        int remainingSteps = 57 - piece.numberOfStepsAlreadyMove;
        return remainingSteps >= numberOfStepsToMove;
    }

    void SwitchTurn()
    {
        isRedPlayerPlaying = !isRedPlayerPlaying;
        isYellowPlayerPlaying = !isYellowPlayerPlaying;

        RedRollDiceHome.SetActive(isRedPlayerPlaying);
        YellowRollDiceHome.SetActive(isYellowPlayerPlaying);

        if (isYellowPlayerPlaying)
        {
            AITurn();
        }

        canDiceRoll = true;
        transferdice = false;
    }

    public void boardSetUP(int number)
    {
        if (number == 0)
        {
            // Rotate board components
            Board.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoPath.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoHome.transform.localEulerAngles = new Vector3(0, 0, -90f);
            OrangeCanvasTemp.transform.localEulerAngles = new Vector3(0, 180, 0);

            // Rotate player pieces
            for (int i = 0; i < 4; i++)
            {
                redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                yellowPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

            // Store Red dice home position and rotation
            var temp = RedRollDiceHome.transform.localPosition;
            var rot = RedRollDiceHome.transform.localEulerAngles;

            // Clone for temporary storage
            GameObject tempClone = CloneTransformHierarchy(RedRollDiceHome);

            // Rotate dice positions
            ExchangeProperties(tempClone.transform, RedRollDiceHome.transform);
            RedRollDiceHome.transform.localPosition = YellowRollDiceHome.transform.localPosition;
            RedRollDiceHome.transform.localEulerAngles = YellowRollDiceHome.transform.localEulerAngles;

            ExchangeProperties(RedRollDiceHome.transform, YellowRollDiceHome.transform);
            YellowRollDiceHome.transform.localPosition = temp;
            YellowRollDiceHome.transform.localEulerAngles = rot;

            ExchangeProperties(YellowRollDiceHome.transform, tempClone.transform);

            // Clean up
            Destroy(tempClone);
        }
    }

    public static GameObject CloneTransformHierarchy(GameObject original)
    {
        GameObject clone = new GameObject(original.name);

        // Copy transform properties
        clone.transform.position = original.transform.position;
        clone.transform.rotation = original.transform.rotation;
        clone.transform.localScale = original.transform.localScale;

        // Clone children recursively
        foreach (Transform child in original.transform)
        {
            GameObject childClone = CloneTransformHierarchy(child.gameObject);
            childClone.transform.SetParent(clone.transform);
        }

        return clone;
    }

    public void ExchangeProperties(Transform parent1, Transform parent2)
    {
        for (int i = 0; i < parent1.childCount && i < parent2.childCount; i++)
        {
            parent1.GetChild(i).position = parent2.GetChild(i).position;
            parent1.GetChild(i).localEulerAngles = parent2.GetChild(i).localEulerAngles;
        }
    }

    public void CheckGameOver()
    {
        if (redCompletePlayers == 4)
        {
            ShowGameOver("User Wins!");
        }
        else if (yellowCompletePlayers == 4)
        {
            ShowGameOver("AI Wins!");
        }
    }

    void ShowGameOver(string message)
    {
        Debug.Log(message);
    }

    public void ReturnHomeScreen()
    {
        Destroy(GameObject.FindGameObjectWithTag("UIManager"));
        SceneManager.LoadScene("HomeScreen");
    }
}