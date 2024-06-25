using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof (CharacterController))]
public class PlayerMovementBehaviour : MonoBehaviour
{
    PlayerInput playerInput;
    public static PlayerMovementBehaviour instance;

    [Header("Player Movement")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float maxReverseSpeed;
    [SerializeField] private float topSpeedSprinting;
    [SerializeField] public float accelerationRate;
    [SerializeField] public float decelerationRate;
    [SerializeField] private float momentumDegradeRate;
    [SerializeField] private float offRoadDegradeMultiplier;
    [SerializeField] private float gravity = -9.81f;

    [Header("Player Stamina")]
    [SerializeField] private float sprintMultiplier;
    [SerializeField] public float topEndurance;
    [SerializeField] private float staminaDegradeRateResting;
    [SerializeField] private float staminaDegradeMultiplier;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckDistance;

    private CharacterController characterController;

    private Vector3 playerVelocity;
    private float currentStamina;
    public bool isGrounded { get; private set; }
    public float currentSpeed;
    public bool rocketEngine;
    private float moveMultiplier = 1;

    private bool currentlyAccelerating;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = PlayerInput.instance;
        currentStamina = topEndurance;
    }

    void Update()
    {
        GroundedCheck();
        MovePlayer();
        EnduranceDegrade();
        SetBikeAudio();
    }

    private void SetBikeAudio()
    {
        if (currentSpeed > 0)
        {
            if (currentlyAccelerating)
                AudioManager.instance.SetBikeAudio("bikeAccelerate");
            else
                AudioManager.instance.SetBikeAudio("bikeCoast");
        }
        else
        {
            AudioManager.instance.SetBikeAudio("none");
        }
    }

    void EnduranceDegrade()
    {
        // if currently cycling
        currentStamina -= staminaDegradeRateResting * staminaDegradeMultiplier;

        UIManager.instance.SetStaminaBar(currentStamina / 100);
    }

    public float CurrentStamina ()
    {
        return currentStamina;
    }

    void MovePlayer()
    {
        float offRoadPenalty = (IsOnRoad()) ? 1f : offRoadDegradeMultiplier;

        if (playerInput.vertical > 0 && currentStamina > 0)
        {
            currentlyAccelerating = true;

            if (playerInput.sprint)
            {
                staminaDegradeMultiplier = (IsOnRoad())? 2:4;
                currentSpeed = Mathf.Clamp(currentSpeed + accelerationRate*2/offRoadPenalty, 0, topSpeedSprinting);
            }
            else
            {
                staminaDegradeMultiplier = (IsOnRoad()) ? 1:2;

                if (currentSpeed > topSpeed)
                    currentSpeed = Mathf.Clamp(currentSpeed - momentumDegradeRate*offRoadPenalty, 0, topSpeedSprinting);
                else
                    currentSpeed = Mathf.Clamp(currentSpeed + accelerationRate/offRoadPenalty, 0, topSpeed);
            }
        }
        else if (playerInput.vertical < 0 && currentStamina > 0)
        {
            currentlyAccelerating = false;

            currentSpeed = Mathf.Clamp(currentSpeed - decelerationRate * 2, maxReverseSpeed, topSpeedSprinting);
            staminaDegradeMultiplier = (IsOnRoad()) ? 1 : 2;
        }
        else
        {
            currentlyAccelerating = false;

            if (currentSpeed > 0)
                currentSpeed = Mathf.Clamp(currentSpeed - momentumDegradeRate*(offRoadPenalty*4), 0, topSpeedSprinting);
            
            staminaDegradeMultiplier = -0.5f;
        }
        
        //moveMultiplier = playerInput.sprint ? sprintMultiplier : 1;

        //characterController.Move((transform.forward * playerInput.vertical + transform.right * playerInput.horizontal) * moveSpeed * Time.deltaTime * moveMultiplier);
        if (rocketEngine)
        { 
            //When rocketboosting you constantly move at topSpeed but spend no stamina
            currentSpeed = topSpeedSprinting + 8;
            staminaDegradeMultiplier = 0;
        }

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

    public bool IsOnRoad()
    {
        bool isOnRoad = false;
        Collider[] nearbyGround = Physics.OverlapSphere(groundCheckTransform.position, groundCheckDistance);

        for (int i = 0; i < nearbyGround.Length; i++)
        {
            if (nearbyGround[i].CompareTag("Road"))
            {
                isOnRoad = true;
            }
        }

        return isOnRoad;
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
        currentStamina = topEndurance;
    }
}