using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPowerUps : MonoBehaviour
{
    [Header("Inventario")]
    public int cantEscudos = 0;
    public int cantArmas = 0;

    [Header("Arma Activa")]
    public bool armaActiva = false;
    public float duracionArma = 10f;
    private float temporizadorArma;

    [Header("Disparo")]
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public float ritmoDisparo = 0.5f;

    [Header("UI")]
    public TextMeshProUGUI textoCantEscudos;
    public TextMeshProUGUI textoCantArmas;
    public Image barraArmaUI;

    private PlayerHealth salud;

    void Start()
    {
        salud = GetComponent<PlayerHealth>();
        ActualizarUI();
        if (barraArmaUI != null) barraArmaUI.fillAmount = 0;
    }

    void Update()
    {
        if (GameManager.Instancia != null && !GameManager.Instancia.juegoIniciado) return;

        // ACTIVAR ESCUDO (Q)
        if (Input.GetKeyDown(KeyCode.Q) && cantEscudos > 0 && !salud.escudoActivo)
        {
            cantEscudos--;
            salud.ActivarEscudoVisual();
            ActualizarUI();
        }

        // ACTIVAR ARMA (E)
        if (Input.GetKeyDown(KeyCode.E) && cantArmas > 0 && !armaActiva)
        {
            cantArmas--;
            armaActiva = true;
            temporizadorArma = duracionArma;

            InvokeRepeating(nameof(Disparar), 0f, ritmoDisparo);
            ActualizarUI();
        }

        // LÓGICA DEL TIEMPO DEL ARMA
        if (armaActiva)
        {
            temporizadorArma -= Time.deltaTime;
            if (barraArmaUI != null) barraArmaUI.fillAmount = temporizadorArma / duracionArma;

            if (temporizadorArma <= 0)
            {
                armaActiva = false;
                CancelInvoke(nameof(Disparar));
                if (barraArmaUI != null) barraArmaUI.fillAmount = 0;
            }
        }
    }

    private void Disparar()
    {
        if (prefabBala != null && puntoDisparo != null)
        {
            Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
        }
    }

    public void RecogerPowerUp(string tipo)
    {
        if (tipo == "Escudo") cantEscudos++;
        else if (tipo == "Arma") cantArmas++;

        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (textoCantEscudos != null) textoCantEscudos.text = cantEscudos.ToString();
        if (textoCantArmas != null) textoCantArmas.text = cantArmas.ToString();
    }
}
