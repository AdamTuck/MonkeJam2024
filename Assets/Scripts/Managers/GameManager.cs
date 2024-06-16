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

    public UnityEvent cutsceneStarted, cutsceneEnded;

    public static GameManager instance;

    public enum GameState { DayStart, DayRunning, DayEnd, NightUpgrade, GameOver, GameEnd }

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
        currentLevel.StartLevel();
        cutsceneEnded?.Invoke();

        WeatherController.Instance.RollRandomWeather();

        WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = dayStartTime;
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = 0;

        UIManager.instance.StartDaySplashScreen();
    }

    private void DayRunning()
    {
        WeatherMakerDayNightCycleManagerScript.Instance.Speed = dayTimeSpeed;
        PlayerInput.instance.UnlockInputs();

        Debug.Log("Level In: " + currentLevel.gameObject.name);
    }

    private void DayEnd ()
    {
        Debug.Log("Level End: " + currentLevel.gameObject.name);

        WeatherMakerDayNightCycleManagerScript.Instance.Speed = 0;

        ChangeState(GameState.NightUpgrade, levels[++currentLevelIndex]);
    }

    private void NightUpgrade()
    {

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
}