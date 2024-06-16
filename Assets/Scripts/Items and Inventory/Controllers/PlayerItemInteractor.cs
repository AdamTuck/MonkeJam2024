using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemInteractor : MonoBehaviour
{
    public static PlayerItemInteractor instance;
    private PlayerInput playerInput;
    private UIManager uiManager;


    public List<IUseableItem> items;
    public IUseableItem selectedItem;

    private bool itemOnCooldown;
    public int itemCooldownTimer;

    public Transform itemPos;

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
        uiManager = UIManager.instance;
        selectedItem = items[0];
        itemOnCooldown = false;
    }

    private void Update()
    {
        //Player input all only happens once with certain cooldown
        //Use selected Item
        if (playerInput.leftBtn && !itemOnCooldown)
        {
            itemOnCooldown = true;
            UseItem();
            Invoke(nameof(resetItemCooldown), 1);
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
        Sprite prev = selectedItem.sprite;
        Sprite main = nextItem.sprite;
        Sprite next;
        if (items.IndexOf(nextItem) < items.Count - 1)
        {
            next = items[items.IndexOf(nextItem) + 1].sprite;
        }
        else
        {
            next = items[0].sprite;
        }

        selectedItem = nextItem;
        uiManager.updateWeapons(main, next, prev);
        
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
        Sprite prev = selectedItem.sprite;
        Sprite main = nextItem.sprite;
        Sprite next;
        if (items.IndexOf(nextItem) > 0)
        {
            next = items[items.IndexOf(nextItem) - 1].sprite;
        }
        else
        {
            next = items[items.Count - 1].sprite;
        }

        selectedItem = nextItem;
        uiManager.updateWeapons(main, next, prev);
    }


    #region ResetFunctions
    private void resetItemCooldown()
    {
        itemOnCooldown = false;
    }
    public void ResetRocketBoost()
    {
        Invoke(nameof(RocketBoostReset), 5);
    }
    private void RocketBoostReset() 
    {
        PlayerMovementBehaviour.instance.rocketEngine = false;
    }
    #endregion ResetFunctions
}