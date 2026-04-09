using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct VfxEntry
{
    public string Key;
    public PooledObject Prefab;
}

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [SerializeField] private VfxEntry[] vfx_entries;

    private Dictionary<string, PooledObject> vfx_dict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        vfx_dict = new Dictionary<string, PooledObject>();

        foreach (VfxEntry entry in vfx_entries)
        {
            if (!string.IsNullOrEmpty(entry.Key) && entry.Prefab != null)
            {
                vfx_dict[entry.Key] = entry.Prefab;
            }
        }
    }

    public void InstantiateVFX(string vfx_name, Vector3 position, float scale)
    {
        if (!vfx_dict.TryGetValue(vfx_name, out PooledObject prefab))
        {
            Debug.LogWarning("Instanciando un VFX que no existe: " + vfx_name);
            return;
        }

        PooledObject instance = Instantiate(prefab, position, prefab.transform.rotation);
        instance.transform.localScale = prefab.transform.localScale * scale;
    }

}