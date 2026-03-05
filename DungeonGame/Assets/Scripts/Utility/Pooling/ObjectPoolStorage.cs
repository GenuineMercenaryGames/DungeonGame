using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolStorage
{
    #region Variables

    public Transform ParentTransform { get; set; }
    public GameObject Prefab { get; private set; }
    public Stack<GameObject> Objects { get; private set; }
    public int Capacity { get; private set; }
    public int RegrowFactor { get; set; }
    public bool CanRegrow { get; set; }

    #endregion

    #region Constructor

    public ObjectPoolStorage(Transform parentTransform, GameObject prefab, int initialCapacity = 20, bool allowRegrow = true, int regrowFactor = 2)
    {
        ParentTransform = parentTransform;
        Prefab = prefab;
        Capacity = initialCapacity;
        RegrowFactor = regrowFactor;
        CanRegrow = allowRegrow;
        Objects = new(Capacity);
        CreateInstances(Capacity);
    }

    #endregion

    #region PublicMethods

    public void Resize(int newCapacity)
    {
        if (newCapacity < Capacity)
            return;

        int count = newCapacity - Capacity;
        CreateInstances(count);
        Capacity = newCapacity;
    }

    public void Clear()
    {
        foreach (var obj in Objects)
        {
            obj.SetActive(false);
        }
    }

    public GameObject Get()
    {
        if (Objects.Count <= 0)
        {
            if (CanRegrow)
            {
                Resize(Capacity * 2);
            }
            else
            {
                return null;
            }
        }

        GameObject obj = Objects.Pop();
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        Objects.Push(obj);
    }

    public T Get<T>() where T : Component
    {
        var obj = Get();
        var component = obj.GetComponent<T>();
        if (component == null)
        {
            Return(obj);
            return null;
        }
        return component;
    }

    public void Return<T>(T component) where T : Component
    {
        Return(component.gameObject);
    }

    #endregion

    #region PrivateMethods

    private GameObject CreateInstance()
    {
        GameObject go = UnityInstantiate();
        PooledObject po = go.GetComponent<PooledObject>();
        if (po == null)
            po = go.AddComponent<PooledObject>();
        po.OwningPool = this;
        go.SetActive(false);
        return go;
    }

    private void CreateInstances(int n)
    {
        for (int i = 0; i < n; ++i)
        {
            GameObject obj = CreateInstance();
            Objects.Push(obj);
        }
    }

    private GameObject UnityInstantiate()
    {
        if (ParentTransform == null)
        {
            var go = GameObject.Instantiate(Prefab);
            return go;
        }
        else
        {
            var go = GameObject.Instantiate(Prefab, ParentTransform);
            return go;
        }
    }

    #endregion
}
