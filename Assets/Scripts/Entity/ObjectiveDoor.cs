using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDoor : MonoBehaviour, ISelectable
{
    [SerializeField] private bool isRestaurant;
    [SerializeField] private GameObject waypointObj;

    public void OnHoverEnter()
    {
        if (isRestaurant)
            UIManager.instance.ShowTooltip("Press E to pick up food");
        else
            UIManager.instance.ShowTooltip("Press E to complete delivery");
    }

    public void OnHoverExit()
    {
        UIManager.instance.HideTooltip();
    }

    public void ShowWaypoint(bool showWaypoint)
    {
        waypointObj.SetActive(showWaypoint);
    }

    public void OnSelect()
    {
        if (isRestaurant)
            MissionManager.instance.PickUpFood(gameObject.name);
        else
            MissionManager.instance.DeliverFood(gameObject.name);

        EnableObjective(false);
    }

    public void EnableObjective (bool enableObjective)
    {
        gameObject.SetActive(enableObjective);
        ShowWaypoint(enableObjective);
    }

    public void DisableObjective()
    {
        waypointObj.SetActive(false);
        gameObject.SetActive(false);
    }
}
