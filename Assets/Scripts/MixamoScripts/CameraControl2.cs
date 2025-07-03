using UnityEngine;

public class CameraControl2 : MonoBehaviour
{
    public Transform player;
    public float sensitivity = 2f;
    public float maxHeight = 80f;
    public float minHeight = -80f;
    public float distanceFromPlayer = 3f;
    public float transitionSpeed = 5f; // Speed to return behind player

    private float rotationX = 0f;
    private float rotationY = 0f;
    private bool isMoving = false;
    private Rigidbody playerRb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Detect player movement based on velocity
        if (playerRb != null)
        {
            isMoving = playerRb.velocity.magnitude > 0.1f; // Small threshold to prevent jitter
        }

        if (Input.GetMouseButton(1)) // Right mouse button enables free orbit mode
        {
            isMoving = false;
        }

        float moveMouseX = Input.GetAxis("Mouse X") * sensitivity;
        float moveMouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // If the player is NOT moving, allow free orbit
        if (!isMoving)
        {
            rotationY += moveMouseX;
            rotationX -= moveMouseY;
            rotationX = Mathf.Clamp(rotationX, minHeight, maxHeight);
        }
        else
        {
            // When moving, smoothly reset the camera behind the player
            rotationY = Mathf.Lerp(rotationY, player.eulerAngles.y, Time.deltaTime * transitionSpeed);
            rotationX = Mathf.Lerp(rotationX, 15f, Time.deltaTime * transitionSpeed); // Slight downward angle
        }

        // Calculate camera position
        Vector3 direction = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.position = player.position + rotation * direction;

        // Always look at the player
        print(isMoving);
        transform.LookAt(player.position);
    }
}
