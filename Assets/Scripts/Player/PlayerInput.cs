using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }
    public bool sprint { get; private set; }
    public bool jump { get; private set; }
    public bool interact { get; private set; }
    public bool leftBtn { get; private set; }
    public bool rightBtn { get; private set; }
    public bool escape { get; private set; }
    public bool weapon1Pressed { get; private set; }
    public bool weapon2Pressed { get; private set; }
    public bool commandPressed { get; private set; }

    private bool clear;

    private bool inputsAllowed;

    // Singleton
    public static PlayerInput instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    void Update()
    {
        ClearInputs();
        SetInputs();
    }

    private void FixedUpdate()
    {
        clear = true;
    }

    void SetInputs ()
    {
        if (inputsAllowed)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            sprint = sprint || Input.GetButton("Sprint");
            jump = jump || Input.GetButtonDown("Jump");
            interact = interact || Input.GetKeyDown(KeyCode.E);

            leftBtn = leftBtn || Input.GetButtonDown("Fire1");
            rightBtn = rightBtn || Input.GetButtonDown("Fire2");

            escape = escape || Input.GetKeyDown(KeyCode.Escape);

            weapon1Pressed = weapon1Pressed || Input.GetKeyDown(KeyCode.Alpha1);
            weapon2Pressed = weapon2Pressed || Input.GetKeyDown(KeyCode.Alpha2);

            commandPressed = commandPressed || Input.GetKeyDown(KeyCode.F);
        }
    }

    void ClearInputs ()
    {
        if (!clear)
            return;

        horizontal = 0;
        vertical = 0;

        mouseX = 0;
        mouseY = 0;

        sprint = false;
        jump = false;
        interact = false;

        leftBtn = false;
        rightBtn = false;

        escape = false;

        weapon1Pressed = false;
        weapon2Pressed = false;

        commandPressed = false;
    }

    public void CutsceneStarted ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputsAllowed = false;
    }

    public void CutsceneEnded ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputsAllowed = true;
    }
}