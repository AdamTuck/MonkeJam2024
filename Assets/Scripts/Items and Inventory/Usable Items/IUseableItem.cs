using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "IUseableItem", menuName = "ScriptableObjects / SpawnManagerScriptableObject", order = 1)]
public abstract class IUseableItem : ScriptableObject
{
    public Transform playerTransform;
    public int count;
    public int cost;
    public int itemCooldown;
    public Sprite sprite;
    public GameObject spawnObject;
    //offset from player position
    public Vector3 spawnOffset;
    public abstract void Use();
}
