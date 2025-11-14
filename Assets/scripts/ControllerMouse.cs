using UnityEngine;

public class ControllerMouse : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensX = 100f;
    public float sensY = 100f;

    [Header("Suavizado")]
    public float smoothTime = 0.05f;

    public Transform playerBody;

    float xRotation;
    float smoothX, smoothY;
    float currentX, currentY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Lectura del mouse (raw = más preciso)
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        // Suavizado
        smoothX = Mathf.Lerp(smoothX, mouseX, smoothTime);
        smoothY = Mathf.Lerp(smoothY, mouseY, smoothTime);

        // Rotación vertical
        xRotation -= smoothY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal del cuerpo
        playerBody.Rotate(Vector3.up * smoothX);
    }
}

