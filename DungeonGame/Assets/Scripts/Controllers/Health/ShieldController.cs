using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float rechargeRate;

    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0.0f;
    }

    void Update()
    {
        if (elapsedTime >= rechargeTime && currentValue < maxValue)
        {
            currentValue = Mathf.Clamp(currentValue + rechargeRate * Time.deltaTime, 0.0f, maxValue);
        }

        elapsedTime += Time.deltaTime;
    }

}
