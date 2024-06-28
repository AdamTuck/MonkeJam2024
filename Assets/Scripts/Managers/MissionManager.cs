using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MissionManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float newMissionInterval;
    [SerializeField] private int maxConcurrentMissions;
    [SerializeField] private int dailyQuota;
    [SerializeField] private int dailyQuotaIncreasePerDay;
    [SerializeField] private int missionCompleteScrapReward;
    [SerializeField] private int missionFailScrapPenalty;
    [HideInInspector] public int dangerousConditionsExtraScrap;
    [SerializeField] private float newMissionIntervalDecreasePerDay;

    [Header("Data Refs")]
    [SerializeField] private ObjectiveDoor[] destinations;
    [SerializeField] private HomeDoor homeDestination;
    [SerializeField] private MissionType[] missionTypes;
    [SerializeField] private Client[] clients;
    [SerializeField] private GameObject foodBasketObj;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> sounds;

    private bool missionsDoneForTheDay;
    private int missionsCompletedToday;
    private List<Mission> currentMissions = new List<Mission>();
    private float missionTimer;

    public static MissionManager instance;

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
        missionTimer = 0;
    }

    void Update()
    {
        if (GameManager.instance.CurrentState() == "DayRunning")
        {
            UpdateMissionCountersInUI();
            NewMissionTimer();
        }
    }

    private void UpdateMissionCountersInUI()
    {
        for (int i = 0; i < currentMissions.Count; i++)
        {
            currentMissions[i].missionLength -= Time.deltaTime;

            if (currentMissions[i].missionLength < 0)
                FailMission(i);
        }

        UIManager.instance.UpdateQuotaRemaining(Mathf.Clamp(dailyQuota - missionsCompletedToday, 0, dailyQuota));

        UpdateMissionUI();
    }

    private void UpdateMissionUI ()
    {
        bool holdingFood = false;

        for (int i = 0; i < maxConcurrentMissions; i++)
        {
            if (currentMissions.Count > i)
            {
                string statusTxt;

                if (!currentMissions[i].foodPickedUp)
                    statusTxt = "Pick Up from Restaurant";
                else
                {
                    holdingFood = true;
                    statusTxt = "Deliver to Customer";
                }

                UIManager.instance.UpdateClientUI(i, true, currentMissions[i].client.name,
                    currentMissions[i].client.clientImage, currentMissions[i].missionName + ", " + currentMissions[i].restaurantName,
                    currentMissions[i].missionLength, statusTxt);
            }
            else
                UIManager.instance.UpdateClientUI(i, false, "", null, "", 0f, "");
        }

        foodBasketObj.SetActive(holdingFood);
    }

    private void NewMissionTimer ()
    {
        missionTimer += Time.deltaTime;

        if (missionTimer > newMissionInterval)
        {
            missionTimer = 0;
            AddNewMissionIfAvailable();
        }
    }

    public void AddNewMissionIfAvailable()
    {
        if (currentMissions.Count < maxConcurrentMissions)
        {
            currentMissions.Add(GenerateNewMission());
            ShowAndHideDestinations();
            UpdateMissionUI();
        }
    }

    public void CompleteMissions ()
    {
        for (int i = 0; i < currentMissions.Count; i++)
        {
            if (currentMissions[i].foodDelivered)
            {
                PlayerInventory.instance.scrap += currentMissions[i].scrapReward;
                UIManager.instance.UpdateScrap();
                missionsCompletedToday++;

                currentMissions.RemoveAt(i);

                CompleteMissions();
            }
        }
    }

    public void FailMission (int missionIndex)
    {
        currentMissions.RemoveAt(missionIndex);

        if (PlayerInventory.instance.scrap > missionFailScrapPenalty)
            PlayerInventory.instance.scrap -= missionFailScrapPenalty;
        else
            PlayerInventory.instance.scrap = 0;

        UpdateMissionUI();
        ShowAndHideDestinations();
    }

    public void ResetMissionsForDay()
    {
        currentMissions.Clear();

        for (int i = 0; i < destinations.Length; i++)
        {
            destinations[i].EnableObjective(false);
        }
            
        missionsCompletedToday = 0;
        missionsDoneForTheDay = false;
        dailyQuota += dailyQuotaIncreasePerDay;

        UIManager.instance.UpdateQuotaRemaining(dailyQuota);

        if (newMissionInterval > newMissionIntervalDecreasePerDay)
            newMissionInterval -= newMissionIntervalDecreasePerDay;

        UpdateMissionUI();
    }

    public Mission GenerateNewMission ()
    {
        int randomMissionIndex = Random.Range(0, missionTypes.Length);
        int randomClientIndex = Random.Range(0, clients.Length);

        Mission newMission = new Mission(clients[randomClientIndex], 
            missionTypes[randomMissionIndex].missionName, 
            missionTypes[randomMissionIndex].restaurantName, 
            missionTypes[randomMissionIndex].missionLength,
            missionCompleteScrapReward+dangerousConditionsExtraScrap);

        return newMission;
    }

    public void PickUpFood (string _restaurantName)
    {
        for (int i = 0; i < currentMissions.Count; i++)
        {
            Debug.Log($"{currentMissions[i].restaurantName}, {_restaurantName}");
            if (currentMissions[i].restaurantName == _restaurantName)
            {
                currentMissions[i].foodPickedUp = true;
            }
        }
        audioSource.clip = sounds[0];
        audioSource.Play();

        ShowAndHideDestinations();
        UpdateMissionUI();
    }

    public void DeliverFood (string _clientName)
    {
        for (int i = 0; i < currentMissions.Count; i++)
        {
            Debug.Log($"{currentMissions[i].client.clientName}, {_clientName}");
            if (currentMissions[i].client.clientName == _clientName)
            {
                currentMissions[i].foodDelivered = true;
            }
        }
        audioSource.clip = sounds[1];
        audioSource.Play();

        CompleteMissions();
        UpdateMissionUI();
        ShowAndHideDestinations();

        UIManager.instance.UpdateQuotaRemaining(Mathf.Clamp(dailyQuota - missionsCompletedToday, 0, dailyQuota));

        if (missionsCompletedToday >= dailyQuota)
        {
            homeDestination.EnableHome(true);
            missionsDoneForTheDay = true;
        }
            
    }

    private void ShowAndHideDestinations()
    {
        bool destinationShowing;

        for (int i = 0; i < destinations.Length; i++)
        {
            destinationShowing = false;

            for (int j = 0; j < currentMissions.Count; j++)
            {
                if (!currentMissions[j].foodPickedUp &&
                    destinations[i].name == currentMissions[j].restaurantName)
                    destinationShowing = true;

                if (currentMissions[j].foodPickedUp &&
                    destinations[i].name == currentMissions[j].client.name)
                    destinationShowing = true;
            }

            destinations[i].EnableObjective(destinationShowing);
        }
    }

    public bool MissionsDoneForTheDay()
    {
        return missionsDoneForTheDay;
    }
    public void DisableHome()
    {
        homeDestination.EnableHome(false);
    }
}