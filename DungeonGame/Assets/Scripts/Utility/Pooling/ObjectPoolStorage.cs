using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolStorage
{
    #region Variables

    private GameObject _prefab;
    private int _capacity;
    private Stack<GameObject> _objects;
    private bool _regrow;
    private ObjectPool _parent;

    #endregion

    #region PublicMethods

    public void Init(GameObject prefab, int capacity, bool regrow, ObjectPool parent)
    {
        _prefab = prefab;
        _capacity = capacity;
        _objects = new Stack<GameObject>(_capacity);
        _regrow = regrow;
        _parent = parent;
        PushN(_capacity);
    }

    public void Resize(int newCapacity)
    {
        if (newCapacity < _capacity)
            return;

        int count = newCapacity - _capacity;
        PushN(count);
        _capacity = newCapacity;
    }

    public void Clear()
    {
        foreach (var obj in _objects)
        {
            obj.SetActive(false);
        }
    }

    public GameObject Get()
    {
        if (_objects.Count <= 0)
        {
            if (_regrow)
            {
                Resize(_capacity * 2);
            }
            else
            {
                return null;
            }
        }

        GameObject obj = _objects.Pop();
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        _objects.Push(obj);
    }

    #endregion

    #region PrivateMethods

    private GameObject CreateObjectInstance()
    {
        // NOTE : Add further logic here if need be
        GameObject obj = GameObject.Instantiate(_prefab, _parent.transform);
        PooledObject pooled = obj.GetComponent<PooledObject>(); // NOTE : Can't add it all willy-nilly because some of the objects rely on having the ability to get this component on awake, so I guess it is the user's responsibility to add the component themselves...
        if (pooled == null) pooled = obj.AddComponent<PooledObject>();
        pooled.owningPool = _parent;
        obj.SetActive(false);
        return obj;
    }

    private void PushN(int n)
    {
        for (int i = 0; i < n; ++i)
        {
            GameObject obj = CreateObjectInstance();
            _objects.Push(obj);
        }
    }

    #endregion
}
