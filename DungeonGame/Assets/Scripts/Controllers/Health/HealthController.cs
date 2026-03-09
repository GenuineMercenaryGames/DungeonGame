using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    public ObservableVariable<float> Health = new();
    public ObservableVariable<float> MaxHealth = new();

    public Image healthBar;
    public bool IsDead => Health.GetValue() <= 0;

    void Awake()
    {
        if(maxHealth <= 0.0f)
            maxHealth = 1.0f;

        Health.AddPreprocessor(ClampHealthValue);
        MaxHealth.AddPreprocessor(ClampMaxHealthValue);

        Health.AddListener(OnHealthChanged);

        MaxHealth.Value = maxHealth;
        Health.Value = maxHealth;
    }

    // Limit the current health value to a value in range [0, maxHealth].
    private void ClampHealthValue(out float outHealth, float inHealth)
    {
        outHealth = Mathf.Clamp(inHealth, 0.0f, MaxHealth.GetValue());
    }

    // Limit the max health value to 1.0f at the very least.
    private void ClampMaxHealthValue(out float outMaxHealth, float inMaxHealth)
    {
        outMaxHealth = Mathf.Max(1.0f, inMaxHealth);
    }

    private void OnHealthChanged(float oldValue, float newValue)
    {
        if (newValue <= 0f)
        {
            Object.FindFirstObjectByType<MenuUIManager>().ShowGameOver();
            Time.timeScale = 0f;
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
