using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    public bool juegoIniciado = false;

    [Header("Progresión y Estadísticas")]
    public float velocidadMundo = 4f;
    public float metrosRecorridos = 0f;
    public int monedas = 0;

    public float cantidadAumentoVelocidad = 2f;
    public float metrosParaAumentar = 200f;
    private float siguienteUmbralVelocidad;

    [Header("UI - HUD en Juego")]
    public TextMeshProUGUI textoCuentaRegresiva;
    public TextMeshProUGUI textoMetrosHUD;
    public TextMeshProUGUI textoMonedasHUD;

    [Header("UI - Game Over")]
    public TextMeshProUGUI textoMetrosFinal;
    public TextMeshProUGUI textoMonedasFinal;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        siguienteUmbralVelocidad = metrosParaAumentar;
        StartCoroutine(CuentaRegresiva());
    }

    IEnumerator CuentaRegresiva()
    {
        if (textoCuentaRegresiva != null) textoCuentaRegresiva.gameObject.SetActive(true);

        textoCuentaRegresiva.text = "3";
        yield return new WaitForSeconds(1f);

        textoCuentaRegresiva.text = "2";
        yield return new WaitForSeconds(1f);

        textoCuentaRegresiva.text = "1";
        yield return new WaitForSeconds(1f);

        textoCuentaRegresiva.text = "¡YA!";
        yield return new WaitForSeconds(1f);

        if (textoCuentaRegresiva != null) textoCuentaRegresiva.gameObject.SetActive(false);

        juegoIniciado = true;
        Debug.Log("Arranca el juego");
    }

    void Update()
    {
        if (juegoIniciado)
        {
            metrosRecorridos += velocidadMundo * Time.deltaTime;

            if (metrosRecorridos >= siguienteUmbralVelocidad)
            {
                velocidadMundo += cantidadAumentoVelocidad;
                siguienteUmbralVelocidad += metrosParaAumentar;

                Debug.Log("¡Nivel subido! Nueva velocidad: " + velocidadMundo);
            }

            if (textoMetrosHUD != null) textoMetrosHUD.text = Mathf.FloorToInt(metrosRecorridos) + " m";
        }
    }

    public void SumarMoneda(int cantidad)
    {
        monedas += cantidad;
        if (textoMonedasHUD != null) textoMonedasHUD.text = monedas.ToString();
    }

    public void MostrarEstadisticasFinales()
    {
        if (textoMetrosFinal != null) textoMetrosFinal.text = "Metros: " + Mathf.FloorToInt(metrosRecorridos) + "m";
        if (textoMonedasFinal != null) textoMonedasFinal.text = "Monedas: " + monedas;
    }
}
