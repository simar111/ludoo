using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoAI : MonoBehaviour
{
    public int[] tokens = new int[4] { -1, -1, -1, -1 }; 
    public bool[] isSafeZone = new bool[52]; 
    public bool[] isOccupiedByOpponent = new bool[52];

    private int diceValue = 0; 
    private bool extraTurn = false; 
    private System.Random random = new System.Random();

    void Start()
    {
        InitializeSafeZones();
    }

    void InitializeSafeZones()
    {
        int[] safePositions = { 0, 8, 13, 21, 26, 34, 39, 47 }; 
        foreach (int pos in safePositions)
        {
            isSafeZone[pos] = true;
        }
    }

   
    public void AITurn()
    {
        RollDice();

        List<int> movableTokens = GetMovableTokens();

        if (movableTokens.Count == 0)
        {
            EndTurn();
            return;
        }

        int selectedToken = SelectBestMove(movableTokens);
        MoveToken(selectedToken);

        if (extraTurn)
        {
            extraTurn = false;
            AITurn(); 
        }
        else
        {
            EndTurn();
        }
    }


    void RollDice()
    {
        diceValue = random.Next(1, 7);
        if (diceValue == 6)
        {
            extraTurn = true;
        }
    }

    
    List<int> GetMovableTokens()
    {
        List<int> movableTokens = new List<int>();

        for (int i = 0; i < tokens.Length; i++)
        {
            int currentPosition = tokens[i];

            
            if (currentPosition == -1 && diceValue == 6)
            {
                movableTokens.Add(i);
            }
           
            else if (currentPosition != -1 && currentPosition + diceValue < 52)
            {
                movableTokens.Add(i);
            }
        }

        return movableTokens;
    }

   
    int SelectBestMove(List<int> movableTokens)
    {
        
        foreach (int i in movableTokens)
        {
            if (tokens[i] == -1) return i;
        }

     
        foreach (int i in movableTokens)
        {
            int targetPosition = tokens[i] + diceValue;
            if (isOccupiedByOpponent[targetPosition] && !isSafeZone[targetPosition])
            {
                return i;
            }
        }

       
        foreach (int i in movableTokens)
        {
            int targetPosition = tokens[i] + diceValue;
            if (isSafeZone[targetPosition])
            {
                return i;
            }
        }

       
        return movableTokens[random.Next(0, movableTokens.Count)];
    }

   
    void MoveToken(int tokenIndex)
    {
        if (tokens[tokenIndex] == -1) 
        {
            tokens[tokenIndex] = 0; 
        }
        else
        {
            tokens[tokenIndex] += diceValue; 
        }

        CheckForCapture(tokens[tokenIndex]);
        CheckForWin(tokens[tokenIndex]);
    }

 
    void CheckForCapture(int newPosition)
    {
        if (isOccupiedByOpponent[newPosition] && !isSafeZone[newPosition])
        {
            Debug.Log("AI captured an opponent's token at " + newPosition);
            
        }
    }

    // Checks if a token reached the final home position
    void CheckForWin(int newPosition)
    {
        if (newPosition >= 52)
        {
            Debug.Log("AI token reached home!");
         
        }
    }

    
    void EndTurn()
    {
        Debug.Log("AI turn ended.");
        
    }
}
