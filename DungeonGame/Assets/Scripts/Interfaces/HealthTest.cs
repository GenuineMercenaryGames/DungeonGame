using UnityEngine;
using UnityEngine.UI;

public class HealthTest : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
            currentHealth -= 1f;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount,currentHealth / maxHealth,Time.deltaTime * 5f);
    }
}