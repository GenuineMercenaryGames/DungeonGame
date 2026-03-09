using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text textHealth;
    [SerializeField] private TMP_Text textShield;
    [SerializeField] private TMP_Text textCoins;

    //[SerializeField] private UnityEngine.UI.Image healthBar;
    //[SerializeField] private TMP_Text coinCounter;

    private HealthController healthController;

    public void Init()
    {
        var player = PlayerManager.Instance.Player;

        healthController = player.healthController;

        player.healthController.Health.AddListener(UpdateHealth);
        player.Coins.AddListener(UpdateCoins);

        player.healthController.Health.Notify();
        player.Coins.Notify();
    }

    private void UpdateHealth(float _, float value)
    {
        textHealth.text = $"health: {value}";

        float maxHealth = healthController.MaxHealth.GetValue();
        float target = value / maxHealth;

    }

    private void UpdateCoins(int _, int value)
    {
        textCoins.text = $"coins: {value}";
    }
}
