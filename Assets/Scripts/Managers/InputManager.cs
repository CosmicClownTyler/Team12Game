﻿using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [HideInInspector]
    public PlayerInput PlayerInput;

    // Input values
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool ThrowWasPressed { get; private set; }
    public bool ThrowIsHeld { get; private set; }
    public bool ThrowWasReleased { get; private set; }
    public bool JumpWasPressed { get; private set; }
    public bool CameraChanged { get; private set; }
    public bool PausePressed { get; private set; }
    public bool ResumePressed { get; private set; }

    public bool ActiveGameInput { get; private set; }

    private void Awake()
    {
        // Manage singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        
    }

    public void PauseGameInput()
    {
        PlayerInput.SwitchCurrentActionMap("UI");
        ActiveGameInput = false;
    }
    public void ResumeGameInput()
    {
        PlayerInput.SwitchCurrentActionMap("Player");
        ActiveGameInput = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        // When the throw button is pressed at all
        if (context.phase == InputActionPhase.Started)
        {
            ThrowWasPressed = true;
            ThrowWasReleased = false;
        }

        // When the throw button is released
        if (context.phase == InputActionPhase.Canceled)
        {
            ThrowWasPressed = false;
            ThrowIsHeld = false;
            ThrowWasReleased = true;
        }

        // When the throw button is held down for at least the set time
        if (context.phase == InputActionPhase.Performed)
        {
            ThrowWasPressed = false;
            ThrowWasReleased = false;
            ThrowIsHeld = true;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        // When the jump button is released
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpWasPressed = false;
        }

        // When the jump button is held down for at least the set time
        if (context.phase == InputActionPhase.Performed)
        {
            JumpWasPressed = true;
        }
    }
    public void OnCameraChange(InputAction.CallbackContext context)
    {
        // When the camera change button is released
        if (context.phase == InputActionPhase.Canceled)
        {
            CameraChanged = false;
        }

        // When the jump button is held down for at least the set time
        if (context.phase == InputActionPhase.Performed)
        {
            CameraChanged = true;
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        // When the pause button is released
        if (context.phase == InputActionPhase.Canceled)
        {
            PausePressed = false;
        }

        // When the pause button is held down for at least the set time
        if (context.phase == InputActionPhase.Performed)
        {
            PausePressed = true;
        }
    }
    public void OnResume(InputAction.CallbackContext context)
    {
        // When the resume button is released
        if (context.phase == InputActionPhase.Canceled)
        {
            ResumePressed = false;
        }

        // When the resume button is held down for at least the set time
        if (context.phase == InputActionPhase.Performed)
        {
            ResumePressed = true;
        }
    }
}