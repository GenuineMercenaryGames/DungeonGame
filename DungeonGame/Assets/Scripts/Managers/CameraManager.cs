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
    #endregion

    #region PrivateMethods

    private void UpdateAnchorPosition()
    {
        anchorTransform.position = targetTransform.position;
    }

    private void UpdateCameraPosition()
    {
        var origin = cameraTransform.position;
        var target = springTransform.position;
        cameraTransform.position = Vector3.Lerp(origin, target, delta * cameraMovementSpeed);
    }

    private void UpdateCameraRotation()
    {
        var dir = (targetTransform.position - springTransform.position).normalized;
        var origin = springTransform.rotation;
        var target = Quaternion.LookRotation(dir, Vector3.up);
        springTransform.rotation = Quaternion.Lerp(origin, target, delta * cameraRotationSpeed);
        cameraTransform.rotation = springTransform.rotation;
    }

    #endregion
}
