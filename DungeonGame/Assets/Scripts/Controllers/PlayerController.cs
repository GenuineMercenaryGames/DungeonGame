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

    private void UpdateMove()
    {
        // TODO : Change to use vectors relative to camera...
        Vector3 move = (transform.forward * inputMove.y + transform.right * inputMove.x) * walkSpeed * delta;
        characterController.Move(move);
    }
}
