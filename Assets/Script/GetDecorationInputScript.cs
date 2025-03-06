using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GetDecorationInputScript : MonoBehaviour
{
    [SerializeField] Button next;
    [SerializeField] TMP_InputField  inpwidth;
    [SerializeField] TMP_InputField  inplength;
    [SerializeField] TMP_InputField  inpheight;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] TMP_Text error;


    public static float width =90,length=120,height=35;
    public static string selectedOption="";

    void Start()
    {
        next.onClick.AddListener(GetInputHandler);
    }

    void GetInputHandler()
    {
        bool allValid = true;

        if (float.TryParse(inpwidth.text, out float parsedWidth))
        {
            width = parsedWidth * 20;
        }
        else
        {
            error.text = "Width is invalid or empty.";
            allValid = false;
        }

        if (float.TryParse(inplength.text, out float parsedLength))
        {
            length = parsedLength *20;
        }
        else
        {
            error.text = "Length is invalid or empty.";
            allValid = false;
        }

        if (float.TryParse(inpheight.text, out float parsedHeight))
        {
            height = parsedHeight *10;
        }
        else
        {
            error.text = "Height is invalid or empty.";
            allValid = false;
        }

        if (dropdown != null && dropdown.value >= 0 && dropdown.value < dropdown.options.Count)
        {
            selectedOption = dropdown.options[dropdown.value].text;
        }
        else
        {
            error.text = "No valid option selected from the dropdown.";
            allValid = false;
        }

        if (allValid)
        {
            Debug.Log($"Width: {width}, Length: {length}, Height: {height}, Room: {selectedOption}");
            error.text = "";
            SceneManager.LoadScene("3DroomScene");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("3DroomScene"));

        }
    }

}
