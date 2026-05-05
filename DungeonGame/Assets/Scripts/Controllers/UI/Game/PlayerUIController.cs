using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float lerpSpeed;

    [Header("Component References")]
    [SerializeField] private Image imageHealth;
    [SerializeField] private TMP_Text textCoins;
    [SerializeField] private TMP_Text textAmmo;

    [Header("Portrait")]
    [SerializeField] private Image[] facePortraitStates;

    void Start()
    {
        var player = PlayerManager.Instance.Player;
        player.Coins.AddListener(UpdateCoins);
        player.Coins.Notify();
        player.healthController.Health.AddListener(UpdatePortrait);
        player.healthController.MaxHealth.AddListener(UpdatePortrait);
        player.healthController.Health.Notify();
        player.healthController.MaxHealth.Notify();
    }

    void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        float health = GetPlayerHealth();
        float maxHealth = GetPlayerMaxHealth();
        imageHealth.fillAmount = Mathf.Lerp(imageHealth.fillAmount, health / maxHealth, Time.deltaTime * lerpSpeed);
    }

    private void UpdatePortrait()
    {
        float health = GetPlayerHealth();
        float maxHealth = GetPlayerMaxHealth();
        float fraction = health / maxHealth;
        int index = (int)(fraction * facePortraitStates.Length);
        for (int i = 0; i < facePortraitStates.Length; ++i)
            facePortraitStates[i].enabled = false;
        facePortraitStates[index].enabled = true;
    }

    private void UpdateCoins(int coins)
    {
        textCoins.text = $"{coins}";
    }

    private float GetPlayerHealth() { return PlayerManager.Instance.Player.healthController.Health.Value; }
    private float GetPlayerMaxHealth() { return PlayerManager.Instance.Player.healthController.MaxHealth.Value; }
}
