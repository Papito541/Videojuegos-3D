using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float height = 5f;
    public float heightDamping = 2f;
    public float rotationDamping = 3f;

    void LateUpdate()
    {
        if (!target) return;

        // Ángulo y altura deseados
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        // Ángulo y altura actuales
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Suavizado en rotación
        currentRotationAngle = Mathf.LerpAngle(
            currentRotationAngle,
            wantedRotationAngle,
            rotationDamping * Time.deltaTime
        );

        // Suavizado en altura
        currentHeight = Mathf.Lerp(
            currentHeight,
            wantedHeight,
            heightDamping * Time.deltaTime
        );

        // Convertimos ángulo en rotación
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Nueva posición
        Vector3 pos = target.position - currentRotation * Vector3.forward * distance;
        pos.y = currentHeight;

        transform.position = pos;

        // Mirar al objetivo
        transform.LookAt(target);
    }
}
