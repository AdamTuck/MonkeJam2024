using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunItem : IUseableItem
{
    public ShotgunItem(GameObject target) 
    {

    }

    public override void Use()
    {
        count--;
        Debug.Log("Used Shotgun");
    }
}
