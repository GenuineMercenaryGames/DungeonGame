using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;

    private CharacterController characterController;

    private Vector2 inputMoveRaw;
    private Vector2 inputMove;

    private float delta;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        delta = Time.deltaTime;
        UpdateLookAt();
        UpdateMove();
    }

    public void InputMove(InputAction.CallbackContext ctx)
    {
        inputMoveRaw = ctx.ReadValue<Vector2>();
        inputMove = inputMoveRaw.normalized;
    }

    public void InputCameraZoom(InputAction.CallbackContext ctx)
    {
        float val = ctx.ReadValue<float>();
        CameraManager.Instance.AddCameraZoom(val);
    }

    public void InputDEBUGCameraVibrate(InputAction.CallbackContext ctx)
    {
        CameraManager.Instance.AddCameraVibration(5);
    }

    public void InputLook(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        // Debug.Log($"the value is : {v}");
    }

    public void InputAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log("Player Attack!");
        // TODO : Implement logic
    }

    private void UpdateMove()
    {
        // TODO : Change to use vectors relative to camera...
        // TODO : Gravity support. Trivial to add, but I also want to add some basic forces support for easy knockback and dashing uniform support.
        Vector3 forward = GetMoveForward();
        Vector3 right = GetMoveRight();
        Vector3 move = (forward * inputMove.y + right * inputMove.x) * walkSpeed * delta;
        characterController.Move(move);
    }

    private void UpdateLookAt()
    {
        var cam = Camera.main;

        Vector3 viewportPosPlayer = cam.WorldToViewportPoint(transform.position);
        Vector3 viewportPosMouse = cam.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        Vector3 dir = (viewportPosMouse - viewportPosPlayer).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.Euler(0, -angle, 0);
    }

    private Vector3 GetMoveForward()
    {
        return CameraManager.Instance.ForwardMoveVector;
    }

    private Vector3 GetMoveRight()
    {
        return CameraManager.Instance.RightMoveVector;
    }

}
