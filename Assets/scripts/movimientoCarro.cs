using UnityEngine;

public class movimientoCarro : MonoBehaviour
{
    public WheelCollider llantaDelanteraIzq;
    public WheelCollider llantaDelanteraDer;
    public WheelCollider llantaTraseraIzq;
    public WheelCollider llantaTraseraDer;
    public Transform neumatico1;
    public Transform neumatico2;

    public float aceleracion = 1200f;
    public float velocidad;
    public float velocidadMax = 150f; // km/h
    public float anguloGiro = 25f;
    public float fuerzaFreno = 3000f;

    private bool frenando;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.9f, 0);
    }

    void Update()
    {
        // Freno manual
        frenando = Input.GetKey(KeyCode.Space);

        // Calcular velocidad actual
        velocidad = rb.linearVelocity.magnitude * 3.6f; // m/s a km/h
        velocidad = Mathf.Round(velocidad);

        // Girar visualmente las llantas delanteras
        neumatico1.localEulerAngles = new Vector3(0, llantaDelanteraDer.steerAngle * 2, 0);
        neumatico2.localEulerAngles = new Vector3(0, llantaDelanteraIzq.steerAngle * 2, 0);
    }

    void FixedUpdate()
    {
        float movimiento = Input.GetAxis("Vertical");
        float giro = Input.GetAxis("Horizontal");

        // Dirección
        llantaDelanteraIzq.steerAngle = anguloGiro * giro;
        llantaDelanteraDer.steerAngle = anguloGiro * giro;

        // Freno automático o manual
        if (frenando || Mathf.Abs(movimiento) < 0.1f)
        {
            AplicarFreno(fuerzaFreno);
            QuitarAceleracion();
        }
        else
        {
            QuitarFreno();

            // --- Límite de velocidad real ---
            if (velocidad < velocidadMax)
            {
                llantaTraseraIzq.motorTorque = aceleracion * movimiento;
                llantaTraseraDer.motorTorque = aceleracion * movimiento;
            }
            else
            {
                QuitarAceleracion();

                // Si la velocidad supera el límite, reducirla poco a poco
                if (rb.linearVelocity.magnitude * 3.6f > velocidadMax)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * (velocidadMax / 3.6f);
                }
            }
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

    void QuitarAceleracion()
    {
        llantaTraseraIzq.motorTorque = 0;
        llantaTraseraDer.motorTorque = 0;
    }
}
