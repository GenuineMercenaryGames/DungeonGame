using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravitySpeed;
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashCooldown;

    public CharacterController characterController;
    public WeaponController weaponController;
    public HealthController healthController;
    public ShieldController shieldController;
    public ObservableVariable<int> Coins = new(0);
    [SerializeField] private AttackOrchestrator attackOrchestrator;

    private Vector2 inputMoveRaw;
    private Vector2 inputMove;

    private bool isRunning;
    private bool canDash;

    private float delta { get { return Time.deltaTime; } }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        weaponController = GetComponent<WeaponController>();
        healthController = GetComponent<HealthController>();
        shieldController = GetComponent<ShieldController>();
        attackOrchestrator = GetComponent<AttackOrchestrator>();

        isRunning = false;
        canDash = true;
    }

    void Start()
    {
        PlayerManager.Instance.SetPlayer(this);
        // NOTE : This is a temporary hack because the player prefab is placed manually within the scene.
        // Once the actual spawning logic is implemented within the PlayerManager, this will be removed. But for now, we need this.
    }

    void Update()
    {
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

    public void InputCameraRotate(InputAction.CallbackContext ctx)
    {
        // TODO : Fix issue where camera look at logic expects an additional rotation of 0.0f always... rotating breaks the aiming because of that added offset.
        // This is trivial to fix, but it's 2AM, so I'm going to leave the task for tomorrow lol.
        float val = ctx.ReadValue<float>();
        CameraManager.Instance.AddCameraRotation(val * 25.0f);
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
        Debug.Log("Attack!!!!!!!!!!!!");
        if (ctx.phase == InputActionPhase.Performed)
            Attack();
        attackOrchestrator?.TryAttack();
    }

    public void InputRun(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.phase == InputActionPhase.Performed;
    }

    public void InputDash(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
            Dash();
    }

    #endregion

    #region PrivateMethods

    private void Move(Vector3 deltaX)
    {
        characterController.Move(deltaX);
    }

    private void Move(float x, float y, float z)
    {
        Move(new Vector3(x, y, z));
    }

    private void UpdateMove()
    {
        UpdateMoveWalk();
        UpdateMoveGravity();
    }

    private void UpdateMoveWalk()
    {
        Vector3 forward = GetMoveForward();
        Vector3 right = GetMoveRight();
        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 move = GetMoveVector() * speed * delta;
        Move(move);
    }

    private void UpdateMoveGravity()
    {
        Move(0, -gravitySpeed, 0);
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

    private Vector3 GetMoveVector()
    {
        Vector3 forward = GetMoveForward();
        Vector3 right = GetMoveRight();
        return forward * inputMove.y + right * inputMove.x;
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

    private void Dash()
    {
        if (canDash)
            StartCoroutine(DashCoroutine(GetMoveVector()));
    }

    private IEnumerator DashCoroutine(Vector3 direction)
    {
        canDash = false;
        float elapsedTime = 0.0f;

        while (elapsedTime < dashDuration)
        {
            Move(dashSpeed * direction * delta);
            elapsedTime += delta;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    #endregion

}
