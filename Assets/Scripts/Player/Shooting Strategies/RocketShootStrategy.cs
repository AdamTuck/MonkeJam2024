using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShootStrategy : iShootStrategy
{
    ShootInteractor shootInteractor;
    Transform shootPoint;
    public RocketShootStrategy(ShootInteractor _shootInteractor)
    {
        Debug.Log("Switched to firing rockets");
        shootInteractor = _shootInteractor;
        shootPoint = shootInteractor.GetShootPoint();

        shootInteractor.gunRenderer.material.color = shootInteractor.rocketColour;
    }

    public void Shoot()
    {
        PooledObject pooledRocket = shootInteractor.rocketPool.GetPooledObject();
        pooledRocket.gameObject.SetActive(true);

        Rigidbody rocket = pooledRocket.GetComponent<Rigidbody>();
        rocket.transform.position = shootPoint.position;
        rocket.transform.rotation = shootPoint.rotation;

        rocket.velocity = shootPoint.forward * shootInteractor.GetShootVelocity();

        shootInteractor.rocketPool.DestroyPooledObject(pooledRocket, 5.0f);
    }
}