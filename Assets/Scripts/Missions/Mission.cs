using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
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
}