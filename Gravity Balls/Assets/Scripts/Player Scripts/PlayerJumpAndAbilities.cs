using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAndAbilities : MonoBehaviour
{
    public float airSpeed;
    public float groundSpeed;

    public enum JumpForceStrength { Light, Normal, Heavy };
    public JumpForceStrength jumpForceStrength;

    public float lightJumpForce;
    public float lightAirSpeed;
    public float normalJumpForce;
    public float normalAirSpeed;
    public float heavyJumpForce;
    public float heavyAirSpeed;

    private float jumpForce;

    private float fallingFloat;
    public float defaultFallSpeed;
    public float terminalVelocity;
    public float smoothFallFactor;
    public float glideFallSpeed;
    public float glideSpeed;
    public float diveBombFallSpeed;
    public float diveBombSpeed;


    private bool specialAbilityGlideActive;
    private bool specialAbilityDiveBombActive;


    public int extraJumpsValue;
    private int extraJumps;
    public Vector2 jumpDirection;

    private Rigidbody2D rb;

    public Transform groundCheck;
    public Transform pointA_GC, pointB_GC;

    public Color groundCheckColor;

    public bool isGrounded;
    public bool isFalling;  
    private bool isJumpableObject;
    public LayerMask whatIsGround;
    public LayerMask[] whatIsJumpableObject;

    private PlayerController playerController;
    public CameraController cameraController;

    void Start()
    {     
        jumpForceStrength = JumpForceStrength.Normal;

        specialAbilityGlideActive = false;
        specialAbilityDiveBombActive = false;

        jumpDirection = Vector2.up;
        jumpForce = normalJumpForce;
        extraJumps = extraJumpsValue;

        rb = GetComponent<Rigidbody2D>();

        playerController = GetComponent<PlayerController>();
        playerController.speed = groundSpeed;
     }    

    private void Update()
    {
        //print("[Jump Direction] x: " + jumpDirection.x + " y: " + jumpDirection.y + "   [Velocity] x: " + rb.velocity.x + " y: " + rb.velocity.y);

        DecidePlayerJumpStrenght();
            
        if (!specialAbilityGlideActive && !specialAbilityDiveBombActive)
        {
            SetPlayerJumpStrenght();
        }

        Jump();

        Falling();

        if (isFalling && !specialAbilityGlideActive && !specialAbilityDiveBombActive)
        {
            float smoothedFall = Mathf.Lerp(airSpeed, defaultFallSpeed, smoothFallFactor * Time.fixedDeltaTime);        // keeps speed the same regardless of fps
            airSpeed = smoothedFall;             
        }

        if (isGrounded)
        {
            playerController.speed = groundSpeed;            
        } else
        {
            playerController.speed = airSpeed;
        }
        
    }

    private void FixedUpdate()
    {
        // jump check
        isGrounded = Physics2D.OverlapArea(pointA_GC.position, pointB_GC.position, whatIsGround);
        isJumpableObject = Physics2D.OverlapArea(pointA_GC.position, pointB_GC.position, whatIsJumpableObject[0]);

        GlideAndDiveBomb();

        // limit downward fall velocity
        if (isFalling && !specialAbilityDiveBombActive && fallingFloat < -terminalVelocity)
        {
            rb.velocity = (-jumpDirection * terminalVelocity); 
        }
    }

    private void DecidePlayerJumpStrenght()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            jumpForceStrength = JumpForceStrength.Heavy;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            jumpForceStrength = JumpForceStrength.Normal;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            jumpForceStrength = JumpForceStrength.Light;
        }
    }

    private void SetPlayerJumpStrenght()
    {
        if (jumpForceStrength == JumpForceStrength.Heavy)
        {
            jumpForce = heavyJumpForce;
            airSpeed = heavyAirSpeed;
        }
        else if (jumpForceStrength == JumpForceStrength.Normal)
        {
            jumpForce = normalJumpForce;
            airSpeed = normalAirSpeed;
        }
        else if (jumpForceStrength == JumpForceStrength.Light)
        {            
            jumpForce = lightJumpForce;
            airSpeed = lightAirSpeed;
        }
    }

    private void Jump()
    {

        if (isGrounded || isJumpableObject)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = jumpDirection * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && (isGrounded || isJumpableObject))
        {
            rb.velocity = jumpDirection * jumpForce;
        }
    }

    private void Falling()
    {
        
        Vector2 fallingVector = new Vector2(jumpDirection.x * rb.velocity.x, jumpDirection.y * rb.velocity.y); 

        if (fallingVector.x == 0)
        {
            fallingFloat = fallingVector.y;
        } else
        {
            fallingFloat = fallingVector.x;
        }
        
        if (fallingFloat >= 0)
        {
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }
    }

    private void GlideAndDiveBomb()
    {        
        if (Input.GetKey(KeyCode.Space) && isFalling) // && jumpForceStrength == JumpForceStrength.Light)
        {
            specialAbilityGlideActive = true;
            rb.velocity = (-jumpDirection * glideFallSpeed);
            float smoothedFall = Mathf.Lerp(airSpeed, glideSpeed, smoothFallFactor * Time.fixedDeltaTime);        // keeps speed the same regardless of fps
            airSpeed = smoothedFall;
        }
        else
        {
            specialAbilityGlideActive = false;
        }

        if (Input.GetKey(KeyCode.LeftControl)) // && jumpForceStrength == JumpForceStrength.Heavy)
        {
            specialAbilityDiveBombActive = true;
            rb.velocity = (-jumpDirection * diveBombFallSpeed);
            float smoothedFall = Mathf.Lerp(airSpeed, diveBombSpeed, smoothFallFactor * Time.fixedDeltaTime);        // keeps speed the same regardless of fps
            airSpeed = smoothedFall;
        } 
        else
        {
            specialAbilityDiveBombActive = false;
        }
    }

    public Vector2 CameraRelativeJumpDirectionVector2()
    {
        switch (cameraController.currentCameraOrientation)
        {
            case CameraController.CameraOrientation.Left:
                if (jumpDirection == Vector2.down)
                {
                    return Vector2.right;
                }
                else if (jumpDirection == Vector2.left)
                {
                    return Vector2.down;
                }
                else if (jumpDirection == Vector2.up)
                {
                    return Vector2.left;
                }
                else // (jumpDirection == Vector2.right)
                {
                    return Vector2.up;
                }
            case CameraController.CameraOrientation.Up:
                if (jumpDirection == Vector2.down)
                {
                    return Vector2.up;
                }
                else if (jumpDirection == Vector2.left)
                {
                    return Vector2.right;
                }
                else if (jumpDirection == Vector2.up)
                {
                    return Vector2.down;
                }
                else // (jumpDirection == Vector2.right)
                {
                    return Vector2.left;
                }
            case CameraController.CameraOrientation.Right:
                if (jumpDirection == Vector2.down)
                {
                    return Vector2.left;
                }
                else if (jumpDirection == Vector2.left)
                {
                    return Vector2.up;
                }
                else if (jumpDirection == Vector2.up)
                {
                    return Vector2.right;
                }
                else // (jumpDirection == Vector2.right)
                {
                    return Vector2.down;
                }
            default:
                return jumpDirection; // case CameraController.CameraOrientation.Down:
        }
    }   

    private void OnDrawGizmos()
    {
        // to keep rotation, not sure how it works, also had to use .localPostion instead of .position
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = groundCheckColor;
        Gizmos.DrawCube(groundCheck.localPosition, new Vector3(0.78f, 0.1f, 0.01f));

    }


}
