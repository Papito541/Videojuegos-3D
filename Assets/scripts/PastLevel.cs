using UnityEngine;
using UnityEngine.SceneManagement;

public class PastLevel : MonoBehaviour
{

    // Si usas colisiones tipo "trigger", cambia el método:
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                int escenaActual = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(escenaActual + 1);
        }
    }
}
