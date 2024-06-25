using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject cutsceneScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject tutorialTextObj;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private float tutorialTimeout;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private TMP_Text txtSpeed;
    [SerializeField] private TMP_Text txtScrap;

    [Header("MissionUIRefs")]
    [SerializeField] private GameObject[] missionObjs;
    [SerializeField] private TMP_Text[] clientNameTxt;
    [SerializeField] private TMP_Text[] clientOrderTxt;
    [SerializeField] private TMP_Text[] clientDeadlineTxt;
    [SerializeField] private TMP_Text[] clientStatusTxt;

    [Header("Map Refs")]
    [SerializeField] private GameObject mapObject;

    [Header("Weapon Wheel")]
    [SerializeField] Image mainWeapon;
    [SerializeField] TextMeshProUGUI mainAmount;
    [SerializeField] Image nextWeapon;
    [SerializeField] Image prevWeapon;

    [Header("Other Refs")]
    [SerializeField] GameObject splashScreenObj;
    [SerializeField] CanvasGroup splashScreenBGObj;
    [SerializeField] CanvasGroup daySpashScreenTxt;
    [SerializeField] TextMeshProUGUI tooltipText;

    [SerializeField] TextMeshProUGUI quotaRemainingText;
    [SerializeField] TextMeshProUGUI currentDayTxt;

    public TMP_Text txtHealth;
    public GameObject gameOverText;
    public GameObject victoryText;

    private float tutorialTimer;
    private bool tutorialShowing;

    public static UIManager instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    void Start()
    {
        gameOverText.SetActive(false);
    }

    private void Update()
    {
        if (tutorialShowing)
            TutorialTimeout();
    }

    private void OnEnable()
    {
        playerHealth.OnHealthUpdated += OnHealthUpdate;
        playerHealth.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthUpdated -= OnHealthUpdate;
        playerHealth.OnDeath -= OnDeath; // Maybe don't need this
    }

    public void ShowTutorial (string tutorialType)
    {
        switch (tutorialType)
        {
            case "movementTutorial":
                tutorialText.text = "Use WASD to move & shift to pedal hard.\nMousewheel/Q to switch items, left click/R to use.\n";
                break;
            case "healthTutorial":
                tutorialText.text = "Avoid zombies while making deliveries.\nIf your health reduces to 0, the run ends.";
                break;
            case "roadTutorial":
                tutorialText.text = "Stay on the roads if you can!\nYou'll move faster and use less stamina.";
                break;
            case "staminaTutorial":
                tutorialText.text = "Stamina only drains while pedalling.\nCoast the bike (don't hold W) or come to a stop to slowly regain some stamina.";
                break;
            case "flashlightTutorial":
                tutorialText.text = "F to toggle on/off your bike light.";
                break;
            case "dayEndTutorial":
                tutorialText.text = "Once you've delivered your quota for the day, you can sleep.\nYou can always play later to earn more scrap if you want.\n\nDon't stay out too late.";
                break;
            case "statsTutorial":
                tutorialText.text = "Stamina and health recover after resting each night.";
                break;
        }

        tutorialTextObj.SetActive(true);
        tutorialTimer = 0;
        tutorialShowing = true;
    }

    private void TutorialTimeout ()
    {
        tutorialTimer += Time.deltaTime;

        if (tutorialTimer >= tutorialTimeout)
        {
            tutorialShowing = false;
            tutorialTimer = 0;
            tutorialTextObj.SetActive(false);
        }
    }

    void OnHealthUpdate (float health)
    {
        //txtHealth.text = "Health: " + Mathf.Ceil(health).ToString();
        healthBar.value = health/100;
    }

    void OnDeath ()
    {
        gameOverText.SetActive(true);
    }

    public void OnVictory ()
    {
        victoryText.SetActive(true);
    }

    public void OnRespawnUI ()
    {
        gameOverText.SetActive(false);
    }

    public void OnCutsceneBegin ()
    {
        cutsceneScreen.SetActive(true);
    }

    public void OnCutsceneEnd ()
    {
        cutsceneScreen.SetActive(false);
    }

    public void SetPause (bool setPauseOn)
    {
        if (setPauseOn)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SetStaminaBar(float staminaValue)
    {
        staminaBar.value = staminaValue;
    }

    public void SetSpeedNum (float speedNum)
    {
        txtSpeed.text = $"Speed: {Mathf.Round(speedNum*2.5f)} kph";
    }

    public void UpdateClientUI (int index, bool isActive, string _clientName, Texture _clientImage, string _order, float _deadline, string _status)
    {
        clientNameTxt[index].text = _clientName;
        clientOrderTxt[index].text = _order;
        clientDeadlineTxt[index].text = "" + TimeSpan.FromSeconds(_deadline).ToString("mm\\:ss");
        clientStatusTxt[index].text = _status;

        missionObjs[index].GetComponent<RawImage>().texture = _clientImage;

        missionObjs[index].SetActive(isActive);
    }

    public void UpdateClientUI(int index, bool isActive)
    {
        missionObjs[index].SetActive(isActive);
    }

    public void ToggleMapScreen()
    {
        if (mapObject.activeInHierarchy)
            mapObject.SetActive(false);
        else
            mapObject.SetActive(true);
    }

    public void TogglePause ()
    {
        if (pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            PlayerInput.instance.UnlockInputs();
            Time.timeScale = 1;
        }
        else
        {
            pauseScreen.SetActive(true);
            PlayerInput.instance.LockInputs();
            Time.timeScale = 0;
        }
    }

    public void StartDaySplashScreen()
    {
        StartCoroutine(AnimateIntro());
    }

    IEnumerator AnimateIntro()
    {
        splashScreenObj.SetActive(true);

        LeanTween.alphaCanvas(splashScreenBGObj, 1f, 0);
        LeanTween.alphaCanvas(daySpashScreenTxt, 1f, 2);
        yield return new WaitForSeconds(3);

        GameManager.instance.BeginCurrentDay();

        LeanTween.alphaCanvas(splashScreenBGObj, 0f, 3);
        yield return new WaitForSeconds(3);

        LeanTween.alphaCanvas(daySpashScreenTxt, 0f, 3);
        yield return new WaitForSeconds(3);

        splashScreenObj.SetActive(false);
    }

    public void UpdateCurrentDay(string dayNum)
    {
        currentDayTxt.text = "DAY " + dayNum;
    }

    public void UpdateQuotaRemaining(int quotaRemaining)
    {
        if (quotaRemaining > 0)
            quotaRemainingText.text = $"Quota Remaining: {quotaRemaining}";
        else
            quotaRemainingText.text = $"Day Complete";
    }

    public void ShowTooltip(string _tooltipText)
    {
        tooltipText.gameObject.SetActive(true);
        tooltipText.text = _tooltipText;
    }

    public void HideTooltip()
    {
        tooltipText.gameObject.SetActive(false);
    }


    public void updateWeapons(IUseableItem main, IUseableItem next, IUseableItem prev)
    {
        mainWeapon.sprite = main.sprite;
        nextWeapon.sprite = next.sprite;
        prevWeapon.sprite = prev.sprite;
        updateWeaponAmount(main);
    }
    public void updateWeaponAmount(IUseableItem main)
    {
        mainAmount.text = main.count + " x";
    }

    public void UpdateScrap()
    {
        txtScrap.text = "Scrap: " + PlayerInventory.instance.scrap;
    }
}