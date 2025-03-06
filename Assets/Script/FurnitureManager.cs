using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnitureManager : MonoBehaviour
{
    public static FurnitureManager Instance;

    public GameObject deleteConfirmationPopup;
    public Button yesButton;
    public Button noButton;

    private FurnitureMovement currentFurniture;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        yesButton.onClick.AddListener(DeleteCurrentFurniture);
        noButton.onClick.AddListener(HideDeleteConfirmation);
        deleteConfirmationPopup.SetActive(false);
    }

    public void ShowDeleteConfirmation(FurnitureMovement furniture)
    {
        currentFurniture = furniture;
        deleteConfirmationPopup.SetActive(true);
        CameraMovement.CanMove = false;
    }

    void HideDeleteConfirmation()
    {
        deleteConfirmationPopup.SetActive(false);
        CameraMovement.CanMove = true;
    }

    void DeleteCurrentFurniture()
    {
        if (currentFurniture != null)
        {
            Destroy(currentFurniture.gameObject);
        }
        HideDeleteConfirmation();
    }
}