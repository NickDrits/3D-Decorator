using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GetLoginInputScript : MonoBehaviour
{
    [SerializeField] Button lgn;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_Text error;
    [SerializeField] TMP_Text success;
    public static string[] users; // Array to store all users' data
    public static string[] userInfo; // Array to store a single user's info
    public static string Cid;
    public static string Password;

    void Start()
    {
        lgn.onClick.AddListener(GetInputHandler);
        StartCoroutine(GetRequest("http://localhost/Unity/3d_Decorators/Login.php"));
    }

    void GetInputHandler()
    {
        string usernameText = username.text.Trim();
        string passwordText = password.text.Trim();

        if (!string.IsNullOrEmpty(usernameText) && !string.IsNullOrEmpty(passwordText))
        {
            error.text = "";
            // Send the login request for username and password verification
            StartCoroutine(PostLoginRequest(usernameText, passwordText));
        }
        else
        {
            error.text = "Please fill in both the username and password fields.";
            success.gameObject.SetActive(false);
        }
    }

    // Coroutine to fetch all users' data from the database
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Parse the response and store all users' data
                string rawresponse = webRequest.downloadHandler.text;
               // Debug.Log(rawresponse);
                users = rawresponse.Split('*'); // Split the data by "*"
            }
        }
    }

    // Coroutine to send username and password for verification
    IEnumerator PostLoginRequest(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("Username", username);
        form.AddField("Password", password);

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/Unity/3d_Decorators/VerifyLogin.php", form))
        {
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                error.text = "Connection error. Please try again.";
                yield break; // Exit the coroutine
            }
            else
            {
                string response = webRequest.downloadHandler.text;

                if (response == "0")
                {
                    // Login successful, find the corresponding user info
                    for (int i = 0; i < users.Length; i++)
                    {
                        userInfo = users[i].Split(",");
                        Cid = userInfo[8];
                       // Debug.Log(Cid);
                        Password = password;
                       // Debug.Log(Password);
                        if (userInfo[2] == username || userInfo[3] == username) // Username or email match
                        {
                            // Proceed with login
                            SceneManager.LoadScene("MainAppScene");
                            yield break; // Exit the coroutine after successful login
                        }
                    }

                }
                else
                {
                    error.text = "Invalid credentials!";
                }

            }
        }
    }
}
