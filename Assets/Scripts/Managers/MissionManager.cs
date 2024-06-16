using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Data Refs")]
    [SerializeField] private MissionType[] missionTypes;
    [SerializeField] private Client[] clients;

    private Mission[] currentMissions;

    void Start()
    {
        currentMissions = new Mission[3];
        AddNewMissionIfAvailable();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMissionCountersInUI();
    }

    private void UpdateMissionCountersInUI()
    {
        for (int i = 0; i < currentMissions.Length; i++)
        {
            if (currentMissions[i] != null)
            {
                currentMissions[i].missionLength -= Time.deltaTime;

                if (currentMissions[i].missionLength < 0)
                {
                    // MISSION FAILED

                    UIManager.instance.UpdateClientUI(i, false, "", null, "", 0f);
                }

                UIManager.instance.UpdateClientUI(i, true, currentMissions[i].client.name, currentMissions[i].client.clientImage, currentMissions[i].missionName,
                    currentMissions[i].missionLength);
            }
        }
    }

    public void AddNewMissionIfAvailable()
    {
        for (int i = 0; i < currentMissions.Length; i++)
        {
            if (currentMissions[i] == null)
            {
                currentMissions[i] = GenerateNewMission();

                UIManager.instance.UpdateClientUI(i, true, currentMissions[i].client.clientName, currentMissions[i].client.clientImage, currentMissions[i].missionName,
                    currentMissions[i].missionLength);

                return;
            }
        }
    }

    public Mission GenerateNewMission ()
    {
        int randomMissionIndex = Random.Range(0, missionTypes.Length);
        int randomClientIndex = Random.Range(0, clients.Length);

        Mission newMission = new Mission(clients[randomClientIndex], 
            missionTypes[randomMissionIndex].missionName, 
            missionTypes[randomMissionIndex].missionDescription, 
            missionTypes[randomMissionIndex].missionLength);

        return newMission;
    }
}