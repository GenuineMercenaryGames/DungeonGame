using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]

public enum ItemRarity
{
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY
}

/// <summary>
/// Base of any item defined as asset. All items will expand this.
/// </summary>
public abstract class ItemDefinitionBase : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string id = Guid.NewGuid().ToString(); // Unique id
    [SerializeField] private string displayName;                    // Display name for the item

    [Header("Presentation")]
    [SerializeField] private Sprite icon;                           // Icom to display in UIs
    [TextArea(2, 6)]
    [SerializeField] private string description;                    // Description of the item

    [Header("Meta")]
    [SerializeField] private ItemRarity rarity = ItemRarity.COMMON; // Rarity of the item for future features

    // Opcional: prefab que aparece en el suelo/tienda
    [Header("World (Optional)")]
    [SerializeField] private GameObject pickupPrefab;               // To display the item in the scene. MESH RENDERER for the pickup

    public string Id
    {
        get { return id; }
    }
    public string DisplayName
    {
        get { return displayName; }
    }
    public Sprite Icon {
        get { return icon; }
    }
    public string Description
    {
        get { return description; }
    }
    public ItemRarity Rarity
    {
        get { return rarity; }
    }
    public GameObject PickupPrefab
    {
        get { return pickupPrefab; }
    }

#if UNITY_EDITOR
    // Para evitar duplicados al duplicar assets desde el editor:
    protected virtual void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
            id = Guid.NewGuid().ToString();
    }
#endif
}
