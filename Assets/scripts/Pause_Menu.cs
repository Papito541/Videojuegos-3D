using UnityEngine;

public class nextLevel : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        // Si se presiona la tecla P, alterna el estado de pausa
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    // Este método se puede llamar también desde un botón UI
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pausa el juego
            AudioListener.pause = true; // Pausa todos los sonidos
            Debug.Log("Juego pausado (audio y tiempo detenidos)");
            Debug.Log("Juego pausado");
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el juego
            AudioListener.pause = false; // Reanuda todos los sonidos
            Debug.Log("Juego reanudado");
        }
    }
}
