using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private PlayerInventory inventory;
    public Canvas shopWindow;
    public List<GameObject> toDisable;

    [Header("Text to update")]
    public TextMeshProUGUI scrapText;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI shotgunText;
    public TextMeshProUGUI jumppadText;
    public TextMeshProUGUI rocketEngineText;

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

    public void Start()
    {
        inventory = PlayerInventory.instance;
        playerInput = PlayerInput.instance;
        shopWindow.enabled = false;
    }
    public void Update()
    {
        //Update text only if shop window is open
        if (shopWindow.enabled)
        {
            scrapText.text = "Scrap: " + inventory.scrap.ToString();
            bombText.text = inventory.bomb.count + "x bombs";
            shotgunText.text = inventory.shotgunShell.count + "x shotgun shells";
            jumppadText.text = inventory.jumpPad.count + "x jump pads";
            rocketEngineText.text = inventory.rocketEngine.count + "x rocket engines";
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

    //----------------------------- ITEM SHOP -----------------------------//
    public void buyBomb()
    {
        inventory.scrap -= inventory.bomb.cost;
        inventory.bomb.count++;
    }
    public void buyShotgunShell()
    {
        inventory.scrap -= inventory.shotgunShell.cost;
        inventory.shotgunShell.count++;
    }
    public void buyJumpPad()
    {
        inventory.scrap -= inventory.jumpPad.cost;
        inventory.jumpPad.count++;
    }
    public void buyRocketEngine()
    {
        inventory.scrap -= inventory.rocketEngine.cost;
        inventory.rocketEngine.count++;
    }

    //----------------------------- BIKE SHOP -----------------------------//
    public void buyBike1()
    {

    }
    public void buyBike2()
    {

    }
    public void buyBike3()
    {

    }
    public void buyBike4()
    {

    }
}
