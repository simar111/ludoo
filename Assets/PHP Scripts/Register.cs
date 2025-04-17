using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField PhoneNumber;
    public TMP_Text msg;
    public Button submitBtn;

   PlayerImage playerImage;





    public void callRegister()
    {
        StartCoroutine(Registration());
    }



    IEnumerator Registration()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username.text);
        form.AddField("password", password.text);
        form.AddField("number", PhoneNumber.text);
        form.AddField("image", (int)playerImage);
        UnityWebRequest www = UnityWebRequest.Post("https://phpstack-1216068-4319747.cloudwaysapps.com/register.php", form);
        /*        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form);*/
        BasicUI.instance.showLoader();
        yield return www.SendWebRequest();
        BasicUI.instance.hideLoader();
        if (www.downloadHandler.text == "0")
        {
            Debug.Log("Created");
            DBManager.username = username.text;
            DBManager.MobileNumber = PhoneNumber.text;
            BasicUI.instance.setUsername();
            DBManager.isSignedIN = true;
            DBManager.myImage = playerImage;
            

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStartManager>().InsertImages();
            // Save username and password (consider encrypting the password)
            PlayerPrefs.SetString("username", username.text);
            PlayerPrefs.SetString("password", password.text); // Encrypt this in a real application
            PlayerPrefs.Save();
            this.transform.parent.gameObject.SetActive(false);
            username.text = "";
            password.text = "";
            PhoneNumber.text = "";
    

        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            msg.text = "This Username is already taken";
        }


    }


    public void selectImage(int num)
    {
        playerImage = (PlayerImage)num;
    }


    public void verify()
    {

        submitBtn.interactable = (username.text.Length >= 8 && password.text.Length >= 8 && PhoneNumber.text.Length == 10 && PhoneNumber.text[0] != '1' && PhoneNumber.text[0] != '2');
        if (submitBtn.interactable)
            msg.text = "Click on button to Register";
    }

}
