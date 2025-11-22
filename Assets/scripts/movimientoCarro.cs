using UnityEngine;

public class movimientoCarro : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider llantaDelanteraIzq;
    public WheelCollider llantaDelanteraDer;
    public WheelCollider llantaTraseraIzq;
    public WheelCollider llantaTraseraDer;

    [Header("Ruedas visuales")]
    public Transform neumaticoFL;
    public Transform neumaticoFR;
    public Transform neumaticoRL;
    public Transform neumaticoRR;

    [Header("Parametros")]
    public float aceleracion = 1500f;
    public float freno = 2000f;
    public float velocidadMax = 200f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.6f, 0);

        AjustarFriccion(llantaDelanteraIzq);
        AjustarFriccion(llantaDelanteraDer);
        AjustarFriccion(llantaTraseraIzq);
        AjustarFriccion(llantaTraseraDer);

        ConfigurarSuspension(llantaDelanteraIzq);
        ConfigurarSuspension(llantaDelanteraDer);
        ConfigurarSuspension(llantaTraseraIzq);
        ConfigurarSuspension(llantaTraseraDer);
    }

    void Update()
    {
        ActualizarRuedas();
    }

    void FixedUpdate()
    {
        bool enSuelo =
            llantaDelanteraIzq.isGrounded ||
            llantaDelanteraDer.isGrounded ||
            llantaTraseraIzq.isGrounded ||
            llantaTraseraDer.isGrounded;
        float acelerador = Input.GetAxis("Vertical");
        float giro = Input.GetAxis("Horizontal");
        bool frenando = Input.GetKey(KeyCode.Space);
        float velocidad = rb.linearVelocity.magnitude;
        float anguloGiro = Mathf.Lerp(25f, 8f, velocidad / 50f);
        float velocidadKmh = rb.linearVelocity.magnitude * 3.6f;

        if (enSuelo)
        {
            // motor
            if (velocidadKmh < velocidadMax)
            {
                llantaTraseraIzq.motorTorque = aceleracion * acelerador;
                llantaTraseraDer.motorTorque = aceleracion * acelerador;
            }
            else
            {
                llantaTraseraIzq.motorTorque = 0;
                llantaTraseraDer.motorTorque = 0;
            }

            llantaDelanteraIzq.steerAngle = anguloGiro * giro;
            llantaDelanteraDer.steerAngle = anguloGiro * giro;
        }
        else
        {
            llantaTraseraIzq.motorTorque = 0;
            llantaTraseraDer.motorTorque = 0;
            llantaDelanteraIzq.steerAngle = 0;
            llantaDelanteraDer.steerAngle = 0;
        }

        if (frenando || acelerador == 0)
        {
            AplicarFreno(freno);
        }
        else
        {
            QuitarFreno();
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

    void ActualizarRuedas()
    {
        ActualizarRuedaVisual(llantaDelanteraIzq, neumaticoFL);
        ActualizarRuedaVisual(llantaDelanteraDer, neumaticoFR);
        ActualizarRuedaVisual(llantaTraseraIzq, neumaticoRL);
        ActualizarRuedaVisual(llantaTraseraDer, neumaticoRR);
    }

    void ActualizarRuedaVisual(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);

        mesh.position = pos;
        mesh.rotation = rot;
    }

    void AjustarFriccion(WheelCollider wheel)
    {
        WheelFrictionCurve sideways = wheel.sidewaysFriction;
        sideways.stiffness = 2.0f;
        wheel.sidewaysFriction = sideways;

        WheelFrictionCurve forward = wheel.forwardFriction;
        forward.stiffness = 1.5f;
        wheel.forwardFriction = forward;
    }

    void ConfigurarSuspension(WheelCollider wheel)
    {
        JointSpring spring = wheel.suspensionSpring;

        spring.spring = 35000f; 
        spring.damper = 4500f;   
        spring.targetPosition = 0.5f;

        wheel.suspensionSpring = spring;

        wheel.suspensionDistance = 0.25f; 
    }
}
