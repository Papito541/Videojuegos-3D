using UnityEngine;

public class AICarPersecution : MonoBehaviour
{
    public Transform target;     // Jugador
    public WheelCollider llantaDelanteraIzq;
    public WheelCollider llantaDelanteraDer;
    public WheelCollider llantaTraseraIzq;
    public WheelCollider llantaTraseraDer;

    public float aceleracion = 1200f;
    public float anguloGiro = 20f;
    public float velocidadMax = 120f;   // km/h
    public float distanciaFrenado = 7f; // distancia m�nima antes de frenar

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.9f, 0);
    }

    void FixedUpdate()
    {
        if (!target) return;

        // Direcci�n hacia el jugador
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        float distancia = dir.magnitude;

        // Convertir direcci�n a espacio local
        Vector3 localDir = transform.InverseTransformDirection(dir.normalized);

        // ===== GIRAR EL CARRO HACIA EL JUGADOR =====
        float steer = Mathf.Clamp(localDir.x, -1f, 1f) * anguloGiro;
        llantaDelanteraIzq.steerAngle = steer;
        llantaDelanteraDer.steerAngle = steer;

        // ===== CALCULAR VELOCIDAD REAL =====
        float velocidadActual = rb.linearVelocity.magnitude * 3.6f; // m/s a km/h

        // ===== LOGICA DE ACELERACI�N =====
        bool cerca = distancia < distanciaFrenado;

        if (!cerca && velocidadActual < velocidadMax)
        {
            // Acelerar hacia adelante
            llantaTraseraIzq.motorTorque = aceleracion;
            llantaTraseraDer.motorTorque = aceleracion;

            // Quitar freno
            QuitarFreno();
        }
        else
        {
            // Frenar cuando est� cerca o muy r�pido
            AplicarFreno(3000f);

            // NO acelerar
            llantaTraseraIzq.motorTorque = 0;
            llantaTraseraDer.motorTorque = 0;
        }
    }

    void AplicarFreno(float fuerza)
    {
        llantaDelanteraIzq.brakeTorque = fuerza;
        llantaDelanteraDer.brakeTorque = fuerza;
        llantaTraseraIzq.brakeTorque = fuerza;
        llantaTraseraDer.brakeTorque = fuerza;
    }

    void QuitarFreno()
    {
        llantaDelanteraIzq.brakeTorque = 0;
        llantaDelanteraDer.brakeTorque = 0;
        llantaTraseraIzq.brakeTorque = 0;
        llantaTraseraDer.brakeTorque = 0;
    }
}
