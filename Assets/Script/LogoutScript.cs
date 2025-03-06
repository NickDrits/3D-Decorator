using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutScript : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
