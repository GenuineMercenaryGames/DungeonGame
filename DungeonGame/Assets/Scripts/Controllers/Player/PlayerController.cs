using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private Animator animator;

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

        isRunning = false;
        canDash = true;

        // Self contained. The player handles their own fucking death.
        // For anyone reading, for the love of God, implement things like this from now on rather than stuffing your logic inside of the HealthController class, ffs.
        healthController.Health.AddListener(OnDeath);

        // NOTE : This is a temporary hack because the player prefab is placed manually within the scene.
        // Once the actual spawning logic is implemented within the PlayerManager, this could be removed. But for now, we need this.
        PlayerManager.Instance.SetPlayer(this);
    }

    void Update()
    {
        if (GameTime.IsPaused)
            return;
        UpdateLookAt();
        UpdateMove();
        UpdateAnimation();
    }

    #endregion

    #region PublicMethods - Input

    public void InputMove(InputAction.CallbackContext ctx)
    {
        inputMoveRaw = ctx.ReadValue<Vector2>();
        inputMove = inputMoveRaw.normalized;

        // NOTE : This is retarded, but some keyboard devices actually DO normalize the signal on input for some reason, so, for non-normalized inputs, we need
        // this hack. Also for controller support to properly translate animation to 2-axis cardinal setup. Anyway, fuck my life, and fuck this hack.
        // This used to not be the case tho. Older Unity versions handled analogue input properly, but it seems like the future has something else in store for us...
        inputMoveRaw.x = inputMoveRaw.x == 0.0f ? 0.0f : Mathf.Sign(inputMoveRaw.x);
        inputMoveRaw.y = inputMoveRaw.y == 0.0f ? 0.0f : Mathf.Sign(inputMoveRaw.y);
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

    // NOTE : This is a temporary hack to test the melee animations, disregard completely because we'll have a proper handling in future versions.
    bool attackMode = true; // true -> gun, false -> melee
    public void InputDEBUGCameraVibrate(InputAction.CallbackContext ctx) // For now, this has been repurposed to work as the melee to gun switch button.
    {
        attackMode = !attackMode;
    }

    public void InputLook(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        // Debug.Log($"the value is : {v}");
    }

    public void InputAttack(InputAction.CallbackContext ctx)
    {
        // Debug.Log("Attack!!!!!!!!!!!!");
        if (ctx.phase == InputActionPhase.Performed)
            Attack();
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

    public void InputPause(InputAction.CallbackContext ctx)
    {
        // NOTE : Ugly temporary hack to get things working. In the future, this logic should be more self contained. For now, fuck it.
        if(GameTime.IsPaused && GameTime.CanPause)
            UIManager.Instance.PauseUI.Resume();
        else
            UIManager.Instance.PauseUI.Pause();
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
        float groundY = transform.position.y;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane plane = new(Vector3.up, new Vector3(0, groundY, 0));

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            Vector3 direction = hitPoint - transform.position;
            direction.y = 0;

            transform.rotation = Quaternion.LookRotation(direction);
        }
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
        Vector3 moveVector = forward * inputMove.y + right * inputMove.x;
        return moveVector;
    }

    private Vector3 GetDashVector()
    {
        Vector3 moveVector = GetMoveVector();
        Vector3 dashVector = moveVector.magnitude > 0.0f ? moveVector : transform.forward;
        return dashVector;
    }

    // NOTE : Again, this is a temporary hack just to quickly test the melee attack animations system.
    // TODO : Unify the logic and move it away from being spread out like this, basically look for something better than this temporary crap.
    // The idea would be for the weapon controller system to be unified enough that we could tell from here if its a melee attack or not, and change the
    // animation set the player uses depending on that, as well as being able to maintain a single attack function here.
    private void Attack()
    {
        if (attackMode)
            AttackGun();
        else
            AttackMelee();
    }

    private void AttackGun()
    {
        bool hasShot = weaponController.Attack();
        if (hasShot)
        {
            CameraManager.Instance.AddCameraVibration(4.0f);
            animator.SetTrigger("TriggerShoot");
        }

        // TODO : For now, only the player can add this vibration when we shoot.
        // Since enemies can also have a weapons that shoots, and heavy weapons at that, it would be logical for the vibration to take place whenever an
        // explosion or loud shot takes place near the player's location. So, maybe just make it so that the weapon systems adds camera vibration based on
        // the distance to the camera's ground anchor point?
        // Note that the anchor point is pretty much the player's location, but doing it like this would allow for cinematics and such to have environment-driven
        // vibrations without hardcoded events even if the camera is pointing to a location that is far from the player's position.
    }

    int currentMeleeAttack = 0;
    bool isAttackingMelee = false;
    private void AttackMelee()
    {
        if (!isAttackingMelee)
        {
            StartCoroutine(AttackMeleeCoro(currentMeleeAttack % 3));
            ++currentMeleeAttack;
        }
    }

    // NOTE : Yes, I know the melee animations look like fucking ASS. I need to find something that looks good, but this is good enough for
    // the alpha. I'll improve them if I find the time to do so. For now, we'll have to make do with what we have. And what we have is "no time to fuck around".
    private IEnumerator AttackMeleeCoro(int meleeAttackIndex)
    {
        isAttackingMelee = true;
        
        int layerIndex = animator.GetLayerIndex("CombatMelee");
        string meleeAttackStr = $"Locomotion.Attack{meleeAttackIndex}";
        float elapsedTime = 0.0f;
        float meleeDuration = 0.25f;

        animator.SetLayerWeight(animator.GetLayerIndex("Combat"), 0.0f);
        animator.CrossFadeInFixedTime(meleeAttackStr, 0.1f);
        // animator.Play(meleeAttackStr);
        while (elapsedTime < meleeDuration)
        {
            Move(10.0f * transform.forward * delta);
            elapsedTime += delta;
            yield return null;
        }
        animator.SetLayerWeight(animator.GetLayerIndex("Combat"), 1.0f);
        animator.CrossFadeInFixedTime("Locomotion.LocomotionBlendTree", 0.1f);

        isAttackingMelee = false;
    }

    private void Dash()
    {
        if (canDash)
        {
            animator.SetTrigger("TriggerDash");
            StartCoroutine(DashCoroutine(GetDashVector()));
        }
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

    private void UpdateAnimation()
    {
        float dampTime = 0.02f;
        float speed = isRunning ? 1.0f : 0.0f;
        Vector3 worldMove = new Vector3(inputMoveRaw.x, 0.0f, inputMoveRaw.y);
        Vector3 localMove = transform.InverseTransformDirection(worldMove);
        float moveX = localMove.x * 2.0f;
        float moveY = localMove.z * 2.0f;
        animator.SetFloat("Speed", speed, dampTime, delta);
        animator.SetFloat("MoveX", moveX, dampTime, delta);
        animator.SetFloat("MoveY", moveY, dampTime, delta);
        // Debug.Log($"The input is: raw:{inputMoveRaw}, noRaw:{inputMove}, anim:{new Vector2(moveX, moveY)}");
    }

    #endregion

    #region PrivateMethods - Health

    private void OnDeath(float oldValue, float newValue)
    {
        if (newValue <= 0f)
        {
            UIManager.Instance.GameOverUI.ShowGameOver(false);
            GameTime.IsPaused = true; // Temporary hack until control disabling is added.
            GameTime.CanPause = false;

            // TODO : Play death animation and disable controls.
        }
    }

    #endregion

}
