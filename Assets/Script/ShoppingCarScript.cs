using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShoppingCarScript : MonoBehaviour
{
    // Reference to the parent GameObject where components will be instantiated
    public GameObject componentParent;
    // Prefab for the component (should have Image, NameText, and PriceText placeholders)
    public GameObject componentPrefab;
    
    public static float totalPrice = 0f; // Static variable to sum all prices

    void Start()
    {
        totalPrice = 0f; // Reset total price at the start

        // Check if AddtoShopping.filteredFurniture is null or empty
        if (AddtoShopping.filteredFurniture == null || AddtoShopping.filteredFurniture.Length == 0)
        {
            Debug.LogError("AddtoShopping.filteredFurniture is null or empty.");
            return;
        }

        // Check if componentParent and componentPrefab are assigned
        if (componentParent == null)
        {
            Debug.LogError("componentParent is not assigned.");
            return;
        }
        if (componentPrefab == null)
        {
            Debug.LogError("componentPrefab is not assigned.");
            return;
        }

        // Loop through each entry in AddtoShopping.filteredFurniture array
        foreach (string entry in AddtoShopping.filteredFurniture)
        {
            // Split the entry by commas to extract data
            string[] entryData = entry.Split(',');
            // Ensure the entry has enough fields
            if (entryData.Length < 14)
            {
                Debug.LogWarning($"Invalid entry format: {entry}");
                continue;
            }

            // Extract name (second value) and price (second-to-last value)
            string furnitureName = entryData[1];
            string furniturePrice = entryData[13]; // Second-to-last value

            Debug.Log($"Processing furniture: {furnitureName}, Price: {furniturePrice}");

            // Create a new component instance from the prefab
            GameObject newComponent = Instantiate(componentPrefab, componentParent.transform);

            // Find child objects in the prefab
            Transform imageTransform = newComponent.transform.Find("Image");
            Transform nameTextTransform = newComponent.transform.Find("NameText");
            Transform priceTextTransform = newComponent.transform.Find("PriceText");

            // Check if all required child objects exist
            if (imageTransform == null || nameTextTransform == null || priceTextTransform == null)
            {
                Debug.LogError($"One or more child objects (Image, NameText, PriceText) are missing in prefab: {newComponent.name}");
                Destroy(newComponent); // Clean up incomplete UI element
                continue;
            }

            // Get components from child objects
            Image furnitureImage = imageTransform.GetComponent<Image>();
            TextMeshProUGUI nameText = nameTextTransform.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = priceTextTransform.GetComponent<TextMeshProUGUI>();

            // Check if components are attached to child objects
            if (furnitureImage == null)
                Debug.LogError("Image component is missing on 'Image' child object.");
            if (nameText == null)
                Debug.LogError("Text component is missing on 'NameText' child object.");
            if (priceText == null)
                Debug.LogError("Text component is missing on 'PriceText' child object.");

            // Load image from Resources/FurnitureImages using furnitureName
            Sprite imageSprite = Resources.Load<Sprite>($"FurnitureImages/{furnitureName}");
            if (imageSprite != null)
                furnitureImage.sprite = imageSprite;
            else
                Debug.LogWarning($"Image for {furnitureName} not found in Resources/FurnitureImages.");

            // Set name and price text
            nameText.text = furnitureName;
            priceText.text = $"${furniturePrice}";

            // Set the created component active
            newComponent.SetActive(true);

            // Add the price to the total
            if (float.TryParse(furniturePrice, out float price))
            {
                totalPrice += price;
            }
            else
            {
                Debug.LogWarning($"Unable to parse price: {furniturePrice}");
            }
        }

        // Print the total price at the end
        Debug.Log($"Total price of all items: ${totalPrice:F2}");
    }
}
