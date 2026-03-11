using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMove : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    public float velocidadLateral = 7f;

    [Header("Configuración de Salto")]
    public float fuerzaSalto = 2f;
    public float gravedad = -9.81f;

    [Header("Configuración de Agacharse")]
    public float alturaNormal = 1.71f;
    public float alturaAgachado = 0.85f; 
    public Vector3 centroNormal = new Vector3(0, 0.83f, 0);
    public Vector3 centroAgachado = new Vector3(0, 0.415f, 0);
    public float duracionAgachado = 1.5f;
    public float velocidadCaidaRapida = -15f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocidadVertical;
    private float movimientoX;
    private bool enElSuelo;
    private bool puedeDobleSalto;

    private bool estaAgachado = false;
    private float temporizadorAgachado = 0f;

    private readonly int hashEnSuelo = Animator.StringToHash("EnSuelo");
    private readonly int hashCorriendo = Animator.StringToHash("Corriendo");
    private readonly int hashAgachado = Animator.StringToHash("Agachado");

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        enElSuelo = controller.isGrounded;
        animator.SetBool(hashEnSuelo, enElSuelo);

        if (enElSuelo && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
            puedeDobleSalto = false;
        }

        if (estaAgachado)
        {
            temporizadorAgachado -= Time.deltaTime;

            if (temporizadorAgachado <= 0f)
            {
                Levantarse();
            }
        }

        velocidadVertical.y += gravedad * Time.deltaTime;

        Vector3 movimientoFinal = new Vector3(0, velocidadVertical.y, 0);

        if (GameManager.Instancia.juegoIniciado)
        {
            animator.SetBool(hashCorriendo, true);
            movimientoFinal.x = movimientoX * velocidadLateral;
        }
        else
        {
            animator.SetBool(hashCorriendo, false);
        }

        controller.Move(movimientoFinal * Time.deltaTime);
    }

    public void AlMover(InputAction.CallbackContext contexto)
    {
        movimientoX = contexto.ReadValue<float>();
    }

    public void AlSaltar(InputAction.CallbackContext contexto)
    {
        if (!GameManager.Instancia.juegoIniciado) return;

        if (estaAgachado) return;

        if (contexto.performed)
        {
            if (enElSuelo)
            {
                velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
                puedeDobleSalto = true;
            }
            else if (puedeDobleSalto)
            {
                velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
                puedeDobleSalto = false;
                animator.Play("Jumping", 0, 0f);
            }
        }
    }

    public void AlAgachar(InputAction.CallbackContext contexto)
    {
        if (!GameManager.Instancia.juegoIniciado) return;

        if (contexto.performed)
        {
            if (enElSuelo && !estaAgachado)
            {
                Agacharse();
            }

            else if (!enElSuelo)
            {
                velocidadVertical.y = velocidadCaidaRapida;
            }
        }
    }

    private void Agacharse()
    {
        estaAgachado = true;
        temporizadorAgachado = duracionAgachado;

        animator.SetBool(hashAgachado, true);
        controller.height = alturaAgachado;
        controller.center = centroAgachado;
    }

    private void Levantarse()
    {
        estaAgachado = false;

        animator.SetBool(hashAgachado, false);
        controller.height = alturaNormal;
        controller.center = centroNormal;
    }

    private void OnControllerColliderHit(ControllerColliderHit choque)
    {
        Obstaculo obstaculo = choque.gameObject.GetComponent<Obstaculo>();

        if (obstaculo != null && obstaculo.tipo == Obstaculo.TipoObstaculo.Solido)
        {

            if (choque.normal.y < 0.5f)
            {
                PlayerHealth vida = GetComponent<PlayerHealth>();

                if (vida != null)
                {
                    vida.RecibirDano(vida.vidaMaxima);
                }
            }
        }

    }
}
