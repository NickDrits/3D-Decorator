using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HideShowPasswordScript : MonoBehaviour
{
    [SerializeField] Button hide;
    [SerializeField] TMP_InputField password;
    void Start()
    {
        hide.onClick.AddListener(HideShowHandler);       
    }

    void HideShowHandler(){
        
        if(password.contentType == TMP_InputField.ContentType.Password){
            password.contentType =TMP_InputField.ContentType.Standard;
        }
        else{
            password.contentType = TMP_InputField.ContentType.Password;
        }
        password.ForceLabelUpdate();
    }

    

}
