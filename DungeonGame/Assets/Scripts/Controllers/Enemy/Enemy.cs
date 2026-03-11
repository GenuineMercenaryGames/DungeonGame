using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityHFSM;
using static UnityEngine.UI.Image;

public abstract class Enemy : MonoBehaviour
{


    [SerializeField] protected float playerScanningRange = 10.0f;
    [SerializeField] protected float playerAttackRange = 5.0f;
    [SerializeField] protected float moveSpeed = 3.0f;

    // Debug
    [SerializeField] private string currentStateDebug;
    [SerializeField] private LineRenderer shotLine;
    private float shotLineTime = 0.06f;

    public NavMeshAgent agent;
    public HealthController healthController;

    private Transform target;
    private StateMachine fsm;
    private Animator animator;

    protected abstract StateMachine MainFSM();

    #region Utility Functions

    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, target.position);
    }
    public void MoveTowardsPlayer()
    {
        agent.SetDestination(target.position);
    }
    public void MoveTowardsPoint(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void StayStill()
    {
        agent.ResetPath();
    }
    public void MeleeAttack()
    {
        agent.ResetPath();
        animator.SetTrigger("MeleeAttack");
        //GetComponent<WeaponController>().Attack();
        
        // NOTE : Kike, doesn't this always damage the target regardless of the distance?
        // -Dani 
        // -Kike: sí, estoy esperando al melee para meterlo bien.
        var health = target.GetComponent<HealthController>();
        if(health != null) health.Health.Value -= 20f;
    }
    public void RangeAttack()
    {
        agent.ResetPath();
        // Por ahora hago la melee animation, hasta que quitemos los placeholders.
        animator.SetTrigger("MeleeAttack");
        GetComponent<WeaponController>().Attack();
    }

    public bool IsDead()
    {
        return healthController.Health.Value <= 0.0f;
    }

    public bool PointInDirectPlayerSight(Vector3 point)
    {
        bool blocked = agent.Raycast(point, out NavMeshHit hit);
        return !blocked;
    }

    public bool InDirectPlayerSight()
    {
        return PointInDirectPlayerSight(target.position);
    }

    public void LookToPlayer()
    {
        transform.LookAt(target.position);
    }

    public void SmoothLookToPlayer()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            180f * Time.deltaTime
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerScanningRange);
        Gizmos.DrawWireSphere(transform.position, playerAttackRange);
#if UNITY_EDITOR
        if (Application.isPlaying && fsm != null)
        {
            Handles.Label(transform.position + Vector3.up * 1f, fsm.GetActiveHierarchyPath());
            Handles.Label(transform.position + Vector3.up * 2f, "Health: " + healthController.Health.Value + " / " + healthController.MaxHealth.Value);
        }
#endif
    }

    public bool ReachedDestination()
    {
        return agent.remainingDistance < 0.01f;
    }

    public bool InViewAngle()
    {
        Vector3 toPlayer = target.position - transform.position;
        toPlayer.y = 0f; 

        if (toPlayer.sqrMagnitude < 0.0001f)
            return true;

        float angle = Vector3.Angle(transform.forward, toPlayer);
        return angle <= 10 * 0.5f;
    }

    int maxPoints = 10;

    public Vector3 GetRandomReachablePoint(Vector3 position, float radius)
    {
        for (int i = 0; i < maxPoints; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;
            randomOffset.y = 0f;

            Vector3 candidate = position + randomOffset;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return position;
    }

    public Vector3 GetRandomPlayerClosestReachablePoint(float radius)
    {

        Vector3 closestPos = transform.position;
        float closestDistance = 9999;

        for (int i = 0; i < maxPoints; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;
            randomOffset.y = 0f;

            Vector3 candidate = target.position + randomOffset;

            if(Vector3.Distance(candidate, transform.position) < closestDistance)
            {
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    closestPos = candidate;
                    closestDistance = Vector3.Distance(candidate, transform.position);
                }
            }

        }

        return closestPos;
    }

    #endregion

    #region Debug
    private void ShowShotLine(Vector3 to)
    {
        if (shotLine == null) return;

        Vector3 from = transform.position != null
            ? transform.position
            : transform.position + Vector3.up * 1.2f;

        shotLine.SetPosition(0, from);
        shotLine.SetPosition(1, to);
        shotLine.enabled = true;

        CancelInvoke(nameof(HideShotLine));
        Invoke(nameof(HideShotLine), shotLineTime);
    }

    private void HideShotLine()
    {
        if (shotLine != null)
            shotLine.enabled = false;
    }

    #endregion


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthController = GetComponent<HealthController>();
        if (shotLine != null)
        {
            shotLine.positionCount = 2;
            shotLine.enabled = false;
        }
    }

    void Start()
    {
        target = PlayerManager.Instance.Player.transform;
        fsm = MainFSM();
        fsm.Init();
    }

    void Update()
    {
        fsm.OnLogic();
        currentStateDebug = fsm.GetActiveHierarchyPath();
        float normalizedSpeed = (agent.velocity.magnitude / moveSpeed);
        animator.SetFloat("MoveSpeed", normalizedSpeed);

        if (agent == null) return;
        if (agent.pathPending) return;
        if (agent.path == null) return;

        Vector3[] corners = agent.path.corners;
        if (corners == null || corners.Length < 2) return;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 1], Color.green);
        }
    }
}