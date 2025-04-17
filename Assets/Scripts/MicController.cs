using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class MicController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public GameObject RedMicNot;
    public GameObject YellowMicNot;
    public GameObject BlueMicNot;
    public GameObject GreenMicNot;
    void Start()
    {


       
        if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
        {
          
            GameManager.gm.ManageRollingDice[2].transform.parent.GetComponentInChildren<Toggle>().interactable = true;
        }
        if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[0].UserId)
        {
        
            GameManager.gm.ManageRollingDice[0].transform.parent.GetComponentInChildren<Toggle>().interactable = true;
        }
        if (PhotonNetwork.PlayerList.Length>2 && PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[2].UserId)
        {
        
            GameManager.gm.ManageRollingDice[1].transform.parent.GetComponentInChildren<Toggle>().interactable = true;
        }
        if (PhotonNetwork.PlayerList.Length > 2 &&  PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[3].UserId)
        {
          
            GameManager.gm.ManageRollingDice[3].transform.parent.GetComponentInChildren<Toggle>().interactable = true;
        }
        // Add similar conditions for other pieces if necessary


    }


    public void toggleMicBlue()
    {

        photonView.RPC("ToggleBlue", RpcTarget.All);
    }


    public void toggleMicGreen()
    {
        photonView.RPC("ToggleGreen", RpcTarget.All);

    }


    public void toggleMicRed()
    {

        photonView.RPC("ToggleRed", RpcTarget.All);
    }


    public void toggleMicYellow()
    {
        photonView.RPC("ToggleYellow", RpcTarget.All);

    }





    [PunRPC]
    void ToggleRed()
    {
        if (RedMicNot.activeSelf)
        {
            RedMicNot.SetActive(false);

        }
        else
        {
            RedMicNot.SetActive(true);
        }

    }

    [PunRPC]
    void ToggleYellow()
    {
        if (YellowMicNot.activeSelf)
        {
            YellowMicNot.SetActive(false);

        }
        else
        {
            YellowMicNot.SetActive(true);
        }

    }


    [PunRPC]
    void ToggleGreen()
    {
        if (GreenMicNot.activeSelf)
        {
            GreenMicNot.SetActive(false);

        }
        else
        {
            GreenMicNot.SetActive(true);
        }

    }

    [PunRPC]
    void ToggleBlue()
    {
        if (BlueMicNot.activeSelf)
        {
            BlueMicNot.SetActive(false);

        }
        else
        {
            BlueMicNot.SetActive(true);
        }

    }


    public void toggleMic(Recorder rc)
    {
        if (rc.RecordingEnabled)
        {
            rc.RecordingEnabled = false;

        }
        else
        {
            rc.RecordingEnabled = true;
        }
    }
}
