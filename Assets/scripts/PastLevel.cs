using UnityEngine;
using UnityEngine.SceneManagement;

public class PastLevel : MonoBehaviour
{

    // Si usas colisiones tipo "trigger", cambia el método:
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Jugador ha muerto");
            int escenaActual = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(escenaActual + 1);
        }
    }
}
