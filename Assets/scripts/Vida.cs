using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{

    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;
    public GameObject panelCorazones;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (panelCorazones != null)
            panelCorazones.SetActive(false);
        ActualizarCorazones();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarCorazones();
    }

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {

            corazones[i].sprite = i < Global.vidas ? corazonLleno : corazonVacio;
        }
    }
}
