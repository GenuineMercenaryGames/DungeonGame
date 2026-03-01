using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float minValue;

    public float Value { get { return currentValue; } set { currentValue = value; } }
    public float MinValue { get { return minValue; } set { minValue = value; } }
    public float MaxValue { get { return maxValue; } set { maxValue = value; } }

}
