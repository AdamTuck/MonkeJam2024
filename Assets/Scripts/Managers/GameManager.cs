using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DigitalRuby.WeatherMaker;

public class GameManager : MonoBehaviour
{
    [Header("Game Properties")]
    [SerializeField] float dayStartTime;
    [SerializeField] float dayEndTime;
    [SerializeField] float dayTimeSpeed;

    [SerializeField] private Health playerHealth;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform checkpointPos;

    private int currentDay;

    private GameState currentState;
    private int currentLevelIndex = 0;

    private bool tutorialsEnabled;
    private bool[] tutorialShown;

    [Header("References")]
    [SerializeField] PlayerMovementBehaviour playerMovement;

    public UnityEvent cutsceneStarted, cutsceneEnded;

    public static GameManager instance;

    public enum GameState { DayStart, DayRunning, StormWall, DayEnd, NightUpgrade, GameOver, GameEnd }

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        PlayerInventory.instance.bomb.count = 0;
        PlayerInventory.instance.shotgunShell.count = 0;
        PlayerInventory.instance.jumpPad.count = 0;
        PlayerInventory.instance.rocketEngine.count = 0;
        
        ChangeState(GameState.DayStart);

        playerHealth.OnDeath += GameOver;
        tutorialShown = new bool[8];
        tutorialsEnabled = true;
    }

    private void Update()
    {
        DayNightRoutines();
    }

    public void ChangeState (GameState state)
    {
        currentState = state;

        switch (currentState)
        {
            case GameState.DayStart:
                DayStart();
                break;
            case GameState.DayRunning:
                DayRunning();
                break;
            case GameState.StormWall:
                StormWallRace();
                break;
            case GameState.DayEnd:
                DayEnd();
                break;
            case GameState.NightUpgrade:
                NightUpgrade();
                break;
            case GameState.GameOver:
                GameOver();
                break;
            case GameState.GameEnd:
                GameEnd();
                break;
            default:
                break;
        }
    }

    private void DayStart ()
    {
        Debug.Log("Day start");
        cutsceneEnded?.Invoke();

        WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = dayStartTime;
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = 0;

        currentDay++;
        UIManager.instance.UpdateCurrentDay(currentDay.ToString());
        UIManager.instance.StartDaySplashScreen();

        EnvironmentManager.Instance.RollRandomWeather();
        EnvironmentManager.Instance.TurnOffStreetlamps();

        MissionManager.instance.DisableHome();
        MissionManager.instance.AddNewMissionIfAvailable();

        EnemySpawner.instance.spawningEnabled = true;

        AudioManager.instance.SetAmbience("dayMusic");
        UIManager.instance.UpdateScrap();
    }

    private void DayRunning()
    {
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = dayTimeSpeed;
        PlayerInput.instance.UnlockInputs();
    }

    private void StormWallRace()
    {
        AudioManager.instance.SetAmbience("storm");
        StormWall.instance.ShrinkWall();
    }

    private void DayEnd ()
    {
        StormWall.instance.ResetWall();
        MissionManager.instance.ResetMissionsForDay();
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = 0;
        PlayerMovementBehaviour.instance.currentSpeed = 0;

        EnemySpawner.instance.spawningEnabled = false;
        EnemySpawner.instance.DespawnAllEnemies();

        AudioManager.instance.SetAmbience("shop");
        ChangeState(GameState.NightUpgrade);
    }

    private void NightUpgrade()
    {
        ShopManager.instance.openShop();
    }

    public void EndNightUpgrade()
    {
        ChangeState(GameState.DayStart);
        EnvironmentManager.Instance.RefreshScrap();
        RespawnAtCheckpoint();
    }

    public void EndCurrentDay()
    {
        ChangeState(GameState.DayEnd);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");

        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RespawnAtCheckpoint ()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerHealth.RespawnPlayer();
        UIManager.OnRespawnUI();
        playerMovement.ResetStats();
        PlayerMovementBehaviour.instance.currentSpeed = 0;

        player.transform.position = new Vector3(checkpointPos.position.x,
            checkpointPos.position.y,
            checkpointPos.position.z);

        Physics.SyncTransforms();

        player.transform.rotation = Quaternion.Euler(new Vector3(checkpointPos.eulerAngles.x, 
            checkpointPos.eulerAngles.y, 
            checkpointPos.eulerAngles.z));
    }

    private void GameEnd ()
    {
        Debug.Log("Game End - Winner");
    }

    public void CutsceneEnded ()
    {
        cutsceneEnded?.Invoke();
    }

    public void BeginCurrentDay()
    {
        GameManager.instance.ChangeState(GameState.DayRunning);
    }

    private void DayNightRoutines()
    {
        if (currentState.ToString() == "DayRunning")
        {
            if (WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay >= dayEndTime)
                ChangeState(GameState.StormWall);

            if (WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay >= 66600 && !EnvironmentManager.Instance.StreetLampsOn())
                EnvironmentManager.Instance.TurnOnStreetlamps();

            if (tutorialsEnabled)
            {
                if (currentDay == 1 && !tutorialShown[0])
                {
                    UIManager.instance.ShowTutorial("movementTutorial");
                    tutorialShown[0] = true;
                }

                if (!tutorialShown[1] && playerHealth.GetCurrentHealth() < 100)
                {
                    UIManager.instance.ShowTutorial("healthTutorial");
                    tutorialShown[1] = true;
                }

                if (!tutorialShown[2] && WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay >= 36000 && !PlayerMovementBehaviour.instance.IsOnRoad())
                {
                    UIManager.instance.ShowTutorial("roadTutorial");
                    tutorialShown[2] = true;
                }

                if (!tutorialShown[3] && PlayerMovementBehaviour.instance.CurrentStamina() < 90)
                {
                    UIManager.instance.ShowTutorial("staminaTutorial");
                    tutorialShown[3] = true;
                }

                if (!tutorialShown[4] && WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay >= 66600)
                {
                    UIManager.instance.ShowTutorial("flashlightTutorial");
                    tutorialShown[4] = true;
                }

                if (!tutorialShown[5] && MissionManager.instance.MissionsDoneForTheDay())
                {
                    UIManager.instance.ShowTutorial("dayEndTutorial");
                    tutorialShown[5] = true;
                }

                if (!tutorialShown[6] && currentDay == 2)
                {
                    UIManager.instance.ShowTutorial("statsTutorial");
                    tutorialShown[6] = true;
                }

                if (!tutorialShown[7] && currentDay >= 2
                    && WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay >= 36000
                    && MissionManager.instance.dangerousConditionsExtraScrap > 0)
                {
                    UIManager.instance.ShowTutorial("weatherTutorial");
                    tutorialShown[7] = true;
                }
            }
        }
    }

    public int CurrentDay()
    {
        return currentDay;
    }

    public string CurrentState()
    {
        return currentState.ToString();
    }
}