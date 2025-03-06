using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class AddtoShopping : MonoBehaviour
{
    public static string[] filteredFurniture;
    private List<string> furnitureInScene = new List<string>();

    public GameObject emptyListPopup;  // Assign this in the Inspector
    public GameObject nonEmptyListPopup;  // Assign this in the Inspector

    public void GetFurniture()
    {
        FindFurnitureInScene();
        FilterFurnitureList();
        PrintFilteredFurniture();
        ShowAppropriatePopup();
    }

    void FindFurnitureInScene()
    {
        FurnitureMovement[] furnitureObjects = FindObjectsOfType<FurnitureMovement>();
        foreach (FurnitureMovement furniture in furnitureObjects)
        {
            string furnitureName = furniture.gameObject.name.Replace("(Clone)", "").Trim();
            furnitureInScene.Add(furnitureName);
        }
    }

    void FilterFurnitureList()
    {
        if (GetFurnituresScript.furniture != null && GetFurnituresScript.furniture.Length > 0)
        {
            List<string> tempFilteredList = new List<string>();
            foreach (string item in GetFurnituresScript.furniture)
            {
                if (item != null && item.Split(',').Length > 1)
                {
                    string furnitureName = item.Split(',')[1].Trim();
                    int count = furnitureInScene.Count(f => f == furnitureName);
                    for (int i = 0; i < count; i++)
                    {
                        tempFilteredList.Add(item);
                    }
                }
            }
            filteredFurniture = tempFilteredList.ToArray();
        }
        else
        {
            filteredFurniture = new string[0];
            Debug.LogWarning("GetFurnituresScript.furniture is null or empty");
        }
    }


    void PrintFilteredFurniture()
    {
        Debug.Log("Furniture in scene: " + string.Join(", ", furnitureInScene));
        Debug.Log("Filtered Furniture List:");
        foreach (string furniture in filteredFurniture)
        {
            Debug.Log(furniture);
        }
    }

    void ShowAppropriatePopup()
    {
        if (filteredFurniture.Length == 0)
        {
            StartCoroutine(ShowPopupForDuration(emptyListPopup, 3f));
        }
        else
        {
            StartCoroutine(ShowPopupForDuration(nonEmptyListPopup, 3f));
        }
    }

    IEnumerator ShowPopupForDuration(GameObject popup, float duration)
    {
        popup.SetActive(true);
        yield return new WaitForSeconds(duration);
        popup.SetActive(false);
    }

    public void ChangeScene()
    {
        string[] objectsToKeep = { "Directional Light", "Canvas", "EventSystem", "Camera", "Button", "FurnitureManager" };
        
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (!objectsToKeep.Contains(obj.name) && 
                !IsChildOfKeepObject(obj, "Canvas") && 
                !IsChildOfKeepObject(obj, "Button"))
            {
                Destroy(obj);
            }
        }
        
        SceneManager.LoadScene("MainAppScene");
    }

    private bool IsChildOfKeepObject(GameObject obj, string parentName)
    {
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            if (parent.name == parentName)
            {
                return true;
            }
            parent = parent.parent;
        }
        return false;
    }

}
