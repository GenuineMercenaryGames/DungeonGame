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
        UpdateMove();
    }

    public void InputMove(InputAction.CallbackContext ctx)
    {
        inputMoveRaw = ctx.ReadValue<Vector2>();
        inputMove = inputMoveRaw.normalized;
    }

    public void InputCameraZoom(InputAction.CallbackContext ctx)
    {
        Debug.Log("EEEEEEE");
        float val = ctx.ReadValue<float>();
        CameraManager.Instance.AddCameraZoom(val);
    }

    private void UpdateMove()
    {
        // TODO : Change to use vectors relative to camera...
        Vector3 move = (transform.forward * inputMove.y + transform.right * inputMove.x) * walkSpeed * delta;
        characterController.Move(move);
    }
}
