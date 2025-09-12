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

    // Update is called once per frame
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
        float velocidadActual = agachado ? velocidadCrouch : (corriendo ? velocidadRun : velocidadWalk);
        float RotacionActual = corriendo ? rotacionSpeedRun : rotacionSpeedWalk;

        // Movimiento hacia adelante/atrás
        transform.Translate(0, 0, y * Time.deltaTime * velocidadActual);
        transform.Rotate(0, x * Time.deltaTime * RotacionActual, 0);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo && !agachado)
        {
            animator.SetTrigger("saltar");
            Invoke("Saltar", 0.3f); // pequeño delay opcional
        }

        // Animaciones
        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);
        animator.SetBool("correr", corriendo);
        animator.SetBool("enSuelo", enSuelo);
    }
    void Saltar()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(saltoHeigth * -2f * Physics.gravity.y), ForceMode.Impulse);
    }
}   
