using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidadWalk = 2;
    public float velocidadRun = 7;
    public float rotacionSpeedWalk = 100;
    public float rotacionSpeedRun = 200;

    public Animator animator;

    private float x, y;

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // ¿Está corriendo?
        bool corriendo = Input.GetKey(KeyCode.LeftControl) && y > 0;

        // Velocidad actual
        float velocidadActual = corriendo ? velocidadRun : velocidadWalk;
        float RotacionActual = corriendo ? rotacionSpeedRun : rotacionSpeedWalk;

        // Movimiento hacia adelante/atrás
        transform.Translate(0, 0, y * Time.deltaTime * velocidadActual);
        transform.Rotate(0, x * Time.deltaTime * RotacionActual, 0);

        // Animaciones
        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);
        animator.SetBool("correr", corriendo);
    }
}   
