using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int startSize;

    [SerializeField] private List<PooledObject> objectPool = new List<PooledObject>();
    [SerializeField] private List<PooledObject> usedPool = new List<PooledObject>();

    private PooledObject tempObject;

    // Start is called before the first frame update
    void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < startSize; i++)
        {
            AddNewObject();
        }
    }

    void AddNewObject ()
    {
        tempObject = Instantiate(objectToPool, transform).GetComponent<PooledObject>();
        tempObject.gameObject.SetActive(false);
        tempObject.SetObjectPool(this);
        objectPool.Add(tempObject);
    }

    public PooledObject GetPooledObject()
    {
        PooledObject objToGet;

        if (objectPool.Count > 0)
        {
            objToGet = objectPool[0];
            usedPool.Add(objToGet);
            objectPool.RemoveAt(0);
        }
        else
        {
            // Adds new object with default values to the pool
            AddNewObject();
            objToGet = GetPooledObject();
        }

        objToGet.gameObject.SetActive(true);
        return objToGet;
    }

    public void DestroyPooledObject (PooledObject objToDestroy, float time = 0)
    {
        if (time == 0)
        {
            objToDestroy.Destroy();
        }
        else
        {
            objToDestroy.Destroy(time);
        }
    }

    public void RestoreObject (PooledObject objToRestore)
    {
        //Debug.Log("Restoring to pool: " + objToRestore.gameObject.name);
        objToRestore.gameObject.SetActive(false);
        usedPool.Remove(objToRestore);
        objectPool.Add(objToRestore);
    }
}