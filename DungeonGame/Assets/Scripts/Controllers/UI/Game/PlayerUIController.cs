using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Image imageHealth;
    [SerializeField] private TMP_Text textCoins;
    [SerializeField] private TMP_Text textAmmo;

    void Start()
    {
        var player = PlayerManager.Instance.Player;
        player.Coins.AddListener(UpdateCoins);
        player.Coins.Notify();
    }

    void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        float health = PlayerManager.Instance.Player.healthController.Health.Value;
        float maxHealth = PlayerManager.Instance.Player.healthController.MaxHealth.Value;
        imageHealth.fillAmount = Mathf.Lerp(imageHealth.fillAmount, health / maxHealth, Time.deltaTime * lerpSpeed);
    }

    private void UpdateCoins(int coins)
    {
        textCoins.text = $"{coins}";
    }
}
