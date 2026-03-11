using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 20f;
    public float tiempoVida = 2f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider otro)
    {
        Obstaculo obstaculo = otro.GetComponent<Obstaculo>();

        if (obstaculo != null)
        {
            if (obstaculo.tipo == Obstaculo.TipoObstaculo.Robot)
            {
                if (obstaculo.particulasExplosion != null)
                {
                    Instantiate(obstaculo.particulasExplosion, obstaculo.transform.position, Quaternion.identity);
                }

                Destroy(obstaculo.gameObject); 
                Destroy(gameObject); 
            }
            else if (obstaculo.tipo == Obstaculo.TipoObstaculo.Solido || obstaculo.tipo == Obstaculo.TipoObstaculo.ChoqueFrontal)
            {
                Destroy(gameObject);
            }
        }
    }
}
