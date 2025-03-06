using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationScript : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Sprite img;
    [SerializeField] Button[] buttons;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject content;
    private static GameObject currContent;
    private string[] contentnames = {"Home","Profile","History","ShoppingCart" ,"Decorate"};
    public void OnClick(){
        for (int i = 0; i < buttons.Length; i++){
           buttons[i].GetComponent<Image>().sprite = sprites[i];
        }
        btn.GetComponent<Image>().sprite = img;
        if(currContent == null){
            
            content.transform.GetChild(0).gameObject.SetActive(false);
        }
        else{
            currContent.SetActive(false);
        }
        for(int i = 0; i < contentnames.Length; i++){
            if(contentnames[i] == btn.name){
                content.transform.GetChild(i).gameObject.SetActive(true);
                currContent = content.transform.GetChild(i).gameObject;
            }
        }
    }
}
