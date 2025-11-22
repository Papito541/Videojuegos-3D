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
    public float velocidadMax = 120f;
    public float distanciaFrenado = 7f; 

    // Sistema anti-atasco
    private float tiempoAtascado = 0f;
    private bool modoReversa = false;
    private float tiempoReversa = 1.2f; 
    private float contadorReversa = 0f;
    private bool tocandoObstaculo = false;

    public float velocidadMinimaAtasco = 1f;
    public float tiempoNecesarioAtasco = 1f;

    [Header("Esquiva de obstáculos")]
    public float distanciaRayFrontal = 7f;
    public float distanciaRayDiagonal = 5f;
    public float fuerzaEsquiva = 0.6f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.2f, 0);
    }

    void FixedUpdate()
    {
        if (!target) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        float distancia = dir.magnitude;

        Vector3 localDir = transform.InverseTransformDirection(dir.normalized);

        float steer = Mathf.Clamp(localDir.x, -1f, 1f) * anguloGiro;

        // Añadir esquiva por Raycast
        steer += DetectarObstaculos() * anguloGiro;

        llantaDelanteraIzq.steerAngle = steer;
        llantaDelanteraDer.steerAngle = steer;

        float velocidadActual = rb.linearVelocity.magnitude * 3.6f;

        bool cerca = distancia < distanciaFrenado;

        if (velocidadActual < velocidadMinimaAtasco && tocandoObstaculo)
        {
            tiempoAtascado += Time.fixedDeltaTime;

            if (tiempoAtascado >= tiempoNecesarioAtasco)
            {
                modoReversa = true;
                contadorReversa = tiempoReversa;
            }
        }
        else
        {
            tiempoAtascado = 0f;
        }

        if (modoReversa)
        {
            contadorReversa -= Time.fixedDeltaTime;

            llantaTraseraIzq.motorTorque = -aceleracion * 0.75f;
            llantaTraseraDer.motorTorque = -aceleracion * 0.75f;

            float giroEscape = Random.Range(-1f, 1f) * anguloGiro;
            llantaDelanteraIzq.steerAngle = giroEscape;
            llantaDelanteraDer.steerAngle = giroEscape;

            QuitarFreno();

            if (contadorReversa <= 0)
            {
                modoReversa = false;
                tiempoAtascado = 0;
            }

            return;
        }

        if (!cerca && velocidadActual < velocidadMax)
        {
            llantaTraseraIzq.motorTorque = aceleracion;
            llantaTraseraDer.motorTorque = aceleracion;

            QuitarFreno();
        }
        else
        {
            AplicarFreno(3000f);

            llantaTraseraIzq.motorTorque = 0;
            llantaTraseraDer.motorTorque = 0;
        }
    }

    void OnCollisionStay(Collision col)
    {
        // Ignorar piso
        if (col.collider.CompareTag("Ground")) return;

        // Si toca algo sólido se considera "obstáculo"
        tocandoObstaculo = true;
    }

    void OnCollisionExit(Collision col)
    {
        tocandoObstaculo = false;
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

    float DetectarObstaculos()
    {
        float correccion = 0f;
        RaycastHit hit;

        Vector3 origen = transform.position + Vector3.up * 0.5f;

        // -------- FRONTAL --------
        if (Physics.Raycast(origen, transform.forward, out hit, distanciaRayFrontal))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                correccion += -Mathf.Sign(hit.point.x - transform.position.x) * fuerzaEsquiva;
            }
        }

        // -------- DIAGONAL IZQUIERDA --------
        if (Physics.Raycast(origen, (transform.forward - transform.right * 0.5f).normalized, out hit, distanciaRayDiagonal))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                correccion += fuerzaEsquiva;
            }
        }

        // -------- DIAGONAL DERECHA --------
        if (Physics.Raycast(origen, (transform.forward + transform.right * 0.5f).normalized, out hit, distanciaRayDiagonal))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                correccion -= fuerzaEsquiva;
            }
        }

        return correccion;
    }
}
