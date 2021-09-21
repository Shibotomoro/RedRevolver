using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Variable that holds a reference to the current state, initialize current state, change current state

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; } // { get; private set; } is shorthand for creating a getter and setter function

    //Enter into the passed parameter State
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    //Leave our current State, and enter the parameter give newState
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
