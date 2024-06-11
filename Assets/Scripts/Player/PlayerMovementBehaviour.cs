using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class PlayerMovementBehaviour : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sprintMultiplier;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckDistance;

    private CharacterController characterController;

    private Vector3 playerVelocity;
    public bool isGrounded { get; private set; }
    private float moveMultiplier = 1;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = PlayerInput.instance;
    }

    void Update()
    {
        GroundedCheck();
        MovePlayer();
    }

    void MovePlayer()
    {
        moveMultiplier = playerInput.sprint ? sprintMultiplier : 1;

        characterController.Move((transform.forward * playerInput.vertical + transform.right * playerInput.horizontal) * moveSpeed * Time.deltaTime * moveMultiplier);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2.0f;
        }

        playerVelocity.y += gravity * Time.deltaTime;

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void GroundedCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckDistance, groundLayerMask);
    }

    public void SetYVelocity (float value)
    {
        playerVelocity.y = value;
    }

    public float GetForwardSpeed ()
    {
        return playerInput.vertical * moveSpeed * moveMultiplier;
    }
}