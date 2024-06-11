using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Player Turn")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool invertMouse;

    private float cameraXRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput = PlayerInput.instance;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }

    void CameraRotation ()
    {
        cameraXRotation += playerInput.mouseY * Time.deltaTime * turnSpeed * (invertMouse ? 1 : -1);
        cameraXRotation = Mathf.Clamp(cameraXRotation, -40, 40);

        transform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);
    }
}
