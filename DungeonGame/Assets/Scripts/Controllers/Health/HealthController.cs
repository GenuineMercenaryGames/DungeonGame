using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    public ObservableVariable<float> Health = new();
    public ObservableVariable<float> MaxHealth = new();

    void Awake()
    {
        if(maxHealth <= 0.0f)
            maxHealth = 1.0f;

        Health.OnValueChanged += ClampHealthValue;
        MaxHealth.OnValueChanged += ClampMaxHealthValue;

        MaxHealth.Value = maxHealth;
        Health.Value = maxHealth;
    }

    // Limit the current health value to a value in range [0, maxHealth].
    private void ClampHealthValue(float oldValue, float newValue)
    {
        Health.SetValueWithoutNotify(Mathf.Clamp(newValue, 0.0f, MaxHealth.GetValue()));
    }

    // Limit the max health value to 1.0f at the very least.
    private void ClampMaxHealthValue(float oldValue, float newValue)
    {
        MaxHealth.SetValueWithoutNotify(Mathf.Max(1.0f, newValue));
    }
}
