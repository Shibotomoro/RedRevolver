using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    private float RawMovementInputDirectionX;
    private float RawMovementInputDirectionY;
    private float MovementInputDirectionX;
    private float MovementInputDirectionY;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastImageYpos;
    private float lastDash = -100f;
    private float coyoteCounter;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;
    private int amountOfDashLeft;
    public int amountOfBullets = 6;

    private bool isFacingRight = true;
    private bool isMoving;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isAttemptingToJump;
    private bool isDashing;
    private bool canNormalJump;
    private bool canWallJump;
    private bool canMove;
    private bool canFlip;
    private bool canDash;
    private bool checkJumpMultiplier;
    private bool hasWallJumped;
    private bool waitForDash;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject bulletBehindPrefab;
    public GameObject bulletDownPrefab;
    public GameObject bulletUpPrefab;
    public GameObject bulletUpRightPrefab;
    public GameObject bulletDownRightPrefab;
    public GameObject bulletUpLeftPrefab;
    public GameObject bulletDownLeftPrefab;

    [SerializeField] private DialogueUI dialogueUI;

    private Rigidbody2D RB;
    private Animator Anim;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;

    public int amountOfJumps = 1;
    public int amountOfDash = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 2.0f;
    public float wallSlideSpeed = 5.0f;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallJumpForce = 20.0f;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float coyoteTime = 0.2f;
    public float diagonalDashMultiplier = 0.7f;

    public float dashTime = 0.3f;
    public float dashSpeed = 20.0f;
    public float dashSpeedY = 20.0f;
    public float distanceBetweenImages = 0.1f;
    public float dashCooldown = 2.5f;

    public Vector2 wallJumpDirection;

    #endregion

    #region Unity Callback Functions

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.IsOpen)
        {
            return;
        }
        amountOfDashLeft = amountOfBullets;
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckIfCanDash();
        CheckDash();
        CheckDialogue();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    #endregion

    #region Check Functions

    private void CheckInput()
    {
        if (!waitForDash)
        {
            RawMovementInputDirectionX = Input.GetAxisRaw("Horizontal");
            RawMovementInputDirectionY = Input.GetAxisRaw("Vertical");
            MovementInputDirectionX = Input.GetAxis("Horizontal");
            MovementInputDirectionY = Input.GetAxis("Vertical");
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (!waitForDash)
        {
            if (Input.GetButtonDown("Horizontal") && isTouchingWall)
            {
                if (!isGrounded && RawMovementInputDirectionX != facingDirection)
                {
                    canMove = false;
                    canFlip = false;

                    turnTimer = turnTimerSet;
                }
            }
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButtonDown("Dash"))
        {

            if (!isGrounded && amountOfBullets > 0)
            {
                AttemptToDash();
                amountOfBullets -= 1;
            }
            else if (Time.time >= (lastDash + dashCooldown) && amountOfBullets > 0)
            {
                AttemptToDash();
                amountOfBullets -= 1;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if(amountOfBullets > 0)
            {
                Shoot();
                amountOfBullets -= 1;
            }          
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && RawMovementInputDirectionX < 0)
        {
            Flip();
        }
        else if (!isFacingRight && RawMovementInputDirectionX > 0)
        {
            Flip();
        }

        if (Mathf.Abs(RB.velocity.x) >= 0.01f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (isGrounded && RB.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && RawMovementInputDirectionX == facingDirection && RB.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!isGrounded && isTouchingWall && RawMovementInputDirectionX != 0 &&
                RawMovementInputDirectionX != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded || coyoteCounter > 0f)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && RawMovementInputDirectionX == -lastWallJumpDirection)
            {
                RB.velocity = new Vector2(RB.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void CheckDash()
    {
        if (isGrounded)
        {
            amountOfDashLeft = amountOfDash;
        }

        if (isDashing)
        {
            Vector2 directionalInput = new Vector2(MovementInputDirectionX, MovementInputDirectionY);
            waitForDash = true;
            CameraShake.Instance.ShakeCamera(2f, .2f);
            if (!isGrounded)
            {
                amountOfDashLeft--;
            }

            if (dashTimeLeft > 0)
            {
                isTouchingWall = false;
                canMove = false;
                canFlip = false;
                if (RawMovementInputDirectionX == 0 && RawMovementInputDirectionY == 0)
                {
                    RB.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);
                }
                else if (Mathf.Abs(RawMovementInputDirectionX) == 1 && Mathf.Abs(RawMovementInputDirectionY) == 1)
                {
                    RB.velocity = new Vector2(dashSpeed * RawMovementInputDirectionX * diagonalDashMultiplier, dashSpeedY * RawMovementInputDirectionY * diagonalDashMultiplier);
                }
                else
                {
                    RB.velocity = new Vector2(dashSpeed * RawMovementInputDirectionX, dashSpeedY * RawMovementInputDirectionY);
                }

                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
                if (Mathf.Abs(transform.position.y - lastImageYpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageYpos = transform.position.y;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
                waitForDash = false;
            }
        }
    }

    private void CheckIfCanDash()
    {
        if (isGrounded && !isTouchingWall)
        {
            amountOfDashLeft = amountOfDash;
        }

        if (amountOfDashLeft <= 0)
        {
            canDash = false;
        }
        else
        {
            canDash = true;
        }
    }

    private void CheckDialogue()
    {
        if (Input.GetKeyDown(KeyCode.E) && isGrounded && !isMoving)
        {
            Interactable?.Interact(this);
        }
    }

    #endregion

    #region Other Functions

    private void UpdateAnimations()
    {
        Anim.SetBool("isMoving", isMoving);
        Anim.SetBool("isGrounded", isGrounded);
        Anim.SetFloat("yVelocity", RB.velocity.y);
        Anim.SetBool("isWallSliding", isWallSliding);
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && RawMovementInputDirectionX == 0)
        {
            RB.velocity = new Vector2(RB.velocity.x * airDragMultiplier, RB.velocity.y);
        }
        else if (canMove)
        {
            RB.velocity = new Vector2(movementSpeed * RawMovementInputDirectionX, RB.velocity.y);
        }

        if (isWallSliding)
        {
            if (RB.velocity.y < -wallSlideSpeed)
            {
                RB.velocity = new Vector2(RB.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            RB.velocity = new Vector2(RB.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * RawMovementInputDirectionX, wallJumpForce * wallJumpDirection.y);
            RB.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }

    private void AttemptToDash()
    {
        if (canDash && amountOfBullets > 0)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;
            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
            lastImageYpos = transform.position.y;
            amountOfDashLeft--;
            if (RawMovementInputDirectionX == 0 && RawMovementInputDirectionY == 0)
            {
                ShootBehind();
            }
            else if (RawMovementInputDirectionX == 1 && RawMovementInputDirectionY == 0)
            {
                ShootBehind();
            }
            else if (RawMovementInputDirectionX == -1 && RawMovementInputDirectionY == 0)
            {
                ShootBehind();
            }
            else if (RawMovementInputDirectionX == 0 && RawMovementInputDirectionY == 1)
            {
                ShootDown();
            }
            else if (RawMovementInputDirectionX == 0 && RawMovementInputDirectionY == -1)
            {
                ShootUp();
            }
            else if (RawMovementInputDirectionX == 1 && RawMovementInputDirectionY == 1)
            {
                ShootDownLeft();
            }
            else if (RawMovementInputDirectionX == 1 && RawMovementInputDirectionY == -1)
            {
                ShootUpLeft();
            }
            else if (RawMovementInputDirectionX == -1 && RawMovementInputDirectionY == 1)
            {
                ShootDownRight();
            }
            else if (RawMovementInputDirectionX == -1 && RawMovementInputDirectionY == -1)
            {
                ShootUpRight();
            }
        }
    }

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    #endregion

    #region Shoot Directions

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootBehind()
    {
        Instantiate(bulletBehindPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootDown()
    {
        Instantiate(bulletDownPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootUp()
    {
        Instantiate(bulletUpPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootUpRight()
    {
        Instantiate(bulletUpRightPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootDownRight()
    {
        Instantiate(bulletDownRightPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootDownLeft()
    {
        Instantiate(bulletDownLeftPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootUpLeft()
    {
        Instantiate(bulletUpLeftPrefab, firePoint.position, firePoint.rotation);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
