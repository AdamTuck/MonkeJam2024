using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : IUseableItem
{
    public BombItem(GameObject target)
    { 
        prefab = target;
    }

    public override void Use()
    {
        count--;
        GameObject created = PlayerInventory.instance.SpawnObject(prefab);
        created.GetComponent<Rigidbody>().AddForce((created.transform.forward + created.transform.up) * 10, ForceMode.Impulse);
    }
}
