using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] public float Speed;
    [SerializeField] public float Duration;
    [SerializeField] public float Cooldown;

    private PlayerMovement playerMovement;
    private bool canDash;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        canDash = true;
    }

    public void Dash()
    {
        if (canDash)
            StartCoroutine(DashCoroutine(transform.forward));
    }

    private IEnumerator DashCoroutine(Vector3 direction)
    {
        canDash = false;
        float elapsedTime = 0.0f;

        while (elapsedTime < Duration)
        {
            playerMovement.AddVelocity(Speed * direction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(Cooldown);
        canDash = true;
    }
}
