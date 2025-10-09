using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject[] tutorialTexts;
    public GameObject tutorialContainer;
    public GameObject panelCorazones;
    private int pasoActual = 0;

    private bool moved = false;
    private bool running = false;
    private bool crouched = false;

    private bool pressedW = false;
    private bool pressedA = false;
    private bool pressedS = false;
    private bool pressedD = false;

    void Start()
    {
        ShowTexts(0);
    }

    void Update()
    {
        if (pasoActual == 0) // Paso de movimiento
        {
            if (Input.GetKeyDown(KeyCode.W)) pressedW = true;
            if (Input.GetKeyDown(KeyCode.A)) pressedA = true;
            if (Input.GetKeyDown(KeyCode.S)) pressedS = true;
            if (Input.GetKeyDown(KeyCode.D)) pressedD = true;

            if (pressedW && pressedA && pressedS && pressedD)
            {
                moved = true;
                SiguientePaso();
            }
        }
        else if (pasoActual == 1) // Paso de correr
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                running = true;
                SiguientePaso();
            }
        }
        else if (pasoActual == 2) // Paso de agacharse
        {
            if (Input.GetKey(KeyCode.C))
            {
                crouched = true;
                SiguientePaso();
            }
        }
    }

    void ShowTexts(int index)
    {
        for (int i = 0; i < tutorialTexts.Length; i++)
        {
            tutorialTexts[i].SetActive(i == index);
        }
    }

    void SiguientePaso()
    {
        tutorialTexts[pasoActual].SetActive(false);
        pasoActual++;

        if (pasoActual < tutorialTexts.Length)
        {
            ShowTexts(pasoActual);
        }
        else
        {
            // Tutorial terminado
            if (tutorialContainer != null)
                tutorialContainer.SetActive(false);

            if (panelCorazones != null)
                panelCorazones.SetActive(true); // Mostrar los corazones
        }
    }
}
