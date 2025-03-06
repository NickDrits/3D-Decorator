using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    public Material wallMaterial; 
    public Material floorMaterial; 

    public Material StartingMaterial;

    public static float RoomWidth { get; private set; }
    public static float RoomLength { get; private set; }
    public static float RoomHeight { get; private set; }

    public static Vector3[] WindowPositions { get; private set; }
    public static Vector3[] DoorPositions { get; private set; }

    public GameObject doorPrefab;
    public GameObject windowPrefab;
    public GameObject buttonsObject;
    public GameObject changeWinDoorObject;

    public Button leftButton;
    public Button rightButton;
    public Canvas mainCanvas;

    void Start()
    {
        wallMaterial.CopyPropertiesFromMaterial(StartingMaterial);
        floorMaterial.CopyPropertiesFromMaterial(StartingMaterial);

        // Fetch dimensions from GetDecorationInputScript
        float width = GetDecorationInputScript.width;
        float length = GetDecorationInputScript.length;
        float height = GetDecorationInputScript.height;

        Debug.Log("Fetched Dimensions: Width = " + width + ", Length = " + length + ", Height = " + height);

        // Assign room dimensions
        RoomWidth = width;
        RoomLength = length;
        RoomHeight = height;

        Debug.Log($"Room Created: Width={RoomWidth}, Length={RoomLength}, Height={RoomHeight}");

        // Create the room components
        Debug.Log("Creating Walls, Floor, and Ceiling...");
        CreateWall(new Vector3(0, height / 2, length / 2), new Vector3(width, height, 0.1f), wallMaterial); // Front wall
        CreateWall(new Vector3(0, height / 2, -length / 2), new Vector3(width, height, 0.1f), wallMaterial); // Back wall
        CreateWall(new Vector3(-width / 2, height / 2, 0), new Vector3(0.1f, height, length), wallMaterial); // Left wall
        CreateWall(new Vector3(width / 2, height / 2, 0), new Vector3(0.1f, height, length), wallMaterial); // Right wall
        CreateWall(new Vector3(0, height, 0), new Vector3(width, 0.1f, length), wallMaterial); // Ceiling
        CreateWall(new Vector3(0, 0, 0), new Vector3(width, 0.1f, length), floorMaterial); // Floor (uses floorMaterial)

        Debug.Log("Walls, Floor, and Ceiling Created.");

        // Calculate positions for doors and windows
        Debug.Log("Calculating Door and Window Positions...");
        CalculateWindowPositions();
        CalculateDoorPositions();

        // Instantiate doors and windows
        Debug.Log("Instantiating Doors and Windows...");
        InstantiateDoorsAndWindows();
    }

    void CreateWall(Vector3 position, Vector3 scale, Material material)
    {
        Debug.Log("Creating Wall at position " + position + " with scale " + scale);

        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = position;
        wall.transform.localScale = scale;

        // Assign material if specified
        if (material != null)
        {
            Renderer renderer = wall.GetComponent<Renderer>();
            renderer.material = material;
        }

        Debug.Log("Wall Created.");
    }

    private void CalculateWindowPositions()
    {
        Debug.Log("CalculateWindowPositions was called");
        WindowPositions = new Vector3[4];

        // Check if windowPrefab is assigned
        if (windowPrefab == null)
        {
            Debug.LogError("windowPrefab is not assigned! Please assign it in the Inspector.");
            return;
        }

        // Get the bounds of the window prefab
        Renderer windowRenderer = windowPrefab.GetComponentInChildren<Renderer>();
        if (windowRenderer == null)
        {
            Debug.LogError("Could not find Renderer on windowPrefab or its children!");
            return;
        }
        float windowHeight = windowRenderer.bounds.size.y;
        Debug.Log("Window Height: " + windowHeight);

        // Middle of each wall (assume room is centered at origin)
        WindowPositions[0] = new Vector3(0, RoomHeight / 2f - windowHeight / 2, RoomLength / 2f);
        Debug.Log("Window Position 0: " + WindowPositions[0]);
        WindowPositions[1] = new Vector3(RoomWidth / 2f, RoomHeight / 2f - windowHeight / 2, 0);
        Debug.Log("Window Position 1: " + WindowPositions[1]);
        WindowPositions[2] = new Vector3(0, RoomHeight / 2f - windowHeight / 2, -RoomLength / 2f);
        Debug.Log("Window Position 2: " + WindowPositions[2]);
        WindowPositions[3] = new Vector3(-RoomWidth / 2f, RoomHeight / 2f - windowHeight / 2, 0);
        Debug.Log("Window Position 3: " + WindowPositions[3]);
        Debug.Log("Window Positions Calculated");
    }

    private void CalculateDoorPositions()
    {
        Debug.Log("CalculateDoorPositions was called");
        DoorPositions = new Vector3[8];

        // Check if doorPrefab is assigned
        if (doorPrefab == null)
        {
            Debug.LogError("doorPrefab is not assigned! Please assign it in the Inspector.");
            return;
        }

        // Get the bounds of the door prefab
        Renderer doorRenderer = doorPrefab.GetComponentInChildren<Renderer>();
        if (doorRenderer == null)
        {
            Debug.LogError("Could not find Renderer on doorPrefab or its children!");
            return;
        }
        float doorWidth = doorRenderer.bounds.size.x;
        Debug.Log("Door Width: " + doorWidth);

        float doorOffset = 1.5f; // Offset from corners to place doors

        // Front wall
        DoorPositions[0] = new Vector3(-RoomWidth / 2f + doorOffset + doorWidth / 2, 0, RoomLength / 2f);
        Debug.Log("Door Position 0: " + DoorPositions[0]);
        DoorPositions[1] = new Vector3(RoomWidth / 2f - doorOffset - doorWidth / 2, 0, RoomLength / 2f);
        Debug.Log("Door Position 1: " + DoorPositions[1]);

        // Right wall
        DoorPositions[2] = new Vector3(RoomWidth / 2f, 0, RoomLength / 2f - doorOffset - doorWidth / 2);
        Debug.Log("Door Position 2: " + DoorPositions[2]);
        DoorPositions[3] = new Vector3(RoomWidth / 2f, 0, -RoomLength / 2f + doorOffset + doorWidth / 2);
        Debug.Log("Door Position 3: " + DoorPositions[3]);

        // Back wall
        DoorPositions[4] = new Vector3(RoomWidth / 2f - doorOffset - doorWidth / 2, 0, -RoomLength / 2f);
        Debug.Log("Door Position 4: " + DoorPositions[4]);
        DoorPositions[5] = new Vector3(-RoomWidth / 2f + doorOffset + doorWidth / 2, 0, -RoomLength / 2f);
        Debug.Log("Door Position 5: " + DoorPositions[5]);

        // Left wall
        DoorPositions[6] = new Vector3(-RoomWidth / 2f, 0, -RoomLength / 2f + doorOffset + doorWidth / 2);
        Debug.Log("Door Position 6: " + DoorPositions[6]);
        DoorPositions[7] = new Vector3(-RoomWidth / 2f, 0, RoomLength / 2f - doorOffset - doorWidth / 2);
        Debug.Log("Door Position 7: " + DoorPositions[7]);

        Debug.Log("Door Positions Calculated");
    }

void InstantiateDoorsAndWindows()
{
    if (DoorPositions.Length > 0 && doorPrefab != null)
    {
        GameObject doorInstance = Instantiate(doorPrefab, DoorPositions[0], Quaternion.identity);
        GameObject doorUI = Instantiate(changeWinDoorObject, mainCanvas.transform);
        AttachChangeWinDoorScript(doorInstance, doorUI);
    }

    if (WindowPositions.Length > 0 && windowPrefab != null)
    {
        GameObject windowInstance = Instantiate(windowPrefab, WindowPositions[0], Quaternion.identity);
        GameObject windowUI = Instantiate(changeWinDoorObject, mainCanvas.transform);
        AttachChangeWinDoorScript(windowInstance, windowUI);
    }
}

void AttachChangeWinDoorScript(GameObject instance, GameObject uiInstance)
{
    ChangeWinDoor changeWinDoorScript = instance.AddComponent<ChangeWinDoor>();
    changeWinDoorScript.Initialize(buttonsObject, uiInstance, instance, leftButton, rightButton);}
}

