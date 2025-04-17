using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text msg;
    public Button submitBtn;

    public void callRegister()
    {
        StartCoroutine(LoginStart());
    }

    IEnumerator LoginStart()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username.text);
        form.AddField("password", password.text);
        UnityWebRequest www = UnityWebRequest.Post("https://phpstack-1216068-4319747.cloudwaysapps.com/login.php", form);
        /*    UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form);*/
        BasicUI.instance.showLoader();
        yield return www.SendWebRequest();
        BasicUI.instance.hideLoader();

        if (www.downloadHandler.text[0] == '0')
        {
            DBManager.username = username.text;
            DBManager.TotalBalance = int.Parse(www.downloadHandler.text.Split('\t')[1]);
            DBManager.MobileNumber = www.downloadHandler.text.Split('\t')[2];
            DBManager.myImage = (PlayerImage)int.Parse(www.downloadHandler.text.Split('\t')[3]);

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStartManager>().InsertImages();
            DBManager.isSignedIN = true;
            // Save username and password (consider encrypting the password)
            PlayerPrefs.SetString("username", username.text);
            PlayerPrefs.SetString("password", password.text); // Encrypt this in a real application
            PlayerPrefs.Save();

            username.text = "";
            string dop = password.text;
            password.text = "";
            msg.text = "Enter Username and Password";
            this.transform.parent.gameObject.SetActive(false);
            BasicUI.instance.setUsername();
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            msg.text = "Wrong Credentials";
            Debug.Log("UserLogin Failed: " + www.downloadHandler.text);
        }
    }

    // Method to retrieve stored username and password
    public void LoadCredentials()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            username.text = PlayerPrefs.GetString("username");
        }

        if (PlayerPrefs.HasKey("password"))
        {
            password.text = PlayerPrefs.GetString("password"); // Decrypt this in a real application
        }
    }
}
