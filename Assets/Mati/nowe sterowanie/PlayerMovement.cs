using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool drawDebugRaycasts = true;

    [Header("Movment Propertise")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;
    public float coyoteDuration = .05f;
    public float maxFallSpeed = -25f;

    [Header("Jump Properties")]
    public float jumpForce = 6.3f;
    public float crouchJumpBoost = 2.5f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = .1f;

    [Header("Enviroment Check Properties")]
    public float footOffset = .4f;
    public float headClearance = .5f;
    public float groundDistance = .2f;
    public LayerMask groundLayer;

    [Header("Status Flags")]
    public bool isOnGround;
    public bool isJumping;
    public bool isCrouching;
    public bool isHeadBlocked;

    PlayerInput input;
    BoxCollider2D bodyCollider;
    Rigidbody2D rigidBody;

    float jumpTime;
    float coyoteTime;
    float playerHeight;

    float originalXScale;
    int direction = 1;

    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();

        originalXScale = transform.localScale.x;

        playerHeight = bodyCollider.size.y;

        colliderStandSize = bodyCollider.size;
        colliderStandOffset = bodyCollider.offset;

        colliderCrouchSize = new Vector2(bodyCollider.size.x, bodyCollider.size.y / 2f);
        colliderStandOffset = new Vector2(bodyCollider.offset.x, bodyCollider.offset.y / 2f);

    }

    void FixedUpdate()
    {
        PhysicsCheck();

        GroundMovement();
        MidAirMovement();
    }

    void PhysicsCheck()
    {
        isOnGround = false;
        isHeadBlocked = false;

        RaycastHit2D leftcheck = Raycast(new Vector2(-footOffset, -0.5f), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, -0.5f), Vector2.down, groundDistance);


        if (leftcheck || rightCheck)
            isOnGround = true;

        RaycastHit2D headCheck = Raycast(new Vector2(0f, 0.2f), Vector2.up, headClearance);

        if (headCheck)
            isHeadBlocked = true;

    }

    void GroundMovement()
    {
        if (input.crouchHeld && !isCrouching && isOnGround)
            Crouch();
        else if (!input.crouchHeld && isCrouching)
            StandUp();
        else if (!isOnGround && isCrouching)
            StandUp();

        float xVelocity = speed * input.horizontal;

        if (xVelocity * direction < 0f)
            FlipCharacterDirection();

        if (isCrouching)
            xVelocity /= crouchSpeedDivisor;

        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

        if (isOnGround)
            coyoteTime = Time.time + coyoteDuration;
    }

    void MidAirMovement()
    {

    
    // Update is called once per frame
    
    
        if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time))
        {
            if (isCrouching && !isHeadBlocked)
            {
                StandUp();
                rigidBody.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            isOnGround = false;
            isJumping = true;

            jumpTime = Time.time + jumpHoldDuration;

            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        }
        else if (isJumping)
        {
            if (input.jumpHeld)
                rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

            if (jumpTime <= Time.time)
                isJumping = false;
        }
        if (rigidBody.velocity.y < maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
    }

    void FlipCharacterDirection()
    {
        direction *= -1;

        Vector3 scale = transform.localScale;

        scale.x = originalXScale * direction;

        transform.localScale = scale;

    }

    void Crouch()
    {

        isCrouching = true;

        bodyCollider.size = colliderCrouchSize;
        bodyCollider.offset = colliderCrouchOffset;

    }

    void StandUp()
    {
        if (isHeadBlocked)
            return;

        isCrouching = false;

        bodyCollider.size = colliderStandSize;
        bodyCollider.offset = colliderStandOffset;
    }
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        return Raycast(offset, rayDirection, length, groundLayer);
    }
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        return hit;
    }
}