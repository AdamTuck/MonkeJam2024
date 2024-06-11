using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehaviour))]
public class PlayerJumpBehaviour : Interact
{
    [Header("Player Jump")]
    [SerializeField] private float jumpVelocity;

    private PlayerMovementBehaviour playerMovementBehaviour;

    public override void Interaction()
    {
        if (playerMovementBehaviour == null)
        {
            playerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
        }

        if (playerInput.jump && playerMovementBehaviour.isGrounded)
        {
            playerMovementBehaviour.SetYVelocity(jumpVelocity);
        }
    }
}
