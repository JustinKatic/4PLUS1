using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPooler : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public int _amountToPool;
        public GameObject _objectToPool;
        public bool _shouldExpand = true;
    }

    public struct SoundObject
    {
        public GameObject WorldObject;

        public AudioSource Source;

        public SoundObject(GameObject obj)
        {
            WorldObject = obj;

            Source = obj.GetComponent<AudioSource>();
        }
    }

    public static SoundPooler SharedInstance;

    private List<SoundObject> _pooledObjects;

    public List<ObjectPoolItem> _itemsToPool;


    private void Awake()
    {
        SharedInstance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        _pooledObjects = new List<SoundObject>();

        foreach (ObjectPoolItem item in _itemsToPool)
        {
            for (int i = 0; i < item._amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item._objectToPool,transform);
                obj.SetActive(false);
                _pooledObjects.Add(new SoundObject(obj));
            }
        }
    }

    public SoundObject GetPooledObject(string tag)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].WorldObject.activeInHierarchy && _pooledObjects[i].WorldObject.tag == tag)
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
                    GameObject obj = (GameObject)Instantiate(item._objectToPool,transform);
                    obj.SetActive(false);
                    SoundObject deb = new SoundObject(obj);
                    _pooledObjects.Add(deb);
                    return deb;
                }
            }
        }
        return new SoundObject();
    }
}
