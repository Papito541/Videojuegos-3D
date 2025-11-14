using UnityEngine;

public class TransitionPlayer : MonoBehaviour
{
    public GameObject player;         // Jugador que se va a desactivar
    public Camera playerCamera;       // Cámara del jugador
    public GameObject carCamera;          // Cámara del carro
    public MonoBehaviour carController;  // Script que controla el carro

    void Start()
    {
        carCamera.SetActive(false);
        carController.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apagar jugador
            player.SetActive(false);

            // Cambiar cámara
            playerCamera.enabled = false;
            carCamera.SetActive(true);

            // Activar control del carro
            carController.enabled = true;
        }
    }
}

