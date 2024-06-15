using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jump Pad", menuName = "ScriptableObjects / Useable Items / Jump Pad", order = 3)]
public class JumpPadItem : IUseableItem
{

    public override void Use()
    {
        //setCount(-1);
        count--;
        Debug.Log("Used Jump Pad");
    }
}
