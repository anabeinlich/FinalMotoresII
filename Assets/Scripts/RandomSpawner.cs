using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Lista de Items Posibles")]
    public GameObject[] itemsPosibles;

    [Header("Configuración")]
    [Range(0, 100)]
    public float probabilidadDeAparicion = 100f;

    void Start()
    {
        SpawnearItem();
    }

    private void SpawnearItem()
    {
        float dado = Random.Range(0f, 100f);

        if (dado <= probabilidadDeAparicion && itemsPosibles.Length > 0)
        {
            int indiceElegido = Random.Range(0, itemsPosibles.Length);

            GameObject itemCreado = Instantiate(itemsPosibles[indiceElegido], transform.position, transform.rotation);

            itemCreado.transform.SetParent(transform);
        }
    }
}
