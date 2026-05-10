using UnityEngine;

public static class CoinTypeData
{
    public enum CoinType
    {
        Bronze = 0,
        Silver,
        Gold,
        Blue,
        Purple,
    }

    public static int[] CoinValue = {
        2,
        10,
        100,
        1000,
        2500
    };

    public static int GetCoinValue(int index)
    {
        if (index < 0 || index >= CoinValue.Length)
            return 0;
        return CoinValue[index];
    }

    public static int GetCoinValue(CoinType type)
    {
        return GetCoinValue((int)type);
    }
}

// NOTE : Maybe I should implement a generic pickup-able item interface of sorts? or some sort of inheritance bullshit.
// Not sure, I believe that this is good as it is, with a different component for each item type, or maybe this being configurable or whatever, but who knows.
// We'll see what's best later on.
public class CoinController : MonoBehaviour
{
    [SerializeField] private CoinTypeData.CoinType coinType;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            SfxManager.Instance.PlaySfx("coin");
            player.Coins.Value += CoinTypeData.GetCoinValue(coinType);
            DestroyCoin();
        }
    }

    private void DestroyCoin()
    {
        // TODO : Modify logic once coin object pooling is implemented.
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
