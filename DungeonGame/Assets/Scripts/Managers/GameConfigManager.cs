using UnityEngine;

public class GameConfigManager : SingletonPersistent<GameConfigManager>
{
    public GameObject selectedWeaponPrimary;
    public GameObject selectedWeaponSecondary;

    public int selectedSkin;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void SpawnMatchManagerInstance()
    {
        var go = new GameObject("Game Config Manager Instance");
        var comp = go.AddComponent<GameConfigManager>();
    }
}
