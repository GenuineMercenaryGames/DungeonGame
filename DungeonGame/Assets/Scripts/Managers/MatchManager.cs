using UnityEngine;

public class MatchManager : SingletonPersistent<MatchManager>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void SpawnMatchManagerInstance()
    {
        var go = new GameObject("Match Manager Instance");
        var comp = go.AddComponent<MatchManager>();
    }
}
