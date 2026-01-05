using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [Tooltip("How much the gun moves sideways/up-down")]
    public float amount = 0.05f;

    [Tooltip("Max distance the gun can travel")]
    public float maxAmount = 0.15f;

    [Tooltip("How much the gun tilts (rotation)")]
    public float rotationAmount = 4f;

    [Tooltip("Max tilt angle")]
    public float maxRotationAmount = 5f;

    [Tooltip("Lower value = Heavier weapon (Slower return)")]
    public float smoothAmount = 3f;

    [Header("Position")]
    public bool swayPosition = true;
    public bool swayRotation = true;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        // Remember where the gun started
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        // 1. Get Mouse Input
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        float rotationX = -Input.GetAxis("Mouse Y") * rotationAmount;
        float rotationY = Input.GetAxis("Mouse X") * rotationAmount;

        // 2. Clamp (Limit) the movement so the gun doesn't fly off screen
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        rotationX = Mathf.Clamp(rotationX, -maxRotationAmount, maxRotationAmount);
        rotationY = Mathf.Clamp(rotationY, -maxRotationAmount, maxRotationAmount);

        // 3. Calculate Target Position & Rotation
        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        Quaternion finalRotation = Quaternion.Euler(rotationX, rotationY, 0);

        // 4. Apply Sway (Lerp = Smoothly move from A to B)
        if (swayPosition)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                finalPosition + initialPosition,
                Time.deltaTime * smoothAmount
            );
        }

        if (swayRotation)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                finalRotation * initialRotation,
                Time.deltaTime * smoothAmount
            );
        }
    }
}