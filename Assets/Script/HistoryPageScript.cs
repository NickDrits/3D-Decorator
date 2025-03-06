using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class HistoryPageScript : MonoBehaviour
{
    public GameObject HistoryTab;
    public GameObject HistoryPage;
    public TMP_Dropdown dropdown;
    public Button nextButton; // Reference to the Next button
    public Button previousButton; // Reference to the Previous button
    public TextMeshProUGUI pageno;

    private string[] orders;
    private List<GameObject> createdTabs = new List<GameObject>(); // List to keep track of created tabs

    private int currentPage = 0; // Current page index
    private const int pageSize = 6; // Number of items per page

    void Start()
    {
        StartCoroutine(CreateHistoryPage("descending_date"));
        // Add button listeners
        nextButton.onClick.AddListener(ShowNextPage);
        previousButton.onClick.AddListener(ShowPreviousPage);
    }

    public void CreateNewHistoryPage()
    {
        int selectedIndex = dropdown.value;
        string selectedOptionText = dropdown.options[selectedIndex].text;
        SortOrders(selectedOptionText);
        CreateHistoryTabs();
    }

    private IEnumerator CreateHistoryPage(string sortby)
    {
        yield return StartCoroutine(GetRequest("http://localhost/Unity/3d_Decorators/GetOrders.php", sortby));

        if (orders == null || orders.Length == 0)
        {
            Debug.LogError("Orders array is empty or null.");
            yield break;
        }

        CreateHistoryTabs();
    }

    private void CreateHistoryTabs()
    {
        DestroyCreatedTabs(); // Destroy previous tabs

        int startIndex = currentPage * pageSize;
        int entriesToProcess = Mathf.Min(orders.Length - startIndex, pageSize);

        for (int i = 0; i < entriesToProcess; i++)
        {
            GameObject newHistoryTab = Instantiate(HistoryTab);
            newHistoryTab.transform.SetParent(HistoryPage.transform);

            createdTabs.Add(newHistoryTab); // Add to the list of created tabs

            TextMeshProUGUI[] textFields = newHistoryTab.GetComponentsInChildren<TextMeshProUGUI>();

            if (textFields.Length >= 4)
            {
                string[] singleorder = orders[startIndex + i].Split(',');

                if (singleorder.Length >= 8)
                {
                    string oid = singleorder[0]; // Oid
                    float price = float.Parse(singleorder[1]); // Price
                    int transport = int.Parse(singleorder[2]); // Transport
                    string state = singleorder[3]; // State
                    int day = int.Parse(singleorder[5]); // Day
                    int month = int.Parse(singleorder[6]); // Month
                    int year = int.Parse(singleorder[7]); // Year

                    textFields[0].text = oid; // Oid
                    textFields[1].text = (price + transport).ToString(); // Sum of price and transport
                    textFields[2].text = $"{day}/{month}/{year}"; // Formatted date
                    textFields[3].text = state; // State
                }
                else
                {
                    Debug.LogError("Not enough data in the singleorder array.");
                }
            }
            else
            {
                Debug.LogError("Not enough TextMeshProUGUI components found.");
            }
        }

        UpdateButtonStates();
    }

    private IEnumerator GetRequest(string url, string sortby)
    {
        string phpUrl = url;
        string sortValue = sortby;
        int cid = Int32.Parse(GetLoginInputScript.Cid);
        string uri = $"{phpUrl}?sort_value={sortValue}&cid={cid}";

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
                    Debug.Log("Raw Response: " + rawresponse); // Log raw response
                    orders = rawresponse.Split('*');
                    break;
            }
        }
    }

    public void DestroyCreatedTabs()
    {
        foreach (var tab in createdTabs)
        {
            if (tab != null)
            {
                Destroy(tab);
            }
        }
        createdTabs.Clear(); // Clear the list after destruction
    }

    public void SortOrders(string sortCriteria)
    {
        if (orders == null || orders.Length == 0)
        {
            Debug.LogWarning("Orders array is null or empty.");
            return;
        }

        switch (sortCriteria)
        {
            case "Ascending Price":
                orders = orders.OrderBy(order => GetPriceSum(order)).ToArray();
                break;

            case "Descending Price":
                orders = orders.OrderByDescending(order => GetPriceSum(order)).ToArray();
                break;

            case "Ascending Date":
                orders = orders.OrderBy(order => GetDate(order)).ToArray();
                break;

            case "Descending Date":
                orders = orders.OrderByDescending(order => GetDate(order)).ToArray();
                break;

            default:
                Debug.LogWarning("Invalid sort criteria.");
                break;
        }
    }

    private float GetPriceSum(string order)
    {
        //Format: Oid,Price,Transport,State,Num_of_prod,Day,Month,Year,Cid
        string[] fields = order.Split(',');

        if (fields.Length >= 3)
        {
            float price = float.Parse(fields[1]);
            float transport = float.Parse(fields[2]);

            return price + transport;
        }

        return 0;
    }

    private DateTime GetDate(string order)
    {
        //Format: Oid,Price,Transport,State,Num_of_prod,Day,Month,Year,Cid
        string[] fields = order.Split(',');

        if (fields.Length >= 7)
        {
            int day = int.Parse(fields[5]);
            int month = int.Parse(fields[6]);
            int year = int.Parse(fields[7]);

            return new DateTime(year, month, day);
        }

        return DateTime.MinValue;
    }

    public void ShowNextPage()
    {
        if ((currentPage + 1) * pageSize < orders.Length)
        {
            currentPage++;
            pageno.text = (currentPage +1).ToString();
            CreateHistoryTabs();
        }
    }

    public void ShowPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            pageno.text = (currentPage +1).ToString();
            CreateHistoryTabs();
        }
    }

    private void UpdateButtonStates()
    {
        nextButton.interactable = (currentPage + 1) * pageSize < orders.Length;
        previousButton.interactable = currentPage > 0;
    }
}
