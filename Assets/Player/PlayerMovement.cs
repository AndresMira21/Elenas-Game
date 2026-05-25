using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;

    [Header("Mouse")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Gravedad")]
    public float gravity = -9.8f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Agacharse")]
    public float alturaAgachado = 0.9f;
    public float alturaNormal = 1.9f;

    private float xRotation = 0f;
    private float yVelocity = 0f;

    private CharacterController controller;
    private bool isGrounded;
    private bool agachado = false;
    public Vector3 camaraOriginal;

    public static bool canMove = true;
    public static bool canLook = true;

    public void SetCameraHeight(float y)
    {
        Vector3 pos = playerCamera.localPosition;
        pos.y = y;

        playerCamera.localPosition = pos;

        camaraOriginal = playerCamera.localPosition;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        camaraOriginal = playerCamera.localPosition;
    }

    void Update()
    {
        // =========================
        // BLOQUEO GLOBAL
        // =========================
        if (!canMove)
            return;

        // =========================
        // DETECTAR SUELO
        // =========================
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        // =========================
        // MOUSE LOOK
        // =========================
        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // =========================
        // AGACHARSE
        // =========================
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (agachado)
            {
                bool hayEspacio = !Physics.SphereCast(
                    transform.position,
                    0.3f,
                    Vector3.up,
                    out RaycastHit hit,
                    alturaNormal - alturaAgachado);

                if (hayEspacio)
                {
                    agachado = false;
                    controller.height = alturaNormal;
                    controller.center = new Vector3(0, alturaNormal / 2, 0);
                    playerCamera.localPosition = camaraOriginal;
                }
            }
            else
            {
                agachado = true;
                controller.height = alturaAgachado;
                controller.center = new Vector3(0, alturaAgachado / 2, 0);

                playerCamera.localPosition = new Vector3(
                    camaraOriginal.x,
                    0.3f,
                    camaraOriginal.z
                );
            }
        }

        // =========================
        // MOVIMIENTO
        // =========================
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // =========================
        // GRAVEDAD
        // =========================
        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);


    }

    public void ResetCrouchState()
    {
        agachado = false;

        controller.height = alturaNormal;
        controller.center = new Vector3(0, alturaNormal / 2, 0);

        playerCamera.localPosition = camaraOriginal;
    }
}