using UnityEngine;

public static class WeaponUtility
{
    public static Vector3 GetRandomSpreadDirection(Vector3 forward, float angle, float spreadScaleX, float spreadScaleY)
    {
        Vector2 randomCircle = Random.insideUnitCircle;
        float spreadRadius = Mathf.Tan(angle * Mathf.Deg2Rad * 0.5f);
        float spreadX = randomCircle.x * spreadRadius * spreadScaleX;
        float spreadY = randomCircle.y * spreadRadius * spreadScaleY;
        float spreadZ = 1.0f;
        Vector3 spread = new Vector3(spreadX, spreadY, spreadZ);
        spread = spread.normalized;
        Quaternion rot = Quaternion.LookRotation(forward);
        Vector3 direction = rot * spread;
        return direction;
    }

    public static Vector3 GetRandomSpreadDirection(Vector3 forward, float angle)
    {
        const float spreadScaleX = 1.0f;
        const float spreadScaleY = 0.1f;
        return GetRandomSpreadDirection(forward, angle, spreadScaleX, spreadScaleY);
        // NOTE: Using 0.0 on Y spread is a temporary hack to make it so that there is no vertical spread.
        // Leads to better gunplay on a top down game.
        // For better visuals and for a little bit of forced hit-miss mechanic, the spread on Y is very small instead of just plain 0.
    }
}
