using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetResetInputScript : MonoBehaviour
{
    [SerializeField] Button reset;
    [SerializeField] TMP_InputField  email;
    [SerializeField] GameObject currscreen;
    [SerializeField] GameObject nextscreen;
    [SerializeField] TMP_Text error;

    void Start()
    {
        reset.onClick.AddListener(GetInputHandler);
    }

    void GetInputHandler()
    {
        string emailText = email.text.Trim();

        if (!string.IsNullOrEmpty(emailText))
        {
            for(int i = 0; i < GetLoginInputScript.users.Length-1;i++){
                string[] userInfo = GetLoginInputScript.users[i].Split(",");
                if(userInfo[1] == emailText)
                {
                    currscreen.SetActive(false);
                    nextscreen.SetActive(true);
                    error.text="";
                    email.text="";
                    return;
                }
            }
            email.text ="";
            error.text="Invalid email address.";
        }
        else
        {
            error.text="Please fill in the email field.";
        }

    }
}
