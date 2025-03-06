using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownColorScript : MonoBehaviour
{
    [SerializeField] private GameObject drp;
    private string hexColor = "00FF66";

    public void OnClick()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#" + hexColor, out color))
        {
            Image dropdownImage = drp.GetComponent<Image>();
            if (dropdownImage != null)
            {
                dropdownImage.color = color;
            }
            else
            {
                Debug.LogWarning("No Image component found on the GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid color string: " + hexColor);
        }
    }
}
