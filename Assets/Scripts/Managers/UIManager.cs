using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject cutsceneScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private float tutorialTimeout;

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
}