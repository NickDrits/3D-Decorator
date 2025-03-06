using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
public class ProfileScript : MonoBehaviour
{
    [SerializeField] TMP_InputField[]  inputs;
    [SerializeField] TMP_Text error;  
    [SerializeField] Button crt;
    [SerializeField] Button add;

    private string[] address;
  

    void Start()
    {
        int j =0;
        crt.onClick.AddListener(GetInputHandler);

        for(int i = 0; i <GetLoginInputScript.userInfo.Length; i++)
        {
            if(i == 5)
            {
                string concatbday = GetLoginInputScript.userInfo[i] + "/" + GetLoginInputScript.userInfo[++i] + "/" + GetLoginInputScript.userInfo[++i];
                inputs[j].text = concatbday;
                i++;
            }
            else 
            {
                inputs[j].text = GetLoginInputScript.userInfo[i];
            }
            j++;
        }
        inputs[j].text = GetLoginInputScript.Password;
        crt.gameObject.SetActive(false);


    }

    void GetInputHandler()
    {
        foreach (TMP_InputField inputField in inputs)
        {
            string inputText = inputField.text.Trim();
            if (string.IsNullOrEmpty(inputText))
            {
                error.text ="Please fill in all the input fields.";
                return; 
            }
        }
        if(GetCreateInputScript.ValidInputs(inputs,error,1))
        {
            error.text="";
            StartCoroutine(UpdateClientInfo());
        }
    }

        IEnumerator UpdateClientInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("Cid", GetLoginInputScript.Cid);
        form.AddField("Fname", inputs[0].text);
        form.AddField("Sname", inputs[1].text);
        form.AddField("Username", inputs[2].text);
        form.AddField("Email", inputs[3].text);
        form.AddField("Password", inputs[4].text);
        form.AddField("Phone", inputs[5].text);
        string[] date = inputs[6].text.Split('/');
        form.AddField("Day", date[0]);
        form.AddField("Month", date[1]);
        form.AddField("Year", date[2]);
        form.AddField("Country", inputs[7].text);
        form.AddField("Town", inputs[8].text);
        form.AddField("Street", inputs[9].text);
        form.AddField("Number", inputs[10].text);
        form.AddField("Postal", inputs[11].text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity/3d_Decorators/UpdateClient.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                error.text = "Error: " + www.error;
            }
            else
            {
                switch (www.downloadHandler.text)
                {
                    case "0":
                        break;
                    case "1":
                        error.text = "Connection to database failed.";
                        break;
                    case "2":
                        error.text = "Cid not found.";
                        break;
                    case "3":
                        error.text = "Username already exists.";
                        break;
                    case "4":
                        error.text = "Email already exists.";
                        break;
                    case "5":
                        error.text = "Error during update. Please try again.";
                        break;
                    default:
                        error.text = "Unknown error.";
                        break;
                }
            }
        }
    }
}
