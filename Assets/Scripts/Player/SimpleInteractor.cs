using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractor : Interact
{
    [Header("Player Interactions")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactionLayer;

    // Raycasting
    private RaycastHit raycastHit;
    private ISelectable selectable;

    public override void Interaction()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out raycastHit, interactionDistance, interactionLayer))
        {
            selectable = raycastHit.transform.GetComponent<ISelectable>();

            if (selectable != null)
            {
                selectable.OnHoverEnter();

                if (playerInput.interact)
                {
                    selectable.OnSelect();
                }
            }
        }

        if (raycastHit.transform == null && selectable != null)
        {
            selectable.OnHoverExit();
            selectable = null;
        }
    }
}