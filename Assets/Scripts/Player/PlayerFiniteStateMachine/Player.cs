using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

//Player Class will handle different player state objects
//We assign this script to our player in Unity

public class Player : MonoBehaviour
{
    #region State Vars

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    [SerializeField] private PlayerData playerData;

    #endregion

    #region Player Components Vars

    public Animator Anim { get; private set; }      //Reference the animator currently on our player Character
    public Rigidbody2D RB { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }

    #endregion

    #region Transforms Check Vars

    #endregion

    #region Other Vars

    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 workspace;

    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();    //Create a State Machine on game load

        // Load these States When we start game
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
    }

    //Called before Update
    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        FacingDirection = 1;

        StateMachine.Initialize(IdleState); //Set Player to idle in beginning
    }

    //Unity function that loads every frame
    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    //Unity function that loads every fixed frame
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Functions

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    #endregion

    #region Check Functions

    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    #endregion

    #region Other Functions

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    #endregion
}
