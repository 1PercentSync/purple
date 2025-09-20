using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;   // assign the Player root

    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float minPitch = -46.8f; // can't look lower than this
    public float maxPitch = 80f;    // can look up

    [Header("Camera Settings")]
    public float fov = 70f;

    private float pitch = 0f;

    void Start()
    {
        if (playerBody == null)
        {
            Debug.LogError("PlayerBody not assigned in FollowPlayer!");
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Camera cam = GetComponent<Camera>();
        if (cam != null)
            cam.fieldOfView = fov;
        else
            Debug.LogWarning("No Camera component found on FollowPlayer object!");
    }

    void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // rotate player horizontally
        playerBody.Rotate(Vector3.up * mouseX);

        // rotate camera vertically
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // clamp the pitch
        transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
