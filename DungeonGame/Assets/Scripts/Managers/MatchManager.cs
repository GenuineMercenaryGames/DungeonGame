using UnityEngine;

public class MatchManager : SingletonPersistent<MatchManager>
{
    public GameObject selectedWeaponPrimary;
    public GameObject selectedWeaponSecondary;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void SpawnMatchManagerInstance()
    {
        var go = new GameObject("Match Manager Instance");
        var comp = go.AddComponent<MatchManager>();
    }
}
