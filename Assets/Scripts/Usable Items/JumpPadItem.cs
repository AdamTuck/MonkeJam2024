using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadItem : IUseableItem
{
    public JumpPadItem(GameObject target)
    {

    }

    public override void Use()
    {
        count--;
        Debug.Log("Used Jump Pad");
    }
}
