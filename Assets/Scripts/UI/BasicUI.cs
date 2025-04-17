using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using TMPro;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    public GameObject menuOption;
    public GameObject inputfield;
    bool toggle = false;


    public GameObject launcher2POnline;
    public GameObject launcher4Online;

    public GameObject P2Screen;
    public GameObject P4Screen;

    public int i;

    public static BasicUI instance;

    public TMP_Text username;
    public TMP_Text money;
    public GameObject loader;

    public GameObject messageButton;
    public GameObject sendButton;
    public GameObject canvas;
    bool isSound = false;
    private void Awake()
    {
        instance = this;
    }

    public void AsToggle()
    {
        toggle = !toggle;
        if (toggle)
        {
            menuOption.SetActive(true);
    /*        inputfield.SetActive(false);*/

        }
        else
        {
          /*  inputfield.SetActive(true);*/
            menuOption.SetActive(false);
        }
    }


    public void ChangeNUmber(int j)
    {
        i = j;
    }

    public void MessageToggle()
    {
        sendButton.gameObject.SetActive(true);
        messageButton.gameObject.SetActive(false);
        canvas.gameObject.SetActive(true);

    }
    public void SendToggle()
    {
        messageButton.gameObject.SetActive(true);
        sendButton.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }


    public void chooseNumberOfPlayer()
    {
        this.gameObject.SetActive(false);
        if (i == 2)
        {

            launcher2POnline.SetActive(true);
            P2Screen.SetActive(true);
            P2Screen.transform.GetComponentInChildren<MagnifierAnimation>().StartMyAnim();
            launcher2POnline.GetComponent<Launcher>().StartMyGame();


        }
        else if (i == 4)
        {
            launcher4Online.SetActive(true);
            P4Screen.SetActive(true);
            launcher4Online.GetComponent<Launcher4Player>().StartMyGame();
        }
    }


    public void setUsername()
    {
        username.text = DBManager.username;
        money.text = DBManager.TotalBalance.ToString();
    }

    public void hideLoader()
    {
        loader.SetActive(false);
    }

    public void showLoader()
    {
        loader.SetActive(true);
    }


    public void SoundOff()
    {
        foreach(var op in GameObject.FindObjectsOfType<AudioSource>())
        {
            op.enabled = false;
        }
    }

    public void SoundOn()
    {
        foreach (var op in GameObject.FindObjectsOfType<AudioSource>())
        {
            op.enabled = true;
        }
    }


/*    public void soundMenu()
    {
        if (isSound)
        {
            SoundOn();
        }
        else
        {
            SoundOff();
        }
        isSound = !isSound;
    }*/
}
