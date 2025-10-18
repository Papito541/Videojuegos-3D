using UnityEngine;

public class LuzCrontoller : MonoBehaviour
{
    public Transform player; // arrastra aquí tu jugador o cámara
    public float maxDistance = 30f; // distancia máxima para luz completa
    public float minDistance = 5f; // distancia donde empieza a atenuarse

    private Light[] allLights;

    void Start()
    {
        // Busca todas las luces dentro de este objeto (LIGHTING)
        allLights = GetComponentsInChildren<Light>();

        // Si no se asignó un jugador, usa la cámara principal
        if (player == null && Camera.main != null)
        {
            player = Camera.main.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        foreach (Light light in allLights)
        {
            float distance = Vector3.Distance(player.position, light.transform.position);

            if (distance > maxDistance)
            {
                // Muy lejos: apaga sombras y baja intensidad
                light.shadows = LightShadows.None;
                light.intensity = Mathf.Lerp(light.intensity, 0.2f, Time.deltaTime * 2f);
            }
            else if (distance > minDistance)
            {
                // Media distancia: baja un poco intensidad y usa sombras suaves
                light.shadows = LightShadows.Soft;
                light.intensity = Mathf.Lerp(light.intensity, 0.6f, Time.deltaTime * 2f);
            }
            else
            {
                // Cercano: brillo completo y sombras duras
                light.shadows = LightShadows.Hard;
                light.intensity = Mathf.Lerp(light.intensity, 1f, Time.deltaTime * 2f);
            }
        }
    }
}
