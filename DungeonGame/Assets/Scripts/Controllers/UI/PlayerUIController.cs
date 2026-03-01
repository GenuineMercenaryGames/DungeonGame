using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text textHealth;
    [SerializeField] private TMP_Text textShield;
    [SerializeField] private TMP_Text textCoins;

    public void Init()
    {
        var player = PlayerManager.Instance.Player;
        player.healthController.Health.OnValueChanged += UpdateHealth;
        player.Coins.OnValueChanged += UpdateCoins;
    }

    private void UpdateHealth(float _, float value)
    {
        textHealth.text = $"health: {value}";
    }

    private void UpdateCoins(int _, int value)
    {
        textCoins.text = $"coins: {value}";
    }
}
