using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    #region Serialized Variables

    // NOTE : Anchor and spring are 2 types of pivots for the camera.
    [Header("Transform References")]
    [SerializeField] private Transform anchorTransform; // base transform
    [SerializeField] private Transform springTransform; // probe transform

    private Transform cameraTransform;
    private Transform targetTransform;

    [Header("Camera Settings")]
    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float cameraDistanceMin;
    [SerializeField] private float cameraDistanceMax;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float cameraDistancePlaneMax;
    [SerializeField] private float vibrationDecay;

    #endregion

    #region Private Variables

    private float delta;

    private float vibration;

    #endregion

    #region Properties

    public Vector3 ForwardMoveVector { get { return springTransform.forward; } }
    public Vector3 RightMoveVector { get { return springTransform.right; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        cameraTransform = Camera.main.transform;
        targetTransform = PlayerManager.Instance.Player.transform;
    }

    void Update()
    {
        if (GameTime.IsPaused)
            return;
        delta = Time.deltaTime;
        UpdateAnchorPosition();
        UpdateCameraPosition();
        UpdateCameraRotation();
        UpdateCameraVibration();
    }

    #endregion

    #region PublicMethods - Zoom

    public void AddCameraZoom(float amount)
    {
        SetCameraZoom(cameraDistance + amount);
    }

    public void SetCameraZoom(float amount)
    {
        cameraDistance = Mathf.Clamp(amount, cameraDistanceMin, cameraDistanceMax);
    }

    public float GetCameraZoom()
    {
        return cameraDistance;
    }

    #endregion

    #region PublicMethods - Vibration

    public void AddCameraVibration(float amount)
    {
        vibration += amount;
    }

    #endregion

    #region PublicMethods - Rotation

    public void AddCameraRotation(float angleIncrement)
    {
        float baseAngle = anchorTransform.rotation.eulerAngles.y;
        SetCameraRotation(baseAngle +  angleIncrement);
    }

    public void SetCameraRotation(float angle)
    {
        anchorTransform.rotation = Quaternion.Euler(0, angle, 0);
    }

    #endregion

    #region PrivateMethods

    private void UpdateAnchorPosition()
    {
        // TODO : Clean this code up a bit lol...

        anchorTransform.position = targetTransform.position;

        var cam = Camera.main;
        Vector3 viewportPosPlayer = cam.WorldToViewportPoint(anchorTransform.position);
        Vector3 viewportPosMouse = cam.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        Vector3 vec = (viewportPosMouse - viewportPosPlayer);
        Vector3 v = new Vector3(vec.x, 0.0f, vec.y);
        Vector3 dir = v.normalized;

        float cursorDist = v.magnitude; // NOTE : Since this is calculated in viewport space, this should give us a value in range [0, 1], so that's perfect for lerping.
        // float chosenCamDist = Mathf.Min(cameraDistancePlaneMax, cursorDist);
        float chosenCamDist = Mathf.Lerp(0.0f, cameraDistancePlaneMax, cursorDist);

        // Debug.Log($"the distance is : {cursorDist}");

        anchorTransform.position += chosenCamDist * dir;
    }

    private void UpdateCameraPosition()
    {
        var dir = -GetLookDirection();
        var origin = cameraTransform.position;
        var target = anchorTransform.position + dir * cameraDistance;
        // cameraTransform.position = Vector3.Lerp(origin, target, delta * cameraMovementSpeed);
        cameraTransform.position = target; // NOTE : Lerp disabled for now because it gives a better camera feel tbh...
    }

    private void UpdateCameraRotation()
    {
        var dir = GetLookDirection();
        var origin = cameraTransform.rotation;
        var target = Quaternion.LookRotation(dir, Vector3.up);
        cameraTransform.rotation = Quaternion.Lerp(origin, target, delta * cameraRotationSpeed);
    }

    private Vector3 GetLookDirection()
    {
        return (anchorTransform.position - springTransform.position).normalized;
    }

    private void UpdateCameraVibration()
    {
        if (vibration < 0.0f)
            return;
        Vector3 dir = Random.insideUnitSphere;
        cameraTransform.position += dir * vibration * delta;
        vibration *= Mathf.Pow(vibrationDecay, delta); // NOTE : Exponential decay ensures that we have framrate independent decay without making the mistake of multiplying the value by a near 0 value, which would make the shake end in one frame.
    }

    #endregion
}
