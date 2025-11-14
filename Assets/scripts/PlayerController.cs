using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float velocidadWalk = 2;
    public float velocidadRun = 7;
    public float velocidadCrouch = 3;
    public float rotacionSpeedWalk = 60;
    public float rotacionSpeedRun = 120;
    public Animator animator;
    public Transform playerBody;

    private float x, y;
    private bool enSuelo;
    private bool agachado;
    CapsuleCollider col;

    public int vidaMaxima = 3;
    private int vidaActual;

    public Rigidbody rb;
    public float saltoHeigth = 1;
    public Transform sueloCheck;
    public float sueloDistant = 0.5f;
    public LayerMask sueloMask;
    private float velocidadActual;
    private float rotacionActual;

    public bool invulnerable = false;  
    public float tiempoInvulnerable = 1f;

    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        vidaActual = vidaMaxima;
    }

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
        velocidadActual = agachado ? velocidadCrouch : (corriendo ? velocidadRun : velocidadWalk);
        rotacionActual = corriendo ? rotacionSpeedRun : rotacionSpeedWalk;

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo && !agachado)
        {
            animator.SetTrigger("saltar");
            Invoke("Saltar", 0.4f);
        }

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);
        animator.SetBool("correr", corriendo);
        animator.SetBool("enSuelo", enSuelo);

    }

    void FixedUpdate()
    {
        Vector3 direccion = playerBody.forward * y + playerBody.right * x;

        // Evitar que corra más rápido en diagonal
        direccion.Normalize();

        // Movimiento usando Rigidbody
        Vector3 movimiento = direccion * velocidadActual * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimiento);
    }

    void Saltar()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(saltoHeigth * -2f * Physics.gravity.y), ForceMode.Impulse);
    }

    public void RecibirDaño()
    {
        if (invulnerable) return; // No recibe daño si está invulnerable

        Global.vidas -=1;
        if (Global.vidas < 0)
            Global.vidas = 0;

        if (Global.vidas == 0)
        {
            Debug.Log("Jugador muerto");
            // Aquí llamas a GameOver si quieres
        }
        else
        {
            StartCoroutine(ActivarInvulnerabilidad());
        }
    }

    IEnumerator ActivarInvulnerabilidad()
    {
        invulnerable = true;
        yield return new WaitForSeconds(tiempoInvulnerable);
        invulnerable = false;
    }

    void Morir()
    {
        Debug.Log("Jugador ha muerto");
        // Aquí podrías reiniciar el nivel o mostrar game over
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Policia"))
        {
            RecibirDaño();
        }
    }
}
