using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class policiasControl : MonoBehaviour
{
    public Transform[] waypoints;
    public float tiempoEspera = 2f;
    public float rangoVision = 10f;
    public float anguloVision = 60f;
    public Transform jugador;
    public Transform puntoInicioJugador;

    private NavMeshAgent agente;
    private Animator anim;
    private int destinoActual = 0;
    private bool esperando = false;
    private bool persiguiendo = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        IniciarPatrulla();
    }

    void Update()
    {
        if (persiguiendo)
        {
            // Continuar persiguiendo solo si el jugador sigue dentro del rango y ángulo
            if (JugadorVisible())
            {
                agente.SetDestination(jugador.position);
                anim.SetFloat("velocidad", agente.velocity.magnitude);
            }
            else
            {
                // Si el jugador se escapa, volver a patrulla
                persiguiendo = false;
                IniciarPatrulla();
            }
        }
        else
        {
            DetectarJugador();

            if (!esperando && !agente.pathPending && agente.remainingDistance < 0.5f)
            {
                StartCoroutine(EsperarEnPunto());
            }

            anim.SetFloat("velocidad", agente.velocity.magnitude);
        }
    }

    void DetectarJugador()
    {
        if (JugadorVisible())
        {
            persiguiendo = true;
        }
    }

    bool JugadorVisible()
    {
        Vector3 direccion = (jugador.position - transform.position).normalized;
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia < rangoVision)
        {
            float angulo = Vector3.Angle(transform.forward, direccion);
            if (angulo < anguloVision / 2f)
            {
                if (Physics.Raycast(transform.position + Vector3.up, direccion, out RaycastHit hit, rangoVision))
                {
                    if (hit.transform.CompareTag("Player"))
                        return true;
                }
            }
        }
        return false;
    }

    IEnumerator EsperarEnPunto()
    {
        esperando = true;
        agente.isStopped = true;
        anim.SetFloat("velocidad", 0);
        yield return new WaitForSeconds(tiempoEspera);

        destinoActual = (destinoActual + 1) % waypoints.Length;
        agente.SetDestination(waypoints[destinoActual].position);
        agente.isStopped = false;
        esperando = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController jugadorScript = collision.gameObject.GetComponent<PlayerController>();
            if (jugadorScript != null)
            {
                jugadorScript.RecibirDaño();
                collision.gameObject.transform.position = puntoInicioJugador.position;

                // Volver a patrullar después del choque
                persiguiendo = false;
                IniciarPatrulla();
            }
        }
    }

    void IniciarPatrulla()
    {
        agente.isStopped = false;
        agente.SetDestination(waypoints[destinoActual].position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);

        Vector3 anguloDerecha = Quaternion.Euler(0, anguloVision / 2, 0) * transform.forward;
        Vector3 anguloIzquierda = Quaternion.Euler(0, -anguloVision / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + anguloDerecha * rangoVision);
        Gizmos.DrawLine(transform.position, transform.position + anguloIzquierda * rangoVision);
    }
}
