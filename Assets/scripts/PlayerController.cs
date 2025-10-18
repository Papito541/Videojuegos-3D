using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float velocidadWalk = 2;
    public float velocidadRun = 7;
    public float velocidadCrouch = 3;
    public float rotacionSpeedWalk = 60;
    public float rotacionSpeedRun = 120;
    public Animator animator;

    private float x, y;
    private bool enSuelo;
    private bool agachado;
    CapsuleCollider col;

    public Rigidbody rb;
    public float saltoHeigth = 1;
    public Transform sueloCheck;
    public float sueloDistant = 0.5f;
    public LayerMask sueloMask;
    private float velocidadActual;
    private float rotacionActual;

    public int vidaMaxima = 3;
    private int vidaActual;

    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    public bool invulnerable = false;  
    public float tiempoInvulnerable = 1f;
    public GameObject panelCorazones;

    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        if (panelCorazones != null)
            panelCorazones.SetActive(false);

        vidaActual = vidaMaxima;
        ActualizarCorazones();
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
        Vector3 movimiento = transform.forward * y * velocidadActual * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimiento);

        Quaternion rot = Quaternion.Euler(0, x * rotacionActual * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * rot);
    }

    void Saltar()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(saltoHeigth * -2f * Physics.gravity.y), ForceMode.Impulse);
    }

    public void RecibirDaño()
    {
        if (invulnerable) return; // No recibe daño si está invulnerable

        vidaActual--;
        if (vidaActual < 0) vidaActual = 0;

        ActualizarCorazones();

        if (vidaActual == 0)
        {
            Morir();
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

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].sprite = i < vidaActual ? corazonLleno : corazonVacio;
        }
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
