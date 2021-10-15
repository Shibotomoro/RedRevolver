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
    public float dashTimeLeft;
    private float lastImageXpos;
    private float lastImageYpos;
    private float lastDash = -100f;
    private float coyoteCounter;
    private float halfGravMultiplier;
    public float halfGravThreshold;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;
    private int amountOfDashLeft;
    public  int amountOfBullets = 6;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isTouchingWall;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isJumping;
    private bool isFacingRight = true;
    private bool isMoving;
    private bool isWallSliding;
    private bool isAttemptingToJump;
    private bool isCrouching;
    private bool isTouchingCorner;
    private bool canNormalJump;
    private bool canWallJump;
    private bool canMove;
    private bool canFlip;
    private bool canDash;
    private bool checkJumpMultiplier;
    private bool hasWallJumped;
    private bool waitForDash;
    private bool playDashParticle;
    private bool cornerDetected;
    private bool fixCornerDash;

    //Wavedash variables
    private bool isInAir;
    private bool isWavedashDistanceToGround;
    private bool isAutoJumpDistanceToGround;
    private bool recentlyJumped;
    private bool diagonalDashDownDetected;
    private bool wavedashInitiated;
    private bool wavedashJumpBufferStore;
    private bool turnOffMovingWhileWavedash;
    private bool autoHopBasedOffDashTime;
    private float recentJumpTimer = 2.0f;
    private float recentJumpTimerSet = 2.0f;
    private float diagonalDashDownDetectedTimer = 2.0f;
    private float diagonalDashDownDetectedTimeSet = 2.0f;
    private float wavedashGroundDistanceCheck = 1f;
    private float autoJumpGroundDistanceCheck = .1f;
    private float waitForWavedashTimer = .1f;
    private float waitForWavedashTimerSet = .1f;
    private float autoHopDashTimeLeftTemp = 0f;
    private float wavedashTimeLeftTemp = 0f;
    [SerializeField] private float wavedashForce = 50f;
    [SerializeField] private float wavedashJumpForceDivider = 1.5f;
    [SerializeField] private float autoHopJumpForceDivider = 5f;

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

    [HideInInspector] public Rigidbody2D RB;
    private Animator Anim;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform cornerCheck;

    public ParticleSystem jumpDust;
    public ParticleSystem dashDust;
    public ParticleSystem slideDust;
    public ParticleSystem wallJumpDust;
    public ParticleSystem turnDust;

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
    public float cornerFixXOffset1 = 0f;
    public float cornerFixYOffset1 = 0f;

    public Vector2 wallJumpDirection;
    private Vector2 RawShootDirectionInput;
    private Vector2Int ShootDirectionInput;
    private Vector2 cornerPosBot;
    private Vector2 cornerPos1;

    private Camera cam;

    #endregion

    #region Unity Callback Functions

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallJumpDirection.Normalize();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.IsOpen)
        {
            return;
        }
        amountOfDashLeft = amountOfBullets;
        CheckCrouch();
        CheckShootDirection();
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckCornerFix();
        CheckIfCanDash();
        CheckDash();
        CheckForWavedash();
        CheckDialogue();
        CheckDashParticle();
        CheckSurroundings();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    #endregion

    #region Check Functions

    private void CheckInput()
    {
        halfGravMultiplier = (Mathf.Abs(RB.velocity.y) < halfGravThreshold && Input.GetButton("Jump")) ? .5f : 1f;
        RB.gravityScale = halfGravMultiplier * 5;

        if (!waitForDash)
        {
            RawMovementInputDirectionX = Input.GetAxisRaw("Horizontal");
            RawMovementInputDirectionY = Input.GetAxisRaw("Vertical");
            MovementInputDirectionX = Input.GetAxis("Horizontal");
            MovementInputDirectionY = Input.GetAxis("Vertical");
        }

        isJumping = false;
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
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
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * variableJumpHeightMultiplier);
            checkJumpMultiplier = false;
        }

        if (Input.GetButtonDown("Dash"))
        {
            playDashParticle = true;

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
            if (!CharacterMenu.isPaused) { 
                if (amountOfBullets > 0)
                {
                    if (ShootDirectionInput.x == 0 && ShootDirectionInput.y == 0)
                    {
                        Shoot();
                    }
                    else if (ShootDirectionInput.x == 1 && ShootDirectionInput.y == 0)
                    {
                        if (isFacingRight)
                        {
                            Shoot();
                        }
                        else
                        {
                            ShootBehind();
                        }
                    }
                    else if (ShootDirectionInput.x == -1 && ShootDirectionInput.y == 0)
                    {
                        if (isFacingRight)
                        {
                            ShootBehind();
                        }
                        else
                        {
                            Shoot();
                        }
                    }
                    else if (ShootDirectionInput.x == 0 && ShootDirectionInput.y == 1)
                    {
                        ShootUp();
                    }
                    else if (ShootDirectionInput.x == 0 && ShootDirectionInput.y == -1)
                    {
                        ShootDown();
                    }
                    else if (ShootDirectionInput.x == 1 && ShootDirectionInput.y == 1)
                    {
                        ShootUpRight();
                    }
                    else if (ShootDirectionInput.x == 1 && ShootDirectionInput.y == -1)
                    {
                        ShootDownRight();
                    }
                    else if (ShootDirectionInput.x == -1 && ShootDirectionInput.y == 1)
                    {
                        ShootUpLeft();
                    }
                    else if (ShootDirectionInput.x == -1 && ShootDirectionInput.y == -1)
                    {
                        ShootDownLeft();
                    }
                    amountOfBullets -= 1;
                }
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

        isTouchingCorner = Physics2D.Raycast(cornerCheck.position, transform.right, wallCheckDistance, whatIsGround);

        isWavedashDistanceToGround = Physics2D.Raycast(groundCheck.position, -transform.up, wavedashGroundDistanceCheck,
            whatIsGround);
        isAutoJumpDistanceToGround = Physics2D.Raycast(groundCheck.position, -transform.up, autoJumpGroundDistanceCheck,
            whatIsGround);

        if (!isTouchingWall && isTouchingCorner && !cornerDetected)
        {
            cornerDetected = true;
            cornerPosBot = cornerCheck.position;
        }
        else
        {
            cornerDetected = false;
        }

        //Wavedash Checks
        if (!isGrounded)
        {
            isInAir = true;
        }
        else
        {
            isInAir = false;
        }

        if (recentlyJumped)
        {
            recentJumpTimer -= Time.deltaTime;
            if (recentJumpTimer <= 0)
            {
                recentlyJumped = false;
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                recentJumpTimer = recentJumpTimerSet;
            }
        }
        else
        {
            recentJumpTimer = recentJumpTimerSet;
        }

        if (diagonalDashDownDetected)
        {
            diagonalDashDownDetectedTimer -= Time.deltaTime;
            if (diagonalDashDownDetectedTimer <= 0)
            {
                diagonalDashDownDetected = false;
            }
        }
        else
        {
            diagonalDashDownDetectedTimer = diagonalDashDownDetectedTimeSet;
        }
        if (turnOffMovingWhileWavedash)
        {
            waitForWavedashTimer -= Time.deltaTime;
            
            if (waitForWavedashTimer <= 0)
            {
                turnOffMovingWhileWavedash = false;
            }
        }
        else
        {
            waitForWavedashTimer = waitForWavedashTimerSet;
        }
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
        var main = slideDust.main;
        if (isTouchingWall && RawMovementInputDirectionX == facingDirection && RB.velocity.y < 0 && !fixCornerDash)
        {
            isWallSliding = true;
            slideDust.Play();
            main.startColor = Color.white;
        }
        else
        {
            isWallSliding = false;
            main.startColor = Color.clear;
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

                AddAfterImages();
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

    private void CheckShootDirection()
    {
        RawShootDirectionInput = Input.mousePosition;
        RawShootDirectionInput = cam.ScreenToWorldPoint((Vector3) RawShootDirectionInput) - transform.position;
        ShootDirectionInput = Vector2Int.RoundToInt(RawShootDirectionInput.normalized);
    }

    private void CheckCrouch()
    {
        if (RawMovementInputDirectionX == 0 && RawMovementInputDirectionY == -1 && isGrounded)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }

    private void CheckDashParticle()
    {
        if (playDashParticle)
        {
            StartCoroutine(DashParticle());
            playDashParticle = false;
        }
    }

    IEnumerator DashParticle()
    {
        dashDust.Play();
        yield return new WaitForSeconds(.3f);
        dashDust.Stop();
    }

    private void CheckCornerFix()
    {
        if (cornerDetected && isDashing)
        {
            fixCornerDash = true;
            if (isFacingRight)
            {
                cornerPos1 = new Vector2(Mathf.Floor(cornerPosBot.x + wallCheckDistance) + cornerFixXOffset1,
                    Mathf.Floor(cornerPosBot.y) + cornerFixYOffset1);
            }
            else
            {
                cornerPos1 = new Vector2(Mathf.Ceil(cornerPosBot.x - wallCheckDistance) - cornerFixXOffset1,
                    Mathf.Floor(cornerPosBot.y) + cornerFixYOffset1);
            }

        }

        if (fixCornerDash)
        {
            transform.position = cornerPos1;
            fixCornerDash = false;
        }
    }

    //Wavedash Function
    private void CheckForWavedash()
    {
        if (isInAir && recentlyJumped)
        {
            if (diagonalDashDownDetected)
            {
                wavedashInitiated = true;
            }
        }

        if (wavedashInitiated)
        {
            if (Input.GetButtonDown("Jump") && isWavedashDistanceToGround)
            {
                wavedashJumpBufferStore = true;
            }
            if (isGrounded)
            {
                if (wavedashJumpBufferStore || Input.GetButtonDown("Jump"))
                {
                    wavedashTimeLeftTemp = dashTimeLeft;
                    dashTimeLeft = 0;
                    turnOffMovingWhileWavedash = true;
                    wavedashJumpBufferStore = false;
                }
                else if (!wavedashJumpBufferStore || Input.GetButtonDown("Jump"))
                {
                    autoHopBasedOffDashTime = true;
                }
                diagonalDashDownDetected = false;
                wavedashInitiated = false;
            }
        }

        if (wavedashTimeLeftTemp >= 0)
        {
            RB.velocity = new Vector2(wavedashForce * facingDirection, jumpForce / wavedashJumpForceDivider);
            wavedashTimeLeftTemp -= Time.deltaTime;
            AddAfterImages();
        }

        if (autoHopBasedOffDashTime && isAutoJumpDistanceToGround)
        {
            autoHopDashTimeLeftTemp = dashTimeLeft;
            dashTimeLeft = 0;
            turnOffMovingWhileWavedash = true;
            autoHopBasedOffDashTime = false;
        }
        if (autoHopDashTimeLeftTemp >= 0)
        {
            RB.velocity = new Vector2(dashSpeed * facingDirection, jumpForce / autoHopJumpForceDivider);
            autoHopDashTimeLeftTemp -= Time.deltaTime;
            AddAfterImages();
        }
    }

    private void AddAfterImages()
    {
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


    #endregion

    #region Other Functions

    private void UpdateAnimations()
    {
        Anim.SetBool("isMoving", isMoving);
        Anim.SetBool("isGrounded", isGrounded);
        Anim.SetFloat("yVelocity", RB.velocity.y);
        Anim.SetBool("isWallSliding", isWallSliding);
        Anim.SetBool("isCrouching", isCrouching);
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && RawMovementInputDirectionX == 0)
        {
            RB.velocity = new Vector2(RB.velocity.x * airDragMultiplier, RB.velocity.y);
        }
        else if (canMove)
        {
            if (!turnOffMovingWhileWavedash)
            {
                RB.velocity = new Vector2(movementSpeed * RawMovementInputDirectionX, RB.velocity.y);
            }
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
            if (isGrounded)
            {
                turnDust.Play();
            }
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            jumpDust.Play();
            RB.velocity = new Vector2(RB.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            recentlyJumped = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            wallJumpDust.Play();
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
            recentlyJumped = true;
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
                if (isInAir)
                {
                    diagonalDashDownDetected = true;
                }
            }
            else if (RawMovementInputDirectionX == -1 && RawMovementInputDirectionY == 1)
            {
                ShootDownRight();
            }
            else if (RawMovementInputDirectionX == -1 && RawMovementInputDirectionY == -1)
            {
                ShootUpRight();
                if (isInAir)
                {
                    diagonalDashDownDetected = true;
                }
            }
        }
    }

    public void RefillAmmo()
    {
        amountOfBullets = 6;
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
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - wavedashGroundDistanceCheck, groundCheck.position.z));
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - autoJumpGroundDistanceCheck, groundCheck.position.z));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(cornerCheck.position, new Vector3(cornerCheck.position.x + wallCheckDistance, cornerCheck.position.y, cornerCheck.position.z));
    }
}
