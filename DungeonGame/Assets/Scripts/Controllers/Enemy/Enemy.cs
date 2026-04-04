using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityHFSM;
using static UnityEngine.UI.Image;

public enum EnemyStates
{
    Wandering,
    Combat,
    Flee,
    Dead
}

public abstract class Enemy : MonoBehaviour
{


    [SerializeField] protected float playerScanningRange = 10.0f;
    [SerializeField] protected float playerAttackRange = 5.0f;
    [SerializeField] protected float moveSpeed = 3.0f;

    public bool attackFinished = true;

    // Debug
    [SerializeField] private string currentStateDebug;
    [SerializeField] private LineRenderer shotLine;
    private float shotLineTime = 0.06f;

    public NavMeshAgent agent;
    public HealthController healthController;
    public UtilitySystem utilitySystem;
    public EnemyUIController enemyUIController;

    private Transform target;
    public StateMachine fsm;
    public Animator animator;
    private EnemyStates currentRequestedState;

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

    public bool HasFinshedAttack()
    {
        return attackFinished;
    }

    public void StayStill()
    {
        agent.ResetPath();
    }

    public void EndAnimationEvent()
    {
        attackFinished = true;
    }

    public Vector3 GetFleePoint(float fleeDistance, float sampleRadius = 2f, int maxTries = 6)
    {
        if (target == null || agent == null)
            return transform.position;

        Vector3 fleeDirection = transform.position - target.position;
        fleeDirection.y = 0f;

        if (fleeDirection.sqrMagnitude < 0.0001f)
            fleeDirection = -transform.forward;

        fleeDirection.Normalize();

        for (int i = 0; i < maxTries; i++)
        {
            float distanceFactor = 1f - (i * 0.15f);
            float currentDistance = Mathf.Max(1f, fleeDistance * distanceFactor);

            Vector3 candidate = transform.position + fleeDirection * currentDistance;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();

                if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                    return hit.position;
            }
        }

        return transform.position;
    }

    public void Flee(float fleeDistance)
    {
        Vector3 fleePoint = GetFleePoint(fleeDistance);
        MoveTowardsPoint(fleePoint);
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
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction);
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

    public bool InAttackRange()
    {
        return Vector3.Distance(target.position, transform.position) < playerAttackRange;
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
        utilitySystem = GetComponent<UtilitySystem>();
        enemyUIController = GetComponent<EnemyUIController>();
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
        EnemyStates bestState = utilitySystem.GetBestState();

        if (bestState != currentRequestedState)
        {
            fsm.RequestStateChange(bestState.ToString());
            currentRequestedState = bestState;
        }

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

    // Implementación base del score, las clases derivadas deberían sobreescribir el score si se quiere cambiar el comportamiento.
    public float GetScore(EnemyStates state)
    {
        float playerDistance = Vector3.Distance(target.transform.position, transform.position);

        switch (state)
        {
            case EnemyStates.Wandering:
                return playerDistance > playerScanningRange ? 0.2f : 0f;
            case EnemyStates.Combat:
                if (playerDistance > playerScanningRange)
                    return 0f;
                return 1f - (playerDistance / playerScanningRange);
            case EnemyStates.Flee:
                return healthController.Health.Value / healthController.MaxHealth.Value < 0.2 ? 1.0f : 0.0f;
            case EnemyStates.Dead:
                return IsDead() ? 1 : 0;
        }
        Debug.Log("Estado no manejado.");
        return 0f;
    }

}