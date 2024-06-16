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

    private int currentDay;

    [SerializeField] private LevelManager[] levels;

    private GameState currentState;
    private LevelManager currentLevel;
    private int currentLevelIndex = 0;

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
        if (levels.Length > 0)
        {
            //ChangeState(GameState.Briefing, levels[currentLevelIndex]);
            ChangeState(GameState.DayStart, levels[currentLevelIndex]);
        }

        playerHealth.OnDeath += GameOver;
    }

    private void Update()
    {
        DayNightRoutines();
    }

    public void ChangeState (GameState state, LevelManager level)
    {
        currentState = state;
        currentLevel = level;

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

        //DEBUG
        PlayerInventory.instance.shotgunShell.count = 10;

        currentLevel.StartLevel();
        cutsceneEnded?.Invoke();

        WeatherController.Instance.RollRandomWeather();

        WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = dayStartTime;
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = 0;

        currentDay++;
        UIManager.instance.UpdateCurrentDay(currentDay.ToString());
        UIManager.instance.StartDaySplashScreen();

        SoundManager.instance.SetAmbience("dayMusic");
    }

    private void DayRunning()
    {
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = dayTimeSpeed;
        PlayerInput.instance.UnlockInputs();

        Debug.Log("Level In: " + currentLevel.gameObject.name);
    }

    private void StormWallRace()
    {
        SoundManager.instance.SetAmbience("storm");
        StormWall.instance.ShrinkWall();
    }

    private void DayEnd ()
    {
        Debug.Log("Level End: " + currentLevel.gameObject.name);

        StormWall.instance.ResetWall();
        MissionManager.instance.ClearCurrentMissions();
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = 0;

        SoundManager.instance.SetAmbience("shop");
        ChangeState(GameState.NightUpgrade, currentLevel);
    }

    private void NightUpgrade()
    {
        ShopManager.instance.openShop();
    }

    public void EndNightUpgrade()
    {
        ChangeState(GameState.DayStart, levels[++currentLevelIndex]);
        RespawnAtCheckpoint();
    }

    public void EndCurrentDay()
    {
        ChangeState(GameState.DayEnd, currentLevel);
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

        player.transform.position = new Vector3(currentLevel.CheckpointPos().position.x, currentLevel.CheckpointPos().position.y, currentLevel.CheckpointPos().position.z);
        player.transform.rotation = Quaternion.Euler(new Vector3(currentLevel.CheckpointPos().eulerAngles.x, currentLevel.CheckpointPos().eulerAngles.y, currentLevel.CheckpointPos().eulerAngles.z));
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
        GameManager.instance.ChangeState(GameState.DayRunning, currentLevel);
    }

    private void DayNightRoutines()
    {
        if (currentState.ToString() == "DayRunning" && WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay >= dayEndTime)
            ChangeState(GameState.StormWall, currentLevel);
    }

    public string CurrentState()
    {
        return currentState.ToString();
    }
}