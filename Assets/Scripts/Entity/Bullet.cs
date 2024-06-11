using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        IDestroyable destroyable = collision.gameObject.GetComponent<IDestroyable>();
        
        if (destroyable != null)
        {
            destroyable.OnCollided();
        }

        if (!collision.gameObject.CompareTag("Player"))
            gameObject.GetComponent<PooledObject>().Destroy();
    }
}