using UnityEngine;

public class CameraManager : SingletonPersistent<CameraManager>
{
    #region Variables

    [Header("Transform References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform springTransform; // NOTE : Maybe the name "pivotTransform" would make more sense, but I like the fact that this has the same text length as the other var names... also it's the Unreal Engine terminology, so maybe more people get what I mean? kinda?
    [SerializeField] private Transform targetTransform; // NOTE : This is currently hardcoded. In the future, make it be extracted from the Player Manager or whatever.

    [Header("Camera Settings")]
    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private float cameraRotationSpeed;

    private float delta;

    #endregion

    #region MonoBehaviour

    void Update()
    {
        delta = Time.deltaTime;
        UpdateSpringPosition();
        UpdateCameraRotation();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateSpringPosition()
    {
        var origin = springTransform.position;
        var target = targetTransform.position;
        springTransform.position = Vector3.Lerp(origin, target, delta * cameraMovementSpeed);
    }

    private void UpdateCameraRotation()
    {
        var dir = (targetTransform.position - springTransform.position).normalized;
        var origin = springTransform.rotation;
        var target = Quaternion.LookRotation(dir, Vector3.up);
        springTransform.rotation = Quaternion.Lerp(origin, target, delta * cameraRotationSpeed);
    }

    #endregion
}
