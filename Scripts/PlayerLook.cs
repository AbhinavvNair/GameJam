using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform PlayerCamera;

    [Header("Settings")]
    public Vector2 Sensitivity = new Vector2(2.0f, 2.0f);

    // Private variables to keep track of rotation
    private Vector2 rotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Hide the cursor
    }

    void Update()
    {
        // USE GetAxisRaw for "Boomer Shooter" snappy aim
        Vector2 mouseInput = new Vector2
        {
            x = Input.GetAxisRaw("Mouse X"),
            y = Input.GetAxisRaw("Mouse Y")
        };

        // Calculate rotation
        // We multiply by settings, but NOT by Time.deltaTime 
        // (Mouse movement is already framerate independent in Unity)
        rotation.x -= mouseInput.y * Sensitivity.y;
        rotation.y += mouseInput.x * Sensitivity.x;

        // Clamp looking up/down
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        // APPLY ROTATION

        // 1. Rotate Camera Up/Down (Pitch)
        PlayerCamera.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);

        // 2. Rotate Player Body Left/Right (Yaw)
        // This turns the actual collider so 'W' matches where you look
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}



