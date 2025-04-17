using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    public GameObject signIn;
    private string savedUsername;
    private string savedPassword;


    public Button OnlineMultiplayer;
    public Button VSFriends;

    private SpriteCollection spriteCollection;

    public Image UIProfilePic;
    public Image ConnectScreen2;
    public Image ConnectScreen4;
    public Image ConnectScreenFriends;

    void Start()
    {
        spriteCollection = Resources.Load<SpriteCollection>("NewSpriteCollection");
        CheckCredentials();
    }

    // Update is called once per frame
    void Update()
    {
        if (OnlineMultiplayer && VSFriends)
        {
            OnlineMultiplayer.interactable = DBManager.isSignedIN;
            VSFriends.interactable = DBManager.isSignedIN;

        }


        
    }

    private void CheckCredentials()
    {
        if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("password"))
        {
             savedUsername = PlayerPrefs.GetString("username");
             savedPassword = PlayerPrefs.GetString("password"); // Decrypt this in a real application

            Debug.Log("Credentials found:");
            Debug.Log("Username: " + savedUsername);
            // Debug.Log("Password: " + savedPassword); // For security reasons, avoid logging passwords in production

            // If needed, perform auto-login or other actions
            // For example, you might want to call a login function automatically:
            AutoLogin();
        }
        else
        {
            Debug.Log("No saved credentials found.");
            // You can prompt the user to log in manually here
        }
    }

    // Example of an auto-login method (if applicable)
    private void AutoLogin()
    {

         StartCoroutine(LoginStart());
    }



    IEnumerator LoginStart()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", savedUsername);
        form.AddField("password", savedPassword);
        UnityWebRequest www = UnityWebRequest.Post("https://phpstack-1216068-4319747.cloudwaysapps.com/login.php", form);
        /*        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form);*/
        BasicUI.instance.showLoader();
        yield return www.SendWebRequest();
        BasicUI.instance.hideLoader();
        Debug.Log(www.downloadHandler.text);
        if (www.downloadHandler.text[0] == '0')
        {
            DBManager.username = savedUsername;
            DBManager.TotalBalance = int.Parse(www.downloadHandler.text.Split('\t')[1]);
            DBManager.MobileNumber = www.downloadHandler.text.Split('\t')[2];
            DBManager.myImage = (PlayerImage)int.Parse(www.downloadHandler.text.Split('\t')[3]);
            InsertImages();
            DBManager.isSignedIN = true;
            signIn.SetActive(false);
            BasicUI.instance.setUsername();
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("UserLogin Failed: " + www.downloadHandler.text);
        }
    }




    public void InsertImages()
    {
        UIProfilePic.sprite = spriteCollection.sprites[(int)DBManager.myImage];
        ConnectScreen2.sprite = spriteCollection.sprites[(int)DBManager.myImage];
        ConnectScreen4.sprite = spriteCollection.sprites[(int)DBManager.myImage];
        ConnectScreenFriends.sprite = spriteCollection.sprites[(int)DBManager.myImage];
    }
}
