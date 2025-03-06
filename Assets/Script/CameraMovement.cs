using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 0.5f; // Speed of camera movement based on swipe
    public static Vector3 initialCameraPosition; // Initial position of the camera
    public static bool CanMove = true; // Determines if the camera can move

    private float orbitWidth; // Width (X-axis) of the elliptical orbit
    private float orbitHeight; // Height (Z-axis) of the elliptical orbit

    private float angle = 0f; // Angle for calculating orbital position
    private float touchStartX; // To track the starting X position of a touch

    void Start()
    {
        // Dynamically fetch room dimensions from CreateRoom.cs
        orbitWidth = GetDecorationInputScript.width / 2f;  // Half of the room's width
        orbitHeight = GetDecorationInputScript.length / 2f; // Half of the room's height
        float cameraY = GetDecorationInputScript.height -10;
        transform.position = new Vector3(transform.position.x, cameraY, transform.position.z);
        float x = Mathf.Cos(angle) * orbitWidth;
        float z = Mathf.Sin(angle) * orbitHeight;

        transform.position = new Vector3(x, cameraY, z);

        // Store initial camera position
        initialCameraPosition = transform.position;
        transform.LookAt(Vector3.zero);
//        Debug.Log($"Initial Camera Position: {initialCameraPosition}");
//        Debug.Log($"Orbit Dimensions: Width={orbitWidth}, Height={orbitHeight}");
    }

    public void CanMoveAgain()
    {
        CanMove = true;
    }

    void Update()
    {
        if (CanMove)
        {
            HandleTouchInput();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Record the starting X position of the touch
                    touchStartX = touch.position.x;
                    break;

                case TouchPhase.Moved:
                    // Calculate how far the finger has moved horizontally
                    float deltaX = touch.position.x - touchStartX;

                    // Adjust the angle based on swipe direction and speed
                    angle -= deltaX * rotationSpeed * Time.deltaTime;

                    // Update the starting X position for continuous movement
                    touchStartX = touch.position.x;

                    // Update camera position along the elliptical path
                    UpdateCameraPosition();
                    break;
            }
        }
    }

    private void UpdateCameraPosition()
    {
        // Calculate new position based on an elliptical path
        float x = Mathf.Cos(angle) * orbitWidth;
        float z = Mathf.Sin(angle) * orbitHeight;

        // Update camera position while maintaining its height (Y-axis)
        transform.position = new Vector3(x, initialCameraPosition.y, z);

        // Make the camera look at the center of the room (assumed to be at origin)
        transform.LookAt(Vector3.zero);
    }
}
