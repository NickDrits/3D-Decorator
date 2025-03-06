using UnityEngine;

public class FurnitureMovement : MonoBehaviour
{
    public float moveSpeed = 0.01f; // Speed of movement
    public float rotateSpeed = 0.5f; // Speed of rotation
    public float doubleTapTime = 0.3f; // Time threshold for detecting double-tap

    private Vector2 lastTouchPosition;
    private Vector2[] lastTwoFingerPositions = new Vector2[2];
    private bool isMoving = false;
    private bool isRotating = false;
    private Camera mainCamera;
    private float lastTapTime;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            HandleSingleTouch();
        }
        else if (Input.touchCount == 2)
        {
            HandleDoubleTouch();
        }
        else
        {
            isMoving = false;
            isRotating = false;
        }
    }

    void HandleSingleTouch()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (IsTouchingObject(touch.position))
                {
                    CameraMovement.CanMove = false; // Disable camera movement while moving furniture
                    isMoving = true;
                    lastTouchPosition = touch.position;

                    // Check for double-tap to show delete confirmation
                    if (Time.time - lastTapTime < doubleTapTime)
                    {
                        ShowDeleteConfirmation();
                    }

                    lastTapTime = Time.time;
                }
                break;

            case TouchPhase.Moved:
                if (isMoving)
                {
                    Vector2 deltaPosition = touch.position - lastTouchPosition;

                    // Calculate movement directions based on camera's forward and right vectors
                    Vector3 forward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
                    Vector3 right = mainCamera.transform.right;

                    // Move the furniture in the direction of the swipe relative to the camera's orientation
                    Vector3 moveDirection = (right * deltaPosition.x + forward * deltaPosition.y) * moveSpeed;
                    transform.Translate(moveDirection, Space.World);

                    lastTouchPosition = touch.position; // Update the last touch position
                }
                break;

            case TouchPhase.Ended:
                CameraMovement.CanMove = true; // Re-enable camera movement
                isMoving = false;
                break;
        }
    }

    void HandleDoubleTouch()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            CameraMovement.CanMove = false; // Disable camera movement while rotating furniture

            if (IsTouchingObject(touch0.position))
            {
                isRotating = true;
                lastTwoFingerPositions[0] = touch0.position;
                lastTwoFingerPositions[1] = touch1.position;
            }
        }
        else if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved && isRotating)
        {
            Vector2 currentVector = touch1.position - touch0.position;
            Vector2 previousVector = lastTwoFingerPositions[1] - lastTwoFingerPositions[0];

            float angle = Vector2.SignedAngle(previousVector, currentVector);

            if (Mathf.Abs(angle) > 0.1f)
            {
                transform.Rotate(Vector3.up, -angle * rotateSpeed); // Rotate furniture around Y-axis
            }

            lastTwoFingerPositions[0] = touch0.position;
            lastTwoFingerPositions[1] = touch1.position;
        }
        else if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
        {
            CameraMovement.CanMove = true; // Re-enable camera movement
            isRotating = false;
        }
    }

    bool IsTouchingObject(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == this.transform; // Check if the raycast hit this object
        }

        return false;
    }

    void ShowDeleteConfirmation()
    {
        FurnitureManager.Instance.ShowDeleteConfirmation(this);
    }
}
