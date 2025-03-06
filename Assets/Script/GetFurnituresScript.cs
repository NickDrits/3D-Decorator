using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GetFurnituresScript : MonoBehaviour
{
    public static string[] furniture;

    void Start()
    {
//        Debug.Log(GetDecorationInputScript.width + " " +GetDecorationInputScript.length +" "+ GetDecorationInputScript.height + " " +GetDecorationInputScript.selectedOption) ;
        StartCoroutine(GetRequest("http://localhost/Unity/3d_Decorators/GetFurniture.php"));
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    
                    string rawresponse = webRequest.downloadHandler.text;
                    furniture = rawresponse.Split('*');
                    for (int i = 0; i<furniture.Length-1;i++)
                    {
//                        Debug.Log(furniture[i]);
                    }
                    break;
            }
        }
    }
}
