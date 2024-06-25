using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Player Turn")]
    [SerializeField] private float turnSpeed;
    public float sensitivityMultiplier;
    public bool invertMouse;
    [SerializeField] private float maxCamXRotation;
    [SerializeField] private float maxCamYRotation;

    private float cameraXRotation, cameraYRotation;

    public static CameraMovementBehaviour instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput = PlayerInput.instance;

        sensitivityMultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }

    void CameraRotation ()
    {
        cameraXRotation += playerInput.mouseY * sensitivityMultiplier * Time.deltaTime * turnSpeed * (invertMouse ? 1 : -1);
        cameraYRotation += playerInput.mouseX * sensitivityMultiplier * Time.deltaTime * turnSpeed;

        cameraXRotation = Mathf.Clamp(cameraXRotation, maxCamXRotation*-1, maxCamXRotation);
        cameraYRotation = Mathf.Clamp(cameraYRotation, maxCamYRotation*-1, maxCamYRotation);

        transform.localRotation = Quaternion.Euler(cameraXRotation, cameraYRotation, 0);
    }
}
