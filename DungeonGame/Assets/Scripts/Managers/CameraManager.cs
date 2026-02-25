using UnityEngine;

public class CameraManager : SingletonPersistent<CameraManager>
{
    #region Variables

    [Header("Transform References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform anchorTransform;
    [SerializeField] private Transform springTransform;
    [SerializeField] private Transform targetTransform; // NOTE : This is currently hardcoded. In the future, make it be extracted from the Player Manager or whatever.

    // NOTE : Anchor and spring are 2 types of pivots for the camera.

    [Header("Camera Settings")]
    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float cameraDistanceMin;
    [SerializeField] private float cameraDistanceMax;
    [SerializeField] private float cameraDistance;

    private float delta;

    #endregion

    #region MonoBehaviour

    void Update()
    {
        delta = Time.deltaTime;
        UpdateAnchorPosition();
        UpdateCameraPosition();
        UpdateCameraRotation();
    }

    #endregion

    #region PublicMethods

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

    #region PrivateMethods

    private void UpdateAnchorPosition()
    {
        anchorTransform.position = targetTransform.position;
    }

    private void UpdateCameraPosition()
    {
        var dir = -GetLookDirection();
        var origin = cameraTransform.position;
        var target = anchorTransform.position + dir * cameraDistance;
        cameraTransform.position = Vector3.Lerp(origin, target, delta * cameraMovementSpeed);
    }

    private void UpdateCameraRotation()
    {
        var dir = GetLookDirection();
        var origin = springTransform.rotation;
        var target = Quaternion.LookRotation(dir, Vector3.up);
        springTransform.rotation = Quaternion.Lerp(origin, target, delta * cameraRotationSpeed);
        cameraTransform.rotation = springTransform.rotation;
    }

    private Vector3 GetLookDirection()
    {
        return (targetTransform.position - springTransform.position).normalized;
    }

    #endregion
}
