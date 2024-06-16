using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float newMissionInterval;
    [SerializeField] private int maxConcurrentMissions;

    [Header("Data Refs")]
    [SerializeField] private GameObject[] destinations;
    [SerializeField] private MissionType[] missionTypes;
    [SerializeField] private Client[] clients;

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
        AddNewMissionIfAvailable();
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

        UpdateMissionUI();
    }

    private void UpdateMissionUI ()
    {
        for (int i = 0; i < maxConcurrentMissions; i++)
        {
            if (currentMissions.Count > i)
            {
                string statusTxt;

                if (!currentMissions[i].foodPickedUp)
                    statusTxt = "Pick Up from Restaurant";
                else
                    statusTxt = "Deliver to Customer";

                UIManager.instance.UpdateClientUI(i, true, currentMissions[i].client.name,
                    currentMissions[i].client.clientImage, currentMissions[i].missionName + ", " + currentMissions[i].restaurantName,
                    currentMissions[i].missionLength, statusTxt);
            }
            else
                UIManager.instance.UpdateClientUI(i, false, "", null, "", 0f, "");
        }
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
            UpdateMissionUI();
        }
    }

    public void CompleteMission (int missionIndex)
    {
        PlayerInventory.instance.scrap += currentMissions[missionIndex].scrapReward;

        currentMissions.RemoveAt(missionIndex);
        UpdateMissionUI();
    }

    public void FailMission (int missionIndex)
    {
        currentMissions.RemoveAt(missionIndex);
        UpdateMissionUI();
    }

    public void ClearCurrentMissions()
    {
        currentMissions.Clear();
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
            10);

        destinations[missionTypes[randomMissionIndex].destinationIndex].SetActive(true);
        destinations[missionTypes[randomMissionIndex].destinationIndex].GetComponent<ObjectiveDoor>().ShowWaypoint();

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
                destinations[currentMissions[i].client.clientLocationIndex].SetActive(true);
                destinations[currentMissions[i].client.clientLocationIndex].GetComponent<ObjectiveDoor>().ShowWaypoint();
            }
        }

        UpdateMissionUI();
    }

    public void DeliverFood (string _clientName)
    {
        for (int i = 0; i < currentMissions.Count; i++)
        {
            Debug.Log($"{currentMissions[i].client.clientName}, {_clientName}");
            if (currentMissions[i].client.clientName == _clientName)
            {
                CompleteMission(i);
            }
        }

        UpdateMissionUI();
    }
}