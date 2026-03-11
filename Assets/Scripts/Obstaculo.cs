using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    public enum TipoObstaculo { Transparente, Solido, Robot, ChoqueFrontal }

    [Header("Configuración")]
    public TipoObstaculo tipo;
    public float dano = 20f;

    [Header("Efectos (Solo para el Robot)")]
    public GameObject particulasExplosion;

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            PlayerHealth vidaJugador = otro.GetComponent<PlayerHealth>();

            if (vidaJugador != null)
            {
                if (tipo == TipoObstaculo.Transparente)
                {
                    vidaJugador.RecibirDano(dano);
                    GetComponent<Collider>().enabled = false;
                }
                else if (tipo == TipoObstaculo.Robot)
                {
                    vidaJugador.RecibirDano(dano);

                    if (particulasExplosion != null)
                    {
                        Instantiate(particulasExplosion, transform.position, Quaternion.identity);
                    }

                    Destroy(gameObject);
                }
                else if (tipo == TipoObstaculo.ChoqueFrontal)
                {
                    vidaJugador.RecibirDano(vidaJugador.vidaMaxima);
                }
            }
        }
    }
}
