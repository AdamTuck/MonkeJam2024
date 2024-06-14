using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "IUseableItem", menuName = "ScriptableObjects / SpawnManagerScriptableObject", order = 1)]
public abstract class IUseableItem
{
    public int count = 10;
    public int itemCooldown;
    public GameObject prefab;
    public abstract void Use();
}
