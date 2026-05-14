using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 80f;
    public Transform Player;

   
    public InputSystem_Actions inputs;

    float xRotation = 0;
    private Vector2 lookInput = Vector2.zero;

    private void Awake()
    {
        if (inputs == null) inputs = new InputSystem_Actions();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Look.performed += OnLookPerformed;
        inputs.Player.Look.canceled += OnLookCanceled;
    }

    private void OnDisable()
    {
        inputs.Player.Look.performed -= OnLookPerformed;
        inputs.Player.Look.canceled -= OnLookCanceled;
        inputs.Disable();
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        lookInput = Vector2.zero;
    }

    void Update()
    {
        
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (Player != null)
            Player.Rotate(Vector3.up * mouseX);
    }
}
