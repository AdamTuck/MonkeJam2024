using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDoor : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject waypointObj;

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
        EnableHome(false);
    }

    public void EnableHome (bool enable)
    {
        gameObject.SetActive(enable);
        waypointObj.SetActive(enable);
    }
}