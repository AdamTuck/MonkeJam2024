using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    ObjectPool objectPool;

    private float timer;
    private bool setToDestroy = false;
    private float destroyTime = 0;

    public void SetObjectPool(ObjectPool pool)
    {
        objectPool = pool;
        timer = 0;
        destroyTime = 0;
        setToDestroy = false;
    }

    void Update()
    {
        if (setToDestroy)
        {
            timer += Time.deltaTime;

            if (timer >= destroyTime)
            {
                timer = 0;
                setToDestroy = false;

                Destroy();
            }
        } 
    }

    public void Destroy()
    {
        if (objectPool != null)
        {
            objectPool.RestoreObject(this);
        }
    }

    public void Destroy(float time)
    {
        setToDestroy = true;
        destroyTime = time;
    }
}