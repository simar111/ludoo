using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPieces : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Blue"))
        {
            GameManager.gm.blueOutPlayers -= 1;
        }
        else if (collision.tag.Contains("Red"))
        {
            GameManager.gm.redOutPlayers -= 1;

        }
        else if (collision.tag.Contains("Yellow"))
        {
            GameManager.gm.yellowOutPlayers -= 1;

        }
        else if (collision.tag.Contains("Green"))
        {
            GameManager.gm.greenOutPlayers -= 1;

        }
        collision.GetComponent<PlayerPiece>().isReady = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<PlayerPiece>().isReady = true;
    }
}
