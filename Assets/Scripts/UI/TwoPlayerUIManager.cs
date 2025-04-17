using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayerUIManager : MonoBehaviour
{
   public Toggle BlueGreenToggle;
    public Toggle RedYellowToggle;

    public GameObject BlueGreenDim;
    public GameObject RedYellowDim;


    public TMP_InputField BluePlayerName;
    public TMP_InputField GreenPlayername;
    public TMP_InputField RedPlayerName;
    public TMP_InputField YellowPlayerName;
    public void twoPlayerToggleSelect()
    {
        if(BlueGreenToggle.isOn)
        {
            RedYellowDim.gameObject.SetActive(true);
            BlueGreenDim.gameObject.SetActive(false);
            RedPlayerName.interactable = false;
            YellowPlayerName.interactable = false;
            RedPlayerName.text = "Player 1";
            YellowPlayerName.text = "Player 2";
            BluePlayerName.interactable = true;
            GreenPlayername.interactable = true;
        }
        else
        {
            RedYellowDim.gameObject.SetActive(false);
            BlueGreenDim.gameObject.SetActive(true);
            RedPlayerName.interactable = true;
            YellowPlayerName.interactable = true;
            BluePlayerName.interactable = false;
            GreenPlayername.interactable = false;
            BluePlayerName.text = "Player 1";
            GreenPlayername.text = "Player 2";
        }
    }
}
