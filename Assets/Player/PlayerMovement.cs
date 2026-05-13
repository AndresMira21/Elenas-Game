using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;

    [Header("Mouse")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Gravedad")]
    public float gravity = -9.8f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float xRotation = 0f;
    private float yVelocity = 0f;

    private CharacterController controller;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ===== DETECTAR SUELO =====
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        // ===== MOUSE (MIRAR) =====
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // ===== MOVIMIENTO =====
        float x = Input.GetAxis("Horizontal"); // A/D o flechas
        float z = Input.GetAxis("Vertical");   // W/S o flechas

        Vector3 move = transform.right * x + transform.forward * z;

        // ===== GRAVEDAD =====
        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);
    }
}