using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Script to handle Input Events using Unity Input System

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }

    //Normalize movementInput in respect to 1
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    // OnInput Functions:  Paramater is a InputAction.CallbackContext which is essentially
    // just an action that got triggered when user inputs a value into keyboard/gamepad
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //Pass in our triggerred input into a private vector2 variable called movementInput
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {

    }
}
