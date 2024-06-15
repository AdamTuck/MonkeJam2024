using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class PlayerMovementBehaviour : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Player Movement")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float topSpeedSprinting;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float momentumDegradeRate;
    [SerializeField] private float gravity = -9.81f;

    [Header("Player Stamina")]
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float topEndurance;
    [SerializeField] private float enduranceDegradeRateResting;
    [SerializeField] private float enduranceDegradeMultiplier;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckDistance;

    private CharacterController characterController;

    private Vector3 playerVelocity;
    private float currentEndurance;
    public bool isGrounded { get; private set; }
    private float currentSpeed;
    private float moveMultiplier = 1;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = PlayerInput.instance;
        currentEndurance = topEndurance;
    }

    void Update()
    {
        GroundedCheck();
        MovePlayer();
        EnduranceDegrade();
    }
    
    void EnduranceDegrade()
    {
        // if currently cycling
        currentEndurance -= enduranceDegradeRateResting * enduranceDegradeMultiplier;

        UIManager.instance.SetStaminaBar(currentEndurance / 100);
    }

    void MovePlayer()
    {
        if (playerInput.vertical > 0 && currentEndurance > 0)
        {
            if (playerInput.sprint)
            {
                enduranceDegradeMultiplier = 2;
                currentSpeed = Mathf.Clamp(currentSpeed + accelerationRate*2, 0, topSpeedSprinting);
            }
            else
            {
                enduranceDegradeMultiplier = 1;

                if (currentSpeed > topSpeed)
                    currentSpeed = Mathf.Clamp(currentSpeed - momentumDegradeRate, 0, topSpeedSprinting);
                else
                    currentSpeed = Mathf.Clamp(currentSpeed + accelerationRate, 0, topSpeed);
            }

            //currentSpeed = Mathf.Clamp(currentSpeed + accelerationRate, 0, topSpeed);
        }
        else if (playerInput.vertical < 0 && currentEndurance > 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed - accelerationRate*2, 0, topSpeedSprinting);
            enduranceDegradeMultiplier = 1;
        }
        else
        {
            currentSpeed = Mathf.Clamp(currentSpeed - momentumDegradeRate, 0, topSpeed * 1.5f);
            enduranceDegradeMultiplier = 0;
        }
        
        //moveMultiplier = playerInput.sprint ? sprintMultiplier : 1;

        //characterController.Move((transform.forward * playerInput.vertical + transform.right * playerInput.horizontal) * moveSpeed * Time.deltaTime * moveMultiplier);
        characterController.Move(transform.forward * currentSpeed * Time.deltaTime);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2.0f;
        }

        playerVelocity.y += gravity * Time.deltaTime;

        characterController.Move(playerVelocity * Time.deltaTime);

        UIManager.instance.SetSpeedNum(currentSpeed);
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
        return playerInput.vertical * topSpeed * moveMultiplier;
    }

    public void ResetStats()
    {
        currentEndurance = topEndurance;
    }
}