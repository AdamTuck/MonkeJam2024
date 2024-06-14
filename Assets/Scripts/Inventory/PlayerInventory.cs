using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //Keeps track of amount of items in players inventory
    /*public int scrap;

    public int shotgunShellCount;
    public int bombCount;
    public int itemXCount;*/
    [Header("Prefabs")]
    public GameObject bomb;
    public GameObject shotgunShell;
    public GameObject jumpPad;

    public static PlayerInventory instance;

    //The item and the amount the player has
    public List<IUseableItem> items;
    public Dictionary<IUseableItem, int> inventory;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        //Make inventory
        items = new List<IUseableItem>
        {
            //All items that can be in the inventory
            //Order here dictates order you swap through the items
            /*{ new BombItem(bomb) },
            { new ShotgunItem(shotgunShell) },
            { new JumpPadItem(jumpPad) }*/
            { new BombItem(bomb) },
            { new ShotgunItem(shotgunShell) },
            { new JumpPadItem(jumpPad) }
        };

        inventory = new Dictionary<IUseableItem, int>();
        //Add all items which are useable
        foreach (IUseableItem item in items)
        {
            inventory[item] = item.count;
        }
    }

    public GameObject SpawnObject(GameObject target)
    {
        GameObject res = Instantiate(target, transform.position + transform.forward * 2, transform.rotation);
        return res;
    }
}
