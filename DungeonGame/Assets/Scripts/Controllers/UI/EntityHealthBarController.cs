using UnityEngine;
using UnityEngine.UI;

public class EntityHealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    private HealthController health;

    void Start()
    {
        health = GetComponent<HealthController>();
        health.Health.AddListener(HealthHasChanged);
    }

    void HealthHasChanged(float currentHealth)
    {
        healthBar.fillAmount = currentHealth / health.MaxHealth.Value;
    }
}
