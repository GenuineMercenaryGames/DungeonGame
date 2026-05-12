using System.Collections;
using TMPro;
using UnityEngine;

public class VictoryUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text moneyText;

    public void Start()
    {
        gameObject.SetActive(false); // Temporary hack to hide the UI. Once animations are added, this will not be required.
    }

    public void OnDisable()
    {
        StopAllCoroutines(); // Just in case
    }

    public void RunVictory()
    {
        killsText.text = "0";
        moneyText.text = "0";
        StartCoroutine(ScoreAnimation());
    }

    private IEnumerator ScoreAnimation()
    {
        var wait1sec = new WaitForSeconds(1.0f);
        var waitmsec = new WaitForSeconds(0.06f);

        yield return wait1sec;

        int kills = 0;
        int money = 0;
        int targetKills = PlayerManager.Instance.Player.Kills.Value;
        int targetMoney = PlayerManager.Instance.Player.Coins.Value;

        while (kills < targetKills)
        {
            int increment = Mathf.Max(1, (targetKills - kills) / 10);
            kills = Mathf.Min(kills + increment, targetKills);
            killsText.text = $"{kills}";
            SoundManager.Instance.PlaySound("WeakClick");
            yield return waitmsec;
        }
        killsText.text = $"{targetKills}";

        while (money < targetMoney)
        {
            int increment = Mathf.Max(1, (targetMoney - money) / 8);
            money = Mathf.Min(money + increment, targetMoney);
            moneyText.text = $"{money}";
            SoundManager.Instance.PlaySound("WeakClick");
            yield return waitmsec;
        }
        moneyText.text = $"{targetMoney}";
    }

}
