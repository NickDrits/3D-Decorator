using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class GetCreateInputScript : MonoBehaviour
{
    [SerializeField] Button crt;
    [SerializeField] TMP_InputField[]  inputs;
    [SerializeField] TMP_Text error;   
    [SerializeField] GameObject currscreen;
    [SerializeField] GameObject nextscreen;

    private static readonly string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    private static readonly string datePattern = @"^(0?[1-9]|[12][0-9]|3[01])/(0?[1-9]|1[0-2])/[0-9]{4}$";
    private static readonly string phonePattern = @"^69\d{8}$"; 


    void Start()
    {
        crt.onClick.AddListener(GetInputHandler);
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
        if(ValidInputs(inputs,error,0))
        {
            StartCoroutine(Upload()); 
            error.text="";
            for(int i=0;i<inputs.Length;i++)
            {
                inputs[i].text="";
            }
        }
    }

//flag used for whether verify password field exists, flag = 0 means verify exists
    public static bool ValidInputs(TMP_InputField[]  inputs,TMP_Text error,int flag)
    {
        if(inputs[2].text.Length <6){
            error.text="username too short,make it at least 6 characters long.";
            return false;
        }

        //email check
        if (!IsValidEmail(inputs[3].text))
        {
            error.text = "Invalid email address.";
            return false;
        }

        //password checks
        if (inputs.Length >= 6 && flag == 0)
        {
            string fourthInputText = inputs[4].text.Trim();
            string fifthInputText = inputs[5].text.Trim();

            if (fourthInputText != fifthInputText)
            {
                error.text = "The passwords provided must have the same text.";
                return false; 
            }

            if(fourthInputText.Length < 8){
                error.text = "The password provided must have at least 8 characters.";
                return false;                 
            }
        }

        //phone number check
        if (!IsValidPhoneNumber(inputs[6-flag].text))
        {
            error.text = "Invalid phone number.";
            return false;
        }

        //date check
        if (!IsValidDate(inputs[7-flag].text))
        {
            error.text = "Invalid date format.";
            inputs[7-flag].text = "";
            return false;
        }
        return true;
    }


    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return Regex.IsMatch(email, emailPattern);
    }

    public static bool IsValidDate(string date)
    {
        if (string.IsNullOrEmpty(date))
            return false;

        return Regex.IsMatch(date, datePattern);
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return false;

        return Regex.IsMatch(phoneNumber, phonePattern);
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("Fname", inputs[0].text);
        form.AddField("Sname", inputs[1].text);
        form.AddField("Username", inputs[2].text);
        form.AddField("Email", inputs[3].text);
        form.AddField("Password", inputs[4].text);
        form.AddField("Phone", inputs[6].text);
        string[] date = inputs[7].text.Split('/');
        form.AddField("Day", date[0]);
        form.AddField("Month", date[1]);
        form.AddField("Year", date[2]);
        form.AddField("Country", inputs[8].text);
        form.AddField("Town", inputs[9].text);
        form.AddField("Street", inputs[10].text);
        form.AddField("Number", inputs[11].text);
        form.AddField("Postal", inputs[12].text);

       // Debug.Log("Form data: " + form.data.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity/3d_Decorators/Register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                error.text = "Error: " + www.error;
            }
            else
            {
               // Debug.Log("Response: " + www.downloadHandler.text);
                switch (www.downloadHandler.text)
                {
                    case "0":
                        currscreen.SetActive(false);
                        nextscreen.SetActive(true);
                        SceneManager. LoadScene("LoginScene");
                        break;
                    case "1":
                        error.text = "Connection to database failed.";
                        break;
                    case "3":
                        error.text = "Username already exists.";
                        break;
                    case "5":
                        error.text = "Email already exists.";
                        break;
                    case "6":
                        error.text = "Error during registration. Please try again.";
                        break;
                    default:
                        error.text = "Unknown error.";
                        break;
                }
            }
        }
    }

}
