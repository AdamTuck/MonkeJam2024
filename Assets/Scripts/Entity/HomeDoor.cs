using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDoor : MonoBehaviour, ISelectable
{
    public void OnHoverEnter()
    {
        UIManager.instance.ShowTooltip("Press E to end current day");
    }

    public void OnHoverExit()
    {
        UIManager.instance.HideTooltip();
    }

    public void OnSelect()
    {
        GameManager.instance.EndCurrentDay();
    }
}