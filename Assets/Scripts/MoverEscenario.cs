using UnityEngine;

public class MoverEscenario : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadMundo = 10f;

    void Update()
    {
        if (GameManager.Instancia != null && GameManager.Instancia.juegoIniciado)
        {
            transform.Translate(0, 0, -velocidadMundo * Time.deltaTime, Space.World);
        }
    }
}
