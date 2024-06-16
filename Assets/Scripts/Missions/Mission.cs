using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
<<<<<<< HEAD
    public bool foodPickedUp;

    public Client client;
    public string missionName;
    public string restaurantName;
    public float missionLength;
    public int scrapReward;

    public Mission (Client _client, string _missionType, string _restaurantName, float _missionLength, int _scrapReward)
    {
        client = _client;
        missionName = _missionType;
        restaurantName = _restaurantName;
        missionLength = _missionLength;
        scrapReward = _scrapReward;
    }
=======
    public Client client;
    public string missionName;
    public string missionDescription;
    public float missionLength;

    public Mission (Client _client, string _missionType, string _missionDescription, float _missionLength)
    {
        client = _client;
        missionName = _missionType;
        missionDescription = _missionDescription;
        missionLength = _missionLength;
    }

    // Client, StartPoint, Destination
>>>>>>> BurryBurstBranch
}