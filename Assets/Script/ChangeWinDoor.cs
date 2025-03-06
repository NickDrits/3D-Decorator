using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWinDoor : MonoBehaviour
{
    private float lastTapTime;
    private float doubleTapTime = 0.3f;
    private float holdTime = 0.5f;
    private bool isHolding = false;

    private GameObject buttonsObject;
    private GameObject changeWinDoorObject;

    // Buttons (found dynamically at runtime)
    private Button moveButton;
    private Button changeButton;
    private Button addButton;
    private Button deleteButton;
    private Button returnButton;

    // Reference to movement buttons
    public Button leftButton;  // Assigned via CreateRoom
    public Button rightButton; // Assigned via CreateRoom

    // Reference to the object that triggered this UI
    private GameObject targetObject;

    // Current position index in the positions array
    private int currentPositionIndex = 0;

    public void Initialize(GameObject buttons, GameObject changeWinDoor, GameObject target, Button leftButton, Button rightButton)
    {
        buttonsObject = buttons;
        changeWinDoorObject = changeWinDoor;
        targetObject = target;

        this.leftButton = leftButton;
        this.rightButton = rightButton;

        if (changeWinDoorObject != null)
        {
            // Dynamically find buttons as children of changeWinDoorObject
            moveButton = FindButtonByName(changeWinDoorObject, "Move");
            changeButton = FindButtonByName(changeWinDoorObject, "Change");
            addButton = FindButtonByName(changeWinDoorObject, "Add");
            deleteButton = FindButtonByName(changeWinDoorObject, "Delete");
            returnButton = FindButtonByName(changeWinDoorObject, "Return");

            // Assign functionality to the Delete button
            if (deleteButton != null)
                deleteButton.onClick.AddListener(OnDelete);
            else
                Debug.LogError("Delete button not found in ChangeWinDoor object!");
            
            if (returnButton != null)
            {
                returnButton.onClick.RemoveAllListeners(); // Remove any existing listeners
                returnButton.onClick.AddListener(OnReturn); // Add listener for this functionality
            }
        }
        else
        {
            Debug.LogError("ChangeWinDoor object not assigned!");
        }
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (IsObjectTouched(touch.position))
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        lastTapTime = Time.time;
                        isHolding = true;
                        break;

                    case TouchPhase.Ended:
                        if (Time.time - lastTapTime < doubleTapTime)
                            OnDoubleTap();

                        isHolding = false;
                        break;
                }

                // Check for hold gesture outside the switch statement
                if (isHolding && Time.time - lastTapTime > holdTime)
                {
                    OnHold();
                    isHolding = false;
                }
            }
        }
        else
        {
            // Reset holding state if touch count is not 1
            isHolding = false;
        }
    }

    bool IsObjectTouched(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit) && hit.transform == this.transform;
    }

    void OnDoubleTap()
    {
        ToggleCanvasObjects();
    }

    void OnHold()
    {
        ToggleCanvasObjects();
    }

    void ToggleCanvasObjects()
    {
        if (buttonsObject != null && changeWinDoorObject != null)
        {
            buttonsObject.SetActive(false);
            changeWinDoorObject.SetActive(true);

            // Assign movement button functionality dynamically for this specific target object
            AssignMovementButtons();

            Debug.Log($"Toggled UI for {targetObject.name}");
        }
        else
        {
            Debug.LogError("Buttons or ChangeWinDoor object not found for this instance!");
        }
    }

    void AssignMovementButtons()
    {
        if (leftButton != null)
        {
            leftButton.onClick.RemoveAllListeners(); // Remove any existing listeners
            leftButton.onClick.AddListener(() => MoveLeft()); // Add listener for this target object
        }

        if (rightButton != null)
        {
            rightButton.onClick.RemoveAllListeners(); // Remove any existing listeners
            rightButton.onClick.AddListener(() => MoveRight()); // Add listener for this target object
        }
    }

    void OnDelete()
    {
        if (targetObject != null)
        {
            Debug.Log($"Deleting object: {targetObject.name}");
            Destroy(targetObject);
            CloseUI();
        }
        else
        {
            Debug.LogError("Target object is null! Cannot delete.");
        }
    }

    void CloseUI()
    {
        if (buttonsObject != null && changeWinDoorObject != null)
        {
            buttonsObject.SetActive(true);
            changeWinDoorObject.SetActive(false);

            // Clear movement button listeners when UI is closed
            ClearMovementButtons();

            Debug.Log("Closed ChangeWinDoor UI.");
        }
    }

    void ClearMovementButtons()
    {
        if (leftButton != null) leftButton.onClick.RemoveAllListeners();
        if (rightButton != null) rightButton.onClick.RemoveAllListeners();
    }

    private Button FindButtonByName(GameObject parent, string buttonName)
    {
        Transform buttonTransform = parent.transform.Find(buttonName);
        if (buttonTransform != null)
        {
            Button button = buttonTransform.GetComponent<Button>();
            if (button != null)
                return button;
            else
                Debug.LogError($"Button component not found on {buttonName}");
        }
        else
        {
            Debug.LogError($"{buttonName} button not found in ChangeWinDoor object");
        }

        return null;
    }

    public void MoveLeft()
    {
        MoveToNextPosition(-1);
    }

    public void MoveRight()
    {
        MoveToNextPosition(1);
    }

    private void MoveToNextPosition(int direction)
    {
        // Determine which array (WindowPositions or DoorPositions) contains the current position
        Vector3[] positionsArray = null;

        // Check if targetObject's position matches any position in WindowPositions
        for (int i = 0; i < CreateRoom.WindowPositions.Length; i++)
        {
            if (targetObject.transform.position == CreateRoom.WindowPositions[i])
            {
                positionsArray = CreateRoom.WindowPositions;
                currentPositionIndex = i; // Set the current index based on its position in the array
                break;
            }
        }

        // If not found in WindowPositions, check DoorPositions
        if (positionsArray == null)
        {
            for (int i = 0; i < CreateRoom.DoorPositions.Length; i++)
            {
                if (targetObject.transform.position == CreateRoom.DoorPositions[i])
                {
                    positionsArray = CreateRoom.DoorPositions;
                    currentPositionIndex = i; // Set the current index based on its position in the array
                    break;
                }
            }
        }

        // If no matching position is found, exit early
        if (positionsArray == null || positionsArray.Length == 0)
        {
            Debug.LogError("Target object's position does not match any predefined positions.");
            return;
        }

        // Update the current index based on direction
        currentPositionIndex += direction;

        // Wrap around the index to stay within bounds
        if (currentPositionIndex < 0) currentPositionIndex = positionsArray.Length - 1;
        else if (currentPositionIndex >= positionsArray.Length) currentPositionIndex = 0;

        // Retrieve and apply the next position
        Vector3 nextPosition = positionsArray[currentPositionIndex];
        targetObject.transform.position = nextPosition;

        // Apply rotation logic based on whether it's a window or door
        if (positionsArray == CreateRoom.WindowPositions) // It's a window
        {
            float rotationYChange = direction > 0 ? 90f : -90f; // +90 for right, -90 for left
            targetObject.transform.Rotate(0, rotationYChange, 0);
        }
        else if (positionsArray == CreateRoom.DoorPositions) // It's a door
        {
            int rotationGroup = currentPositionIndex / 2; // Each pair of positions has the same rotation
            float newRotationY = rotationGroup * 90f; // Calculate rotation: 0°, 90°, 180°, or 270°
            targetObject.transform.rotation = Quaternion.Euler(0, newRotationY, 0);
        }

        Debug.Log($"{targetObject.name} moved to position {nextPosition} with rotation {targetObject.transform.rotation.eulerAngles}");
    }

void OnReturn()
{
    // Find the Canvas (assuming it is the parent of the ChangeWinDoor objects)
    Transform canvasTransform = changeWinDoorObject.transform.parent;

    if (canvasTransform != null)
    {
        // Iterate through all children of the Canvas
        for (int i = 0; i < canvasTransform.childCount; i++)
        {
            Transform child = canvasTransform.GetChild(i);

            // Check if the child's name matches "ChangeWinDoor(Clone)"
            if (child.name == "ChangeWinDoor(Clone)")
            {
                child.gameObject.SetActive(false); // Set the child GameObject to inactive
            }
        }

        Debug.Log("All ChangeWinDoor(Clone) children have been set to inactive.");
    }
    else
    {
        Debug.LogError("Canvas transform not found. Ensure ChangeWinDoor objects are under a Canvas.");
    }
}


}
