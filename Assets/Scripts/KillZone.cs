using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            Debug.Log("jugador entro a killzone");
            PlayerHealth vidaJugador = otro.GetComponent<PlayerHealth>();

            if (vidaJugador != null)
            {
                vidaJugador.RecibirDano(vidaJugador.vidaMaxima);
            }
        }
    }
}
