using UnityEngine;

public class ControllerMouse : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Bloquea el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Leer movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotar cámara verticalmente (arriba/abajo)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // evita girar completamente

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotar cuerpo del jugador horizontalmente
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
