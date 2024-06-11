using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShootStrategy : iShootStrategy
{
    ShootInteractor shootInteractor;
    Transform shootPoint;

    public BulletShootStrategy (ShootInteractor _shootInteractor)
    {
        Debug.Log("Switched to firing bullets");
        shootInteractor = _shootInteractor;
        shootPoint = shootInteractor.GetShootPoint();

        shootInteractor.gunRenderer.material.color = shootInteractor.bulletColour;
    }

    public void Shoot()
    {
        PooledObject pooledBullet = shootInteractor.bulletPool.GetPooledObject();
        pooledBullet.gameObject.SetActive(true);

        Rigidbody bullet = pooledBullet.GetComponent<Rigidbody>();
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;

        bullet.velocity = shootPoint.forward * shootInteractor.GetShootVelocity();

        shootInteractor.bulletPool.DestroyPooledObject(pooledBullet, 5.0f);
    }
}