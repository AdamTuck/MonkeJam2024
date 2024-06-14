using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItemMono : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bombable"))
        {
            Debug.Log("Bomb hit destroyable Object");
        }
    }
}
