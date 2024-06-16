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
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private float tutorialTimeout;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private TMP_Text txtSpeed;

    [Header("MissionUIRefs")]
    [SerializeField] private GameObject[] missionObjs;
    [SerializeField] private TMP_Text[] clientNameTxt;
    [SerializeField] private TMP_Text[] clientOrderTxt;
    [SerializeField] private TMP_Text[] clientDeadlineTxt;

    [Header("Map Refs")]
    [SerializeField] private GameObject mapObject;

    [Header("Weapon Wheel")]
    [SerializeField] Image mainWeapon;
    [SerializeField] Image nextWeapon;
    [SerializeField] Image prevWeapon;

    [Header("Other Refs")]
    [SerializeField] GameObject splashScreenObj;
    [SerializeField] CanvasGroup splashScreenBGObj;
    [SerializeField] CanvasGroup daySpashScreenTxt;


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
            case "shootTutorial":
                tutorialText.text = "Click to shoot.\nPress 1 & 2 to switch weapons.";
                break;
            case "enemyTutorial":
                tutorialText.text = "Don't let the enemy see you.";
                break;
            case "grabTutorial":
                tutorialText.text = "Press E to interact, and pick up/drop items.";
                break;
        }

        tutorialText.gameObject.SetActive(true);
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
            tutorialText.gameObject.SetActive(false);
        }
    }

    void OnHealthUpdate (float health)
    {
        txtHealth.text = "Health: " + Mathf.Ceil(health).ToString();
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

    public void UpdateClientUI (int index, bool isActive, string _clientName, Texture _clientImage, string _order, float _deadline)
    {
        clientNameTxt[index].text = _clientName;
        clientOrderTxt[index].text = _order;
        clientDeadlineTxt[index].text = "" + TimeSpan.FromSeconds(_deadline).ToString("mm\\:ss");

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



    public void updateWeapons(Sprite main, Sprite next, Sprite prev)
    {
        mainWeapon.sprite = main;
        nextWeapon.sprite = next;
        prevWeapon.sprite = prev;
    }


}