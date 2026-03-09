using System.Collections.Generic;
using UnityEngine;

public class GeneradorNivel : MonoBehaviour
{
    [Header("Configuración de Bloques")]
    public GameObject bloqueInicialEnEscena;
    public GameObject[] bloquesAleatorios;
    public float longitudBloque = 63.601f;
    public int cantidadBloquesEnPantalla = 5;
    public int tamañoPoolPorPrefab = 3;

    [Header("Posición y Rotación")]
    public Vector3 rotacionBloques = Vector3.zero;

    [Header("Limpieza")]
    public float limiteDestruccion = -40f;

    private List<GameObject> bloquesActivos = new List<GameObject>();
    private List<GameObject> poolDeBloques = new List<GameObject>();

    void Start()
    {
        Quaternion rotacion = Quaternion.Euler(rotacionBloques);

        foreach (GameObject prefab in bloquesAleatorios)
        {
            for (int i = 0; i < tamañoPoolPorPrefab; i++)
            {
                GameObject clon = Instantiate(prefab, Vector3.zero, rotacion);
                clon.transform.SetParent(transform);
                clon.SetActive(false);
                poolDeBloques.Add(clon);
            }
        }

        if (bloqueInicialEnEscena != null)
        {
            bloqueInicialEnEscena.transform.position = Vector3.zero;
            bloquesActivos.Add(bloqueInicialEnEscena);
        }

        for (int i = 1; i < cantidadBloquesEnPantalla; i++)
        {
            ActivarBloqueDelPool(i * longitudBloque);
        }
    }

    void Update()
    {
        if (GameManager.Instancia != null && !GameManager.Instancia.juegoIniciado) return;

        GameObject bloqueMasViejo = bloquesActivos[0];

        if (bloqueMasViejo.transform.position.z < limiteDestruccion)
        {
            float nuevaPosicionZ = bloquesActivos[bloquesActivos.Count - 1].transform.position.z + longitudBloque;

            bloqueMasViejo.SetActive(false);
            bloquesActivos.RemoveAt(0);

            ActivarBloqueDelPool(nuevaPosicionZ);
        }
    }

    private void ActivarBloqueDelPool(float posicionZ)
    {
        for (int i = 0; i < poolDeBloques.Count; i++)
        {
            int indiceRandom = Random.Range(0, poolDeBloques.Count);
            GameObject bloqueElegido = poolDeBloques[indiceRandom];

            if (!bloqueElegido.activeInHierarchy)
            {
                bloqueElegido.transform.position = new Vector3(0, 0, posicionZ);
                bloqueElegido.SetActive(true);
                bloquesActivos.Add(bloqueElegido);
                return;
            }
        }

        foreach (GameObject bloque in poolDeBloques)
        {
            if (!bloque.activeInHierarchy)
            {
                bloque.transform.position = new Vector3(0, 0, posicionZ);
                bloque.SetActive(true);
                bloquesActivos.Add(bloque);
                return;
            }
        }
    }
}
