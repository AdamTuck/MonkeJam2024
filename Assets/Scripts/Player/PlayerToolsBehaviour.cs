using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolsBehaviour : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] GameObject bikeLight;

    void Start()
    {
        playerInput = PlayerInput.instance;
    }

    void Update()
    {
        if (playerInput.tab)
        {
            UIManager.instance.ToggleMapScreen();
        }

        if (playerInput.flashlight)
        {
            bikeLight.SetActive(!bikeLight.activeInHierarchy);
        }

        if (playerInput.escape)
        {
            UIManager.instance.TogglePause();
        }
    }
}
