using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GetListOfFurniture : MonoBehaviour
{
    // Reference to the panel where the buttons will be instantiated
    public GameObject panel;
    // Reference to the original button (to get its name when clicked)
    public Button originalButton;
    // Reference to the button prefab to instantiate for each item
    public GameObject buttonPrefab;
    
    public ScrollRect scrollView;

    private readonly string[] specificFurnitureTypes = { "Table", "Chair", "Couch", "Bed", "Stand" };

    void Start()
    {
        // Setup listener for the button press (example for testing)
        originalButton.onClick.AddListener(OnButtonClick);
    }

    // This method is triggered when the original button is clicked
      private void OnButtonClick()
    {
        CameraMovement.CanMove = false;

        ClearPanel();

        string buttonName = originalButton.name;

        List<string> filteredFurniture;
        if (buttonName == "Others")
        {
            filteredFurniture = GetOtherFurniture();
        }
        else
        {
            filteredFurniture = GetFurnitureOfType(buttonName);
        }

        CreateFurnitureButtons(filteredFurniture);
    }

    private List<string> GetOtherFurniture()
    {
        List<string> filteredList = new List<string>();

        foreach (var entry in GetFurnituresScript.furniture)
        {
            string[] data = entry.Split(',');

            if (data.Length > 2 && !specificFurnitureTypes.Contains(data[2]))
            {
                filteredList.Add(entry);
            }
        }

        return filteredList;
    }

    // Clears all buttons currently in the panel
    private void ClearPanel()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Filters the furniture entries by the given type
    private List<string> GetFurnitureOfType(string type)
    {
        List<string> filteredList = new List<string>();

        // Iterate through all furniture entries provided by GetFurnituresScript
        foreach (var entry in GetFurnituresScript.furniture)
        {
            string[] data = entry.Split(',');

            // Ensure that there are enough elements to access the type (index 2)
            if (data.Length > 2 && data[2] == type)
            {
                filteredList.Add(entry);
            }
            else
            {
                // Optionally log malformed entries or those without the correct type
                Debug.LogWarning("Skipping malformed entry or entry without expected type: " + entry);
            }
        }

        return filteredList;
    }

    // Creates buttons for each filtered furniture item and sets their labels
   private void CreateFurnitureButtons(List<string> filteredFurniture)
{
    foreach (var item in filteredFurniture)
    {
        // Create a new button
        GameObject newButton = Instantiate(buttonPrefab, panel.transform);

        // Get the name (second element) from the item
        string[] data = item.Split(',');

        // Ensure that the data array has the expected length before accessing
        if (data.Length > 1)
        {
            string furnitureName = data[1];  // Name is the second element (index 1)

            // Get the TextMeshProUGUI component
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

            // Try to load the image with the same name as the furniture
            Sprite furnitureSprite = Resources.Load<Sprite>("FurnitureImages/" + furnitureName);
            
            // If an image is found, set it as the button's image and hide the text
            if (furnitureSprite != null)
            {
                Image buttonImage = newButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = furnitureSprite;
                    
                    // Hide the button text
                    if (buttonText != null)
                    {
                        buttonText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogWarning("Button doesn't have an Image component: " + furnitureName);
                }
            }
            else
            {
                Debug.LogWarning("No matching image found for: " + furnitureName);
                
                // If no image is found, set the button text
                if (buttonText != null)
                {
                    buttonText.text = furnitureName;
                }
            }

            // Set up the button's onClick listener
            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnFurnitureButtonClicked(furnitureName));
        }
        else
        {
            Debug.LogWarning("Skipping entry with missing name: " + item);
        }
    }
}



    // This method is triggered when a furniture button is clicked
    private void OnFurnitureButtonClicked(string furnitureName)
    {
        // Hide the ScrollView
        if (scrollView != null)
        {
            scrollView.gameObject.SetActive(false);
        }

        // Path to the Furniture models directory
        string modelPath = "Furniture/" + furnitureName;

        // Try to load the 3D model prefab
        GameObject furniturePrefab = Resources.Load<GameObject>(modelPath);

        if (furniturePrefab != null)
        {
            CameraMovement.CanMove = true;
            // Instantiate the furniture model in the scene
            Vector3 spawnPosition = new Vector3(0, 10, 10); // You can adjust this position as needed
            Quaternion spawnRotation = Quaternion.identity; // Default rotation
            GameObject furnitureInstance = Instantiate(furniturePrefab, spawnPosition, spawnRotation);

            // Attach the FurnitureMovement script to the spawned furniture
            FurnitureMovement movementScript = furnitureInstance.AddComponent<FurnitureMovement>();

            Debug.Log("Placed furniture in scene and attached FurnitureMovement script: " + furnitureName);
        }
        else
        {
            Debug.LogWarning("No 3D model found for: " + furnitureName);
            
            // If no model is found, show the ScrollView again
            if (scrollView != null)
            {
                scrollView.gameObject.SetActive(true);
            }
        }
    }

}
