using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState is the Base Class and will be inherited by ALL OTHER STATES

public class PlayerState
{

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isExitingState;

    // Each State is going to have a string when we create the state
    // This string will tell the Unity Animator what animation to play
    private string animBoolName;

    protected float startTime; //Variable that holds Enter time of State.  Ex: We enter move state this will tell us when we did that

    // Constructor for any created state
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    //Called when we Enter a state
    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        player.Anim.SetBool(animBoolName, true);
        isExitingState = false;
    }

    //Called When we Leave a State
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    //Called Every Frame
    public virtual void LogicUpdate()
    {

    }

    //Called Every FixedUpdate
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    //Called when we Enter and FixedUpdate.  Ex when we need to check a wall we want to do that immediately and 
    public virtual void DoChecks()
    {

    }
}
