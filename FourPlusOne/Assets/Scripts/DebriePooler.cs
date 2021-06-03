using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebriePooler : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public int _amountToPool;
        public GameObject _objectToPool;
        public bool _shouldExpand = true;
    }

    public struct Debrie
    {
        public GameObject WorldObject;

        public MeshFilter Filter;
        public MeshRenderer Renderer;
        public MeshCollider Collider;
        public Rigidbody Body;

        public Debrie(GameObject obj)
        {
            WorldObject = obj;

            Filter = obj.GetComponent<MeshFilter>();
            Renderer = obj.GetComponent<MeshRenderer>();
            Collider = obj.GetComponent<MeshCollider>();
            Body = obj.GetComponent<Rigidbody>();
        }
    }

    public static DebriePooler SharedInstance;

    private List<Debrie> _pooledObjects;

    public List<ObjectPoolItem> _itemsToPool;


    private void Awake()
    {
        SharedInstance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        _pooledObjects = new List<Debrie>();

        foreach (ObjectPoolItem item in _itemsToPool)
        {
            for (int i = 0; i < item._amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item._objectToPool,transform);
                obj.SetActive(false);
                _pooledObjects.Add(new Debrie(obj));
            }
        }
    }

    public Debrie GetPooledObject(string tag)
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
                    Debrie deb = new Debrie(obj);
                    _pooledObjects.Add(deb);
                    return deb;
                }
            }
        }
        return new Debrie();
    }
}
