using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    //Keeps track of amount of items in players inventory
    /*public int scrap;

    public int shotgunShellCount;
    public int bombCount;
    public int itemXCount;*/
    [Header("Items")]
    public IUseableItem bomb;
    public IUseableItem shotgunShell;
    public IUseableItem jumpPad;
    public IUseableItem rocketEngine;


    [Header("Stats")]
    public int scrap;

    //The item and the amount the player has
    //Ordered List of Useable Items that the player can cycle through
    public List<IUseableItem> items;
    //List of all items the player has
    //public List<Item> inventory;  

    private void Awake()
    {
        //Make Player inventory singleton
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }
        instance = this;


        //Make useable items list
        items = new List<IUseableItem>
        {
            //Order here dictates order you swap through the items
            { bomb },
            { shotgunShell },
            { rocketEngine },
            
        };

    }

    #region HelpFunctions

    

    #endregion HelpFunctions
}
