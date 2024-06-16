using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class MissionType : ScriptableObject
{
    public string missionName;
    public string restaurantName;
    public float missionLength;
    public int destinationIndex;
}
