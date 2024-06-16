using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Client:ScriptableObject
{
    public string clientName;
    public Texture clientImage;
    public int clientLocationIndex;
}
