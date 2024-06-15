using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerItemInteractor : MonoBehaviour
{
    public static PlayerItemInteractor instance;
    public PlayerInput playerInput;


    public List<IUseableItem> items;
    public IUseableItem selectedItem;

    private bool itemOnCooldown;
    public int itemCooldownTimer;

    public Transform gunPos;

    private void Awake()
    {
        //Make PlayerItemInteractor singleton
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }

    void Start()
    {
        playerInput = PlayerInput.instance;
        items = PlayerInventory.instance.items;
        selectedItem = items[0];
    }

    private void Update()
    {
        //Player input all only happens once with certain cooldown
        //Use selected Item
        if (playerInput.leftBtn && !itemOnCooldown)
        {
            itemOnCooldown = true;
            UseItem();
            Invoke(nameof(resetItemCooldown), selectedItem.itemCooldown);
        }
        //Swap weapons
        if (playerInput.itemWheelUp)
        {
            ItemUp();
        }
        if (playerInput.itemWheelDown)
        {
            ItemDown();
        }

    }

    private void UseItem()
    {
        //Items can only be used if there are enough of them
        if (selectedItem.count > 0)
        {
            selectedItem.Use();
        }
        else
        {
            //Play some goofy sound maybe?
        }
    }

    private void ItemUp()
    {
        IUseableItem nextItem;
        if (items.IndexOf(selectedItem) < items.Count - 1)
        {
            nextItem = items[items.IndexOf(selectedItem) + 1];
        }else
        {
            nextItem = items[0];
        }
        Debug.Log("Changed to " + nextItem);
        selectedItem = nextItem;
    }
    private void ItemDown()
    {
        IUseableItem nextItem;
        if (items.IndexOf(selectedItem) > 0)
        {
            nextItem = items[items.IndexOf(selectedItem) - 1];
        }
        else
        {
            nextItem = items[items.Count - 1];
        }
        Debug.Log("Changed to " + nextItem);
        selectedItem = nextItem;
    }


    #region ResetFunctions
    private void resetItemCooldown()
    {
        itemOnCooldown = false;
    }
    #endregion ResetFunctions
}