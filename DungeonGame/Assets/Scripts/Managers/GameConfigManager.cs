using UnityEngine;

public class GameConfigManager : SingletonPersistent<GameConfigManager>
{
    public GameObject selectedWeaponPrimary;
    public GameObject selectedWeaponSecondary;
    public Color skinColor = new Color(255.0f / 255.0f, 178.0f / 255.0f, 0.0f / 255.0f, 1.0f); // Default Sweeper yellow hazmat color.

    public int selectedSkin;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void SpawnMatchManagerInstance()
    {
        var go = new GameObject("Game Config Manager Instance");
        var comp = go.AddComponent<GameConfigManager>();
    }
}
