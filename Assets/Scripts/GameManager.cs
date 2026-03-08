using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    public bool juegoIniciado = false;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(CuentaRegresiva());
    }

    IEnumerator CuentaRegresiva()
    {
        Debug.Log("3...");
        yield return new WaitForSeconds(1f);
        Debug.Log("2...");
        yield return new WaitForSeconds(1f);
        Debug.Log("1...");
        yield return new WaitForSeconds(1f);
        Debug.Log("¡YA!");

        juegoIniciado = true;
        Debug.Log("Arracanca el juego");
    }
}
