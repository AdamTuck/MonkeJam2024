using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Rocket Engine", menuName = "ScriptableObjects / Useable Items / Rocket Engine", order = 4)]
public class RocketEngineItem : IUseableItem
{
    

    public override void Use()
    {
        count--;
        Debug.Log("Used Rocket Engine");
        PlayerInventory.instance.speedMultiplier = 2;
    }
}
