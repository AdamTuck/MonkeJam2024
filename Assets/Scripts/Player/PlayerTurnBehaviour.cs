using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnBehaviour : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Player Rotation")]
    [SerializeField] private float turnSpeed;

    void Start()
    {
        playerInput = PlayerInput.instance;
    }

    void Update()
    {
        RotatePlayer();
    }

    void RotatePlayer ()
    {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * playerInput.horizontal);
    }
}