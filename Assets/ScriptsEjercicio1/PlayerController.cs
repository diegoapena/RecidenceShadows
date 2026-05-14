using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputSystem_Actions inputs;
    public float moveSpeed = 10f;
    public float rotationSpeed = 200f;
    private CharacterController controller;
    [SerializeField] private Vector2 moveInput;
    //public float gravity = -9.81f;
    public float verticalVelocity = 0f;
    public float JumpForce = 5f;
    public float pushForce = 2f;
    public bool IsDashing = false;
    public float dashForce = 20f;
    public float dashDuration = 0.5f;
    private float dashTimer = 0f;
    private bool isSprinting = false;
    private float baseMoveSpeed;



    private void Awake()
    {
        inputs = new();
        controller = GetComponent<CharacterController>();
        baseMoveSpeed = moveSpeed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputs.Player.Jump.performed += Jump_performed;
       
        inputs.Player.Sprint.performed += OnSprint;
        inputs.Player.Sprint.canceled += OnSprintCanceled;
    }


    void Start()
    {

    }


    void Update()
    {
        Movement();
        //OnSimpleMovement();
    }




    public void Movement()
    {
        Vector3 forwardDir = transform.forward;
        forwardDir.y = 0;
        forwardDir.Normalize();

        if (moveInput != Vector2.zero)
        {
            Quaternion targetQuaternion = Quaternion.LookRotation(forwardDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetQuaternion, rotationSpeed * Time.deltaTime);
        }


        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        float currentSpeed = isSprinting ? baseMoveSpeed * 3 : baseMoveSpeed;
        Vector3 moveDir = forwardDir * currentSpeed * moveInput.y;






        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)

            verticalVelocity = -2f;



        moveDir.y = verticalVelocity;


        if (IsDashing)
        {
            moveDir = transform.forward * dashForce * (dashTimer / dashTimer);
            IsDashing = false;
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
                IsDashing = false;

        }


        controller.Move(moveDir * Time.deltaTime);


    }
    /* 
    public void OnSimpleMovement()
    {
        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        

        Vector3 moveDir = transform.forward * moveSpeed * moveInput.y;
        controller.SimpleMove(moveDir);
    }
    */


    private void Jump_performed(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded) return;
        verticalVelocity = JumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 pushDir = (hit.transform.position - transform.position).normalized;
        if (hit.rigidbody != null)
            hit.rigidbody.AddForce(pushDir * pushForce, ForceMode.Impulse);
    }
  
    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }
    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Punto de inicio del rayo (posición del objeto)
        Vector3 start = transform.position;

        // Dirección del rayo (hacia adelante desde el objeto)
        Vector3 direction = transform.forward.normalized;

        // Dibujar el rayo desde la posición del objeto hacia adelante
        Gizmos.DrawRay(start, direction * 5f); // El 5f es la longitud del rayo
    }
}
