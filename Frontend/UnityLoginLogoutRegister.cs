using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UnityLoginLogoutRegister : MonoBehaviour
{

    public string baseUrl = "http://localhost/Nato_Auth/";


    public TMP_InputField accountUserName;
    public TMP_InputField accountPassword;
    public TextMeshProUGUI info;

    public GameObject nextButton;
    public GameObject ui;
    public GameObject planB;

    private string currentUsername;
    private string ukey = "accountusername";

    private void Start()
    {
        currentUsername = "";

        if (PlayerPrefs.HasKey(ukey))
        {
            if (PlayerPrefs.GetString(ukey) != "")
            {
                currentUsername = PlayerPrefs.GetString(ukey);
                info.text = "You are logged in as: " + currentUsername;
            }
            else
            {
                info.text = "You are not logged in.";
            }
        }
        else
        {
            info.text = "You are not logged in.";
        }

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "AccountLogin")
        {
            if (currentUsername == "")
            {
                nextButton.SetActive(true);
                planB.SetActive(false);
                ui.SetActive(true);
            }
            else
            {
                ui.SetActive(false);
                planB.SetActive(true);
                nextButton.SetActive(false);
            }
        }
    }

    public void AccountLogout()
    {
        currentUsername = "";
        PlayerPrefs.SetString(ukey, currentUsername);
        info.text = "You are just logged out.";
        SceneManager.LoadSceneAsync("AccountLogin");
    }

    public void AccountRegister()
    {
        string uName = accountUserName.text;
        string pWord = accountPassword.text;
        StartCoroutine(RegisterNewAccount(uName, pWord));
        SceneManager.LoadSceneAsync("AccountLogin");
    }

    public void AccountLogin()
    {
        string uName = accountUserName.text;
        string pWord = accountPassword.text;
        StartCoroutine(LogInAccount(uName, pWord));
    }

    IEnumerator RegisterNewAccount(string uName, string pWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("newAccountUsername", uName);
        form.AddField("newAccountPassword", pWord);
        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response = " + responseText);
                info.text = "Response = " + responseText;
            }
        }
    }

    IEnumerator LogInAccount(string uName, string pWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUsername", uName);
        form.AddField("loginPassword", pWord);
        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                if (responseText == "1")
                {
                    PlayerPrefs.SetString(ukey, uName);
                    info.text = "Welcome Back " + uName + " !";
                    SceneManager.LoadSceneAsync("TestingLobby");
                }
                else
                {
                    info.text = "Login Failed.";
                }
            }
        }
    }

    public void goToGame()
    {
        SceneManager.LoadSceneAsync("TestingLobby");
    }

    public void goToLogOut()
    {
        SceneManager.LoadSceneAsync("AccountLogout");
    }

    public void goToRegister()
    {
        SceneManager.LoadSceneAsync("AccountRegistration");
    }
}
