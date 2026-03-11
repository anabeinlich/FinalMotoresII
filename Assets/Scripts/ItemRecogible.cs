using UnityEngine;

public class ItemRecogible : MonoBehaviour
{
    public enum TipoItem { Moneda, Cura, PowerUpEscudo, PowerUpArma }
    public TipoItem tipo;

    [Header("Valores")]
    public int valorMoneda = 1;
    public float curacion = 25f;

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            if (tipo == TipoItem.Moneda)
            {
                GameManager.Instancia.SumarMoneda(valorMoneda);
            }
            else if (tipo == TipoItem.Cura)
            {
                PlayerHealth salud = otro.GetComponent<PlayerHealth>();
                if (salud != null) salud.Curar(curacion);
            }
            else if (tipo == TipoItem.PowerUpEscudo || tipo == TipoItem.PowerUpArma)
            {
                PlayerPowerUps powerups = otro.GetComponent<PlayerPowerUps>();
                if (powerups != null)
                {
                    if (tipo == TipoItem.PowerUpEscudo) powerups.RecogerPowerUp("Escudo");
                    if (tipo == TipoItem.PowerUpArma) powerups.RecogerPowerUp("Arma");
                }
            }

            Destroy(gameObject);
        }
    }
}
