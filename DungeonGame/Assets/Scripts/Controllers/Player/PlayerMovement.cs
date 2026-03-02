using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 characterVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Debug.Log($"The velocity is: {characterVelocity}");
        characterController.Move(characterVelocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        characterVelocity = Vector3.zero;
    }

    public void AddVelocity(Vector3 velocity)
    {
        characterVelocity += velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        characterVelocity = velocity;
    }
}
