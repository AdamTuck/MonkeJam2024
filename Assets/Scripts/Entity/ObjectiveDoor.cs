using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDoor : MonoBehaviour, ISelectable
{
    

    public void OnHoverEnter()
    {
        
    }

    public void OnHoverExit()
    {
        
    }

    public void OnSelect()
    {
        gameObject.SetActive(false);
    }
}
