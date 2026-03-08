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


    public PlayerController playerController;
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
        playerController.healthController.Health.Value -= 20f;
    }
    public void RangeAttack()
    {
        agent.ResetPath();
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerScanningRange);
        Gizmos.DrawWireSphere(transform.position, playerAttackRange);
#if UNITY_EDITOR
        if (Application.isPlaying && fsm != null)
        {
            Handles.Label(transform.position + Vector3.up * 5f, fsm.GetActiveHierarchyPath());
        }
#endif
    }

    public bool ReachedDestination()
    {
        return agent.remainingDistance < 0.01f;
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
        target = playerController.characterController.transform;
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