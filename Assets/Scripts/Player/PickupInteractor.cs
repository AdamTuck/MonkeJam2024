using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractor : Interact
{
    [Header("Pick And Drop")]
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupDistance;
    [SerializeField] private Transform attachTransform;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;

    private RaycastHit raycastHit;

    private bool isHoldingObject = false;
    private IPickable pickable;

    public override void Interaction()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out raycastHit, pickupDistance, pickupLayer))
        {
            if (playerInput.interact && !isHoldingObject)
            {
                pickable = raycastHit.transform.GetComponent<IPickable>();

                if (pickable == null)
                    return;

                pickable.OnPicked(attachTransform);
                isHoldingObject = true;
                return;
            }
        }

        if (playerInput.interact && isHoldingObject && pickable != null)
        {
            pickable.OnDropped();
            isHoldingObject = false;
        }
    }
}