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

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocidadVertical;
    private float movimientoX;
    private bool enElSuelo;
    private bool puedeDobleSalto;

    private readonly int hashEnSuelo = Animator.StringToHash("EnSuelo");
    private readonly int hashCorriendo = Animator.StringToHash("Corriendo");

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
}
