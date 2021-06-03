using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public int _amountToPool;
        public GameObject _objectToPool;
        public bool _shouldExpand = true;
    }

    public static ObjectPooler SharedInstance;

    private List<GameObject> _pooledObjects;

    public List<ObjectPoolItem> _itemsToPool;


    private void Awake()
    {
        SharedInstance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        _pooledObjects = new List<GameObject>();

        foreach (ObjectPoolItem item in _itemsToPool)
        {
            for (int i = 0; i < item._amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item._objectToPool);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy && _pooledObjects[i].tag == tag)
            {
                return _pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in _itemsToPool)
        {
            if (item._objectToPool.tag == tag)
            {
                if (item._shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item._objectToPool);
                    obj.SetActive(false);
                    _pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
