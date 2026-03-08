using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    public float velocidadAvance = 10f;
    public float velocidadLateral = 7f;

    [Header("Configuración de Salto")]
    public float fuerzaSalto = 2f;
    public float gravedad = -9.81f;

    private CharacterController controller;
    private Vector3 velocidadVertical;
    private float movimientoX;
    private bool enElSuelo;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        enElSuelo = controller.isGrounded;
        if (enElSuelo && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }

        Vector3 movimiento = new Vector3(movimientoX * velocidadLateral, 0, velocidadAvance);
        controller.Move(movimiento * Time.deltaTime);

        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    public void AlMover(InputAction.CallbackContext contexto)
    {
        movimientoX = contexto.ReadValue<float>();
    }

    public void AlSaltar(InputAction.CallbackContext contexto)
    {
        if (contexto.performed && enElSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }
    }
}
