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
    public GameObject shotgunModel;

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
        shotgunModel.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Player input all only happens once with certain cooldown
        //Use selected Item
        if (playerInput.leftBtn && !itemOnCooldown)
        {
            itemOnCooldown = true;
            UseItem();
            Invoke(nameof(resetItemCooldown), itemCooldownTimer);
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
            if (selectedItem == PlayerInventory.instance.jumpPad)
            {
                //Do extra ground check
                if (PlayerMovementBehaviour.instance.isGrounded)
                {
                    selectedItem.Use();
                }
            }
            else
            {
                selectedItem.Use();
            }
            uiManager.updateWeaponAmount(selectedItem);
        }
        else
        {
            //Play some goofy sound maybe?
        }
    }

    private void ItemUp()
    {
        //Put shotgun away
        if (selectedItem == PlayerInventory.instance.shotgunShell)
        {
            shotgunModel.SetActive(false);
        }

        IUseableItem nextItem;
        if (items.IndexOf(selectedItem) < items.Count - 1)
        {
            nextItem = items[items.IndexOf(selectedItem) + 1];

        }else
        {
            nextItem = items[0];
        }
        Debug.Log("Changed to " + nextItem);
        IUseableItem prev = selectedItem;
        IUseableItem main = nextItem;
        IUseableItem next;
        if (items.IndexOf(nextItem) < items.Count - 1)
        {
            next = items[items.IndexOf(nextItem) + 1];
        }
        else
        {
            next = items[0];
        }

        selectedItem = nextItem;
        //Pull shotgun out
        if (nextItem == PlayerInventory.instance.shotgunShell)
        {
            shotgunModel.SetActive(true);
        }
        uiManager.updateWeapons(main, next, prev);
        
    }
    private void ItemDown()
    {
        //Put shotgun away
        if (selectedItem == PlayerInventory.instance.shotgunShell)
        {
            shotgunModel.SetActive(false);
        }

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
        IUseableItem prev = selectedItem;
        IUseableItem main = nextItem;
        IUseableItem next;
        if (items.IndexOf(nextItem) > 0)
        {
            next = items[items.IndexOf(nextItem) - 1];
        }
        else
        {
            next = items[items.Count - 1];
        }

        //Pull shotgun out
        if (nextItem == PlayerInventory.instance.shotgunShell)
        {
            shotgunModel.SetActive(true);
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