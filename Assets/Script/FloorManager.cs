using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloorManager : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject buttonPrefab;
    public Material floorMaterial;
    public ScrollRect scrollView;

    private string materialsPath = "FloorMaterials";

    public void PopulateScrollView()
    {
        Debug.Log("PopulateScrollView method called");
        ClearScrollView();
        Material[] materials = Resources.LoadAll<Material>(materialsPath);
        Debug.Log($"Found {materials.Length} materials in Resources folder");
        CreateMaterialButtons(materials);
    }

    void ClearScrollView()
    {
        Debug.Log("Clearing ScrollView");
        int childCount = scrollViewContent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(scrollViewContent.transform.GetChild(i).gameObject);
        }
        Debug.Log($"Cleared {childCount} items from ScrollView");
    }

    void CreateMaterialButtons(Material[] materials)
    {
        Debug.Log("Creating material buttons");
        foreach (var material in materials)
        {
            GameObject newButton = Instantiate(buttonPrefab, scrollViewContent.transform);
            if (newButton == null)
            {
                Debug.LogError("Failed to instantiate button prefab");
                continue;
            }
            newButton.transform.localScale = Vector3.one;

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = material.name;
                Debug.Log($"Created button for material: {material.name}");
            }
            else
            {
                Debug.LogWarning($"TextMeshProUGUI component not found on button for material: {material.name}");
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => OnMaterialButtonClicked(material));
            }
            else
            {
                Debug.LogError($"Button component not found on button for material: {material.name}");
            }
        }
    }

    void OnMaterialButtonClicked(Material material)
    {
        Debug.Log($"Material button clicked: {material.name}");
        if (floorMaterial != null)
        {
            floorMaterial.CopyPropertiesFromMaterial(material);
            Debug.Log($"Applied material {material.name} to floor");
        }
        else
        {
            Debug.LogError("Floor material is not assigned");
        }
        
        if (scrollView != null)
        {
            scrollView.gameObject.SetActive(false);
            Debug.Log("ScrollView hidden");
        }
        else
        {
            Debug.LogWarning("ScrollView reference is null");
        }
    }
}

