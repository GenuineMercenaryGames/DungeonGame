using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    public CharacterController characterController;
    public WeaponController weaponController;
    public HealthController healthController;
    public ShieldController shieldController;

    private Vector2 inputMoveRaw;
    private Vector2 inputMove;

    private bool isRunning;

    private float delta;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        weaponController = GetComponent<WeaponController>();
        healthController = GetComponent<HealthController>();
        shieldController = GetComponent<ShieldController>();
    }

    void Update()
    {
        delta = Time.deltaTime;
        UpdateLookAt();
        UpdateMove();
    }

    #endregion

    #region PublicMethods - Input

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
        if (ctx.phase != InputActionPhase.Performed)
            return;
        Attack();
    }

    public void InputRun(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.phase == InputActionPhase.Performed;
    }

    #endregion

    #region PrivateMethods

    private void UpdateMove()
    {
        // TODO : Change to use vectors relative to camera...
        // TODO : Gravity support. Trivial to add, but I also want to add some basic forces support for easy knockback and dashing uniform support.
        Vector3 forward = GetMoveForward();
        Vector3 right = GetMoveRight();
        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 move = (forward * inputMove.y + right * inputMove.x) * speed * delta;
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

    private void Attack()
    {
        bool hasShot = weaponController.Attack();
        if (hasShot)
            CameraManager.Instance.AddCameraVibration(4.0f);
        // TODO : For now, only the player can add this vibration when we shoot.
        // Since enemies can also have a weapons that shoots, and heavy weapons at that, it would be logical for the vibration to take place whenever an
        // explosion or loud shot takes place near the player's location. So, maybe just make it so that the weapon systems adds camera vibration based on
        // the distance to the camera's ground anchor point?
        // Note that the anchor point is pretty much the player's location, but doing it like this would allow for cinematics and such to have environment-driven
        // vibrations without hardcoded events even if the camera is pointing to a location that is far from the player's position.
    }

    #endregion

}
