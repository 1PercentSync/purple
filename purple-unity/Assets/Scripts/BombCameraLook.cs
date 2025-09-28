using UnityEngine;

public class BombCameraLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float minPitch = -45f;
    public float maxPitch = 45f;

    private float pitch = 0f;
    private float yaw = 0f;

    private Quaternion initialRotation;

    void Start()
    {
        // Save initial rotation from the scene
        initialRotation = transform.rotation;
        Vector3 euler = initialRotation.eulerAngles;
        pitch = euler.x;
        yaw = euler.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Apply rotation
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
