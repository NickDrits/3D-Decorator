using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HideHomepage : MonoBehaviour
{
    public Button Cart;
    public GameObject Proceed;
    public GameObject scroll;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        if (AddtoShopping.filteredFurniture == null || AddtoShopping.filteredFurniture.Length == 0)
        {
            Debug.LogError("AddtoShopping.filteredFurniture is null or empty.");
            return;
        }
        else
        {
            Cart.onClick.Invoke();
            Proceed.SetActive(true);
            scroll.SetActive(true);
            text.SetActive(false);
        }
    }
}
