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
    public bool flashlight { get; private set; }
    public bool item { get; private set; }
    public bool itemScroll { get; private set; }
    public bool leftBtn { get; private set; }
    public bool rightBtn { get; private set; }
    public bool tab { get; private set; }
    public bool escape { get; private set; }
    public bool itemWheelDown { get; private set; }
    public bool itemWheelUp { get; private set; }
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
            flashlight = flashlight || Input.GetKeyDown(KeyCode.F);
            item = item || Input.GetKeyDown(KeyCode.R);
            itemScroll = itemScroll || Input.GetKeyDown(KeyCode.Q);

            leftBtn = leftBtn || Input.GetButtonDown("Fire1");
            rightBtn = rightBtn || Input.GetButtonDown("Fire2");

            tab = tab || Input.GetKeyDown(KeyCode.Tab);

            escape = escape || Input.GetKeyDown(KeyCode.Escape);

            itemWheelDown = itemWheelDown || Input.mouseScrollDelta.y > 0;
            itemWheelUp = itemWheelUp || Input.mouseScrollDelta.y < 0;

            commandPressed = commandPressed || Input.GetKeyDown(KeyCode.F);
        }

        escape = escape || Input.GetKeyDown(KeyCode.Escape);
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
        flashlight = false;
        item = false;
        itemScroll = false;

        leftBtn = false;
        rightBtn = false;

        tab = false;
        escape = false;

        itemWheelDown = false;
        itemWheelUp = false;

        commandPressed = false;
    }

    public void LockInputs ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputsAllowed = false;
    }

    public void UnlockInputs ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputsAllowed = true;
    }
}