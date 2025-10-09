using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public Slider barraProgreso;
    public TMP_Text textoProgreso;

    void Start()
    {
        string escenaDestino = PlayerPrefs.GetString("EscenaDestino", "Nivel1");
        StartCoroutine(CargarEscenaAsync(escenaDestino));
    }

    IEnumerator CargarEscenaAsync(string escenaDestino)
    {
        yield return null;

        AsyncOperation operacion = SceneManager.LoadSceneAsync(escenaDestino);
        operacion.allowSceneActivation = false;

        float progresoMostrado = 0f;

        while (!operacion.isDone)
        {
            float progresoReal = Mathf.Clamp01(operacion.progress / 0.9f);

            progresoMostrado = Mathf.MoveTowards(progresoMostrado, progresoReal, Time.deltaTime);

            if (barraProgreso != null)
                barraProgreso.value = progresoMostrado;

            if (textoProgreso != null)
                textoProgreso.text = (progresoMostrado * 100f).ToString("F0") + "%";

            if (progresoMostrado >= 1f && operacion.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operacion.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
