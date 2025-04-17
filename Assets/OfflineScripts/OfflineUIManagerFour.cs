using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineUIManagerFour : MonoBehaviour
{
   

    public TMP_InputField RedPlayerName;
    public TMP_InputField BluePlayerName;
    public TMP_InputField GreenPlayerName;
    public TMP_InputField YellowPlayerName;

    public string RedName;
    public string BlueName;
    public string GreenName;
    public string YellowName;




    public void loadNextScene()
    {

        RedName=RedPlayerName.text;
        BlueName=BluePlayerName.text;
        GreenName=GreenPlayerName.text;
        YellowName=YellowPlayerName.text;
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("LudoGame");
    }
}
