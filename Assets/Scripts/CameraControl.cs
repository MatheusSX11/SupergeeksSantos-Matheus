using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // The target object to orbit around.
    public Transform target;

    // Distance from the target.
    public float distance = 10.0f;
    
    // Rotation speeds for mouse movement.
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    
    // Limits for vertical rotation.
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    
    // Zoom limits.
    public float distanceMin = 5f;
    public float distanceMax = 20f;
    
    private float x = 0.0f;
    private float y = 0.0f;
    private bool isCursorLocked = true;

    void Start()
    {
        
        // Initialize the angles based on the current rotation.
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        LockCursor();

        // If there is a rigidbody, prevent it from rotating.
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update()
    {
        // Toggle cursor lock state with the Escape key.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = !isCursorLocked;
            if (isCursorLocked)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            // Update the rotation angles based on mouse input.
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // Create the rotation.
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            // Adjust the distance based on the scroll wheel.
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            
            // Calculate the new position.
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            // Apply the rotation and position to the camera.
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Clamp the vertical angle to prevent flipping.
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}