using System.Collections;
using UnityEngine;

public class lucesPoli : MonoBehaviour
{
    public Light luzRoja;
    public Light luzAzul;
    public float intervalo = 0.3f;

    void Start()
    {
        StartCoroutine(AlternarLuces());
    }

    IEnumerator AlternarLuces()
    {
        while (true)
        {
            luzRoja.enabled = true;
            luzAzul.enabled = false;
            yield return new WaitForSeconds(intervalo);

            luzRoja.enabled = false;
            luzAzul.enabled = true;
            yield return new WaitForSeconds(intervalo);
        }
    }
}
