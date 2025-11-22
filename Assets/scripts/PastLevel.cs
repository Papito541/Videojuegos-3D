using UnityEngine;
using UnityEngine.SceneManagement;

public class PastLevel : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerCar"))
        {
            Debug.Log("Jugador ha muerto");
            int escenaActual = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(escenaActual + 1);
        }
    }
}
