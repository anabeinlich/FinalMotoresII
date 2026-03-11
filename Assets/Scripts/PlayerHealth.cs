using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Escudo")]
    public bool escudoActivo = false;
    public GameObject visualEscudo;

    [Header("Interfaz (UI)")]
    public Image barraVidaUI;
    public GameObject panelGameOver;

    public float tiempoRetrasoGameOver = 1.5f;

    private Animator animator;
    private readonly int hashMuerto = Animator.StringToHash("Muerto");

    void Start()
    {
        animator = GetComponent<Animator>();
        vidaActual = vidaMaxima;
        ActualizarUI();
    }

    public void Curar(float cantidad)
    {
        if (GameManager.Instancia != null && !GameManager.Instancia.juegoIniciado) return;

        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
        ActualizarUI();
    }

    public void ActivarEscudoVisual()
    {
        escudoActivo = true;
        if (visualEscudo != null) visualEscudo.SetActive(true);
    }

    public void RecibirDano(float cantidad)
    {
        if (GameManager.Instancia != null && !GameManager.Instancia.juegoIniciado) return;

        if (escudoActivo)
        {
            escudoActivo = false;
            if (visualEscudo != null) visualEscudo.SetActive(false);
            return;
        }

        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        ActualizarUI();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void ActualizarUI()
    {
        if (barraVidaUI != null)
        {
            barraVidaUI.fillAmount = vidaActual / vidaMaxima;
        }
    }

    public void Morir()
    {
        if (GameManager.Instancia != null)
        {
            GameManager.Instancia.juegoIniciado = false;
            GameManager.Instancia.MostrarEstadisticasFinales();
        }

        animator.SetTrigger(hashMuerto);
        Invoke(nameof(MostrarPanelGameOver), tiempoRetrasoGameOver);

        Debug.Log("THYRA muurio");
    }

    private void MostrarPanelGameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
    }

}
