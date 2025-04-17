using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BlueHome : LudoHome
{
 

    [PunRPC]
    void PlayerSound(string str)
    {
        GameObject ko = GameObject.FindGameObjectWithTag(str);
        ko.GetComponent<AudioSource>().Play();
    }
}
