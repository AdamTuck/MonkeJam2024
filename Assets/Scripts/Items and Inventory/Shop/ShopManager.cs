using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private PlayerInventory inventory;
    public Canvas shopWindow;

    [Header("Cost of bike upgrades")]
    public int staminaCost;
    public int accelerationCost;
    public int decelerationCost;
    public int armorCost;

    [Header("Text to update")]
    public TextMeshProUGUI scrapText;
    public TextMeshProUGUI cannotBuyMessage;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI shotgunText;
    public TextMeshProUGUI jumppadText;
    public TextMeshProUGUI rocketEngineText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI accelerationText;
    public TextMeshProUGUI brakesText;
    public TextMeshProUGUI armorText;

    private PlayerInput playerInput;
    public static ShopManager instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }
    [Header("Refrences")]
    public GameObject player;
    private PlayerMovementBehaviour playerMovement;
    private Health playerHealth;

    public void Start()
    {
        inventory = PlayerInventory.instance;
        playerInput = PlayerInput.instance;
        playerMovement = player.GetComponent<PlayerMovementBehaviour>();
        playerHealth = player.GetComponent<Health>();
        shopWindow.enabled = false;

    }
    public void Update()
    {
        //Update text only if shop window is open
        if (shopWindow.enabled)
        {
            scrapText.text = "Scrap: " + inventory.scrap;
            bombText.text = inventory.bomb.count + "x bombs";
            shotgunText.text = inventory.shotgunShell.count + "x shotgun shells";
            jumppadText.text = inventory.jumpPad.count + "x jump pads";
            rocketEngineText.text = inventory.rocketEngine.count + "x rocket engines";
            //Bike upgrades
            staminaText.text = "Stamina " + playerMovement.topEndurance;
            accelerationText.text = "Acceleration " + playerMovement.accelerationRate * 100;
            brakesText.text =  "Brakes " + playerMovement.decelerationRate * 100;
            armorText.text = "Armor " + playerHealth.maxHealth;
        }
    }

    public void openShop()
    {
        shopWindow.enabled = true;
        playerInput.LockInputs();
    }

    public void closeShop()
    {
        shopWindow.enabled = false;
        playerInput.UnlockInputs();
    }

    private void CannotBuy()
    {
        //Display cannot buy message
        cannotBuyMessage.enabled = true;
        Invoke(nameof(ResetCannotBuyMessage), 3);
    }
    private void ResetCannotBuyMessage()
    {
        cannotBuyMessage.enabled = false;
    }

    //----------------------------- ITEM SHOP -----------------------------//
    public void buyBomb()
    {
        if (inventory.scrap - inventory.bomb.cost >= 0)
        {
            inventory.scrap -= inventory.bomb.cost;
            inventory.bomb.count++;
        }
        else
        {
            CannotBuy();
        }
    }
    public void buyShotgunShell()
    {
        if (inventory.scrap - inventory.shotgunShell.cost >= 0)
        {
            inventory.scrap -= inventory.shotgunShell.cost;
            inventory.shotgunShell.count++;
        }
        else
        {
            CannotBuy();
        }
    }
    public void buyJumpPad()
    {
        if (inventory.scrap - inventory.jumpPad.cost >= 0)
        {
            inventory.scrap -= inventory.jumpPad.cost;
            inventory.jumpPad.count++;
        }
        else
        {
            CannotBuy();
        }
    }
    public void buyRocketEngine()
    {
        if (inventory.scrap - inventory.rocketEngine.cost >= 0)
        {
            inventory.scrap -= inventory.rocketEngine.cost;
            inventory.rocketEngine.count++;
        }
        else
        {
            CannotBuy();
        }
    }

    //----------------------------- BIKE SHOP -----------------------------//
    public void buyBike1()
    {
        //Top speed or stamina upgrade
        if (inventory.scrap - staminaCost >= 0)
        {
            inventory.scrap -= staminaCost;
            playerMovement.topEndurance += 10;
        }
        else
        {
            CannotBuy();
        }
        
    }
    public void buyBike2()
    {
        if (inventory.scrap - accelerationCost >= 0)
        {
            inventory.scrap -= accelerationCost;
            playerMovement.accelerationRate += 0.01f;
        }
        else
        {
            CannotBuy();
        }
    }
    public void buyBike3()
    {
        if (inventory.scrap - decelerationCost >= 0)
        {
            inventory.scrap -= decelerationCost;
            playerMovement.decelerationRate += 0.01f;
        }
        else
        {
            CannotBuy();
        }
    }
    public void buyBike4()
    {
        if (inventory.scrap - armorCost >= 0)
        {
            inventory.scrap -= armorCost;
            playerHealth.maxHealth += 50;
        }
        else
        {
            CannotBuy();
        }
    }
}
