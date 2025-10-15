using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovemetController : MonoBehaviour
{
    [Header("Referências")]
    public Camera playerCamera;

    [Header("Configurações de Movimento")]
    public float walkSpeed = 3f;
    public float runSpeed = 4f;

    [Header("Configurações de Câmera")]
    public float lookSpeed = 1.5f;
    public float lookXLimit = 45f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    public bool canMove = true;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCamera();
        HandleMovement();
    }

    void HandleMovement()
    {
        // só move se canMove for true
        if (!canMove)
        {
            // evita drift de movimento quando sentado
            moveDirection = Vector3.zero;
            return;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleCamera()
    {
        // rotação da câmera (sempre disponível)
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // rotação do corpo com o mouse
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
