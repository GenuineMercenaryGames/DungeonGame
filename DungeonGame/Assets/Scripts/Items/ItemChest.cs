using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    [SerializeField] private List<ItemDefinitionBase> m_itemPool;
    private List<float> m_itemProbabilities;
    private float m_totalProbWeight;

    [SerializeField] private ItemPickup itemInstance;
    [SerializeField] private Transform m_spawnPoint;

    private Animator m_animator;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        if (m_itemPool.Count > 0)
        {
            BuildProbabilityWeight();

            m_animator.SetTrigger("OpenChest");

            //DropItem();
        }
        else
            Debug.LogWarning("Chest item pool has no items assigned!");
    }

    public void DropItem()
    {
        ItemDefinitionBase item = GetRandomItem();

        if(item == null)
        {
            Debug.LogWarning("ItemChest::DropItem has not returned any valid item to drop");
            return;
        }

        ItemPickup pickup = Instantiate(itemInstance, m_spawnPoint.position, Quaternion.identity);
        pickup.CreateItemPickup(item);
    }

    private ItemDefinitionBase GetRandomItem()
    {
        if(m_itemPool == null || m_itemPool.Count == 0)
        {
            Debug.LogWarning("ItemChest::GetRandomItem has no items to drop!");
            return null;
        }
        
        float random = UnityEngine.Random.Range(0.0f, m_totalProbWeight);
        float cumulative = 0.0f;

        for(int i = 0; i < m_itemPool.Count; i++)
        {
            cumulative += m_itemProbabilities[i];

            if(random <= cumulative)
            {
                ItemDefinitionBase item = m_itemPool[i];
                RemoveItemFromPool(i);

                return item;
            }
        }

        return m_itemPool[m_itemPool.Count - 1];
    }

    private void BuildProbabilityWeight()
    {
        m_itemProbabilities = new List<float>();
        m_totalProbWeight = 0.0f;
        float weight = 0.0f;

        for (int i = 0; i < m_itemPool.Count; i++)
        {
            ItemDefinitionBase item = m_itemPool[i];

            if(!item)
            {
                m_itemProbabilities[i] = 0.0f;
                continue;
            }

            weight = GetRarityWeight(item.Rarity);
            m_itemProbabilities.Add(weight);
            m_totalProbWeight += weight;
        }
    }

    private float GetRarityWeight(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.COMMON: return 40f;
            case ItemRarity.UNCOMMON: return 25f;
            case ItemRarity.RARE: return 20f;
            case ItemRarity.EPIC: return 10f;
            case ItemRarity.LEGENDARY: return 0.05f;
            default: return 0f;
        }
    }
    
    private void RemoveItemFromPool(int index)
    {
        Debug.Log("Removing item and probability in position " + index + "\n");
        m_totalProbWeight -= m_itemProbabilities[index];
        m_itemPool.RemoveAt(index);
        m_itemProbabilities.RemoveAt(index);
    }
}