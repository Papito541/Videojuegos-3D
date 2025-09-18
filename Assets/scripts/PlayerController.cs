using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidadWalk = 2;
    public float velocidadRun = 7;
    public float velocidadCrouch = 3;
    public float rotacionSpeedWalk = 100;
    public float rotacionSpeedRun = 200;

    public Animator animator;

    private float x, y;
    private bool enSuelo;
    private bool agachado;

    public Rigidbody rb;
    public float saltoHeigth = 1;

    public Transform sueloCheck;
    public float sueloDistant = 0.1f;
    public LayerMask sueloMask;

    private float velocidadActual;
    private float rotacionActual;

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        enSuelo = Physics.CheckSphere(sueloCheck.position, sueloDistant, sueloMask);

        if (Input.GetKeyDown(KeyCode.C))
        {
            agachado = !agachado;
            animator.SetBool("agachado", agachado);
        }

        bool corriendo = !agachado && Input.GetKey(KeyCode.LeftControl) && y > 0;

        // Velocidad actual
        velocidadActual = agachado ? velocidadCrouch : (corriendo ? velocidadRun : velocidadWalk);
        rotacionActual = corriendo ? rotacionSpeedRun : rotacionSpeedWalk;

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo && !agachado)
        {
            animator.SetTrigger("saltar");
            Invoke("Saltar", 0.3f);
        }

        // Animaciones
        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);
        animator.SetBool("correr", corriendo);
        animator.SetBool("enSuelo", enSuelo);
    }

    void FixedUpdate()
    {
        // Movimiento físico
        Vector3 movimiento = transform.forward * y * velocidadActual * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimiento);

        // Rotación física
        Quaternion rot = Quaternion.Euler(0, x * rotacionActual * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * rot);
    }

    void Saltar()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(saltoHeigth * -2f * Physics.gravity.y), ForceMode.Impulse);
    }
}