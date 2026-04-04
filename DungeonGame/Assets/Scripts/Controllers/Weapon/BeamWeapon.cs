using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class BeamWeapon : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    private WeaponController wc;
    private bool isShooting;

    void Start()
    {
        SetLinePoints(Vector3.zero, Vector3.zero);
        wc = GetComponent<WeaponController>();
        isShooting = false;
    }

    void Update()
    {
        if (isShooting)
            BeamThink();
    }

    public void StartBeam()
    {
        isShooting = true;
    }

    public void StopBeam()
    {
        isShooting = false;
        SetLinePoints(Vector3.zero, Vector3.zero);
    }

    private void BeamThink()
    {
        float maxDistance = 1000.0f; // NOTE : Should be a configurable variable, but hardcoded is ok for now.
        var origin = wc.bulletSpawnTransform.position;
        var direction = wc.bulletSpawnTransform.forward;
        bool hasHit = Physics.Raycast(origin, direction, out var hitInfo, maxDistance);
        var hitPoint = hasHit ? hitInfo.point : origin + direction * maxDistance;

        // NOTE : Temporarily just have the line renderer be part of the weapon. Later on, implement a line visualizer game object with a pool so that this code
        // can be moved from using this components system with events to a weapon interface system more easily.
        // The line renderer would be attacked to a beam prefab, and we'd pool that and use it while we need to, then return it.
        SetLinePoints(origin, hitPoint);

        if (hasHit)
        {
            if (hitInfo.collider.TryGetComponent<HealthController>(out var health))
            {
                health.Health.Value -= /*wc.Damage*/ 1.0f * Time.deltaTime; // Temporary hack because I lost the fucking damage field...
            }
        }
    }

    private void SetLinePoints(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPositions(new Vector3[] { start, end });
    }
}
