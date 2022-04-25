using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerGravityDirection { Down, Left, Up, Right };
    public PlayerGravityDirection playerGravityDirection;

    public float speed;   

    private float moveInputHorizonatal;
    private float moveInputVertical;
    
    private float moveInputUpDown;
    private float moveInputLeftRight;       

    public bool facingRight;

    [Range(0, 2)]
    public float smoothRotationFactor;
    private Vector3 targetRotation;

    private Rigidbody2D rb;
    private bool isNoninteractableObject;
    public LayerMask whatIsInteractableObject;
    //public LayerMask whatIsNoninteractableObject;


    private WorldGravity worldGravity;
    private PersonalGravity personalGravity;
    private PlayerJumpAndAbilities playerJumpAndAbilities;
    public CameraController cameraController;
        

    private void Start()
    {
        facingRight = true;

        worldGravity = GetComponent<WorldGravity>();
        personalGravity = GetComponent<PersonalGravity>();
        playerJumpAndAbilities = GetComponent<PlayerJumpAndAbilities>();

        rb = GetComponent<Rigidbody2D>();        
    }

    private void Update()
    {
        // apply world or personal gravity
        GetGravity();

        // rotational direction player orientation
        Rotation();

        // facing direction player orientation
        Facing();

        // toggle personal gravity on and off
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            personalGravity.TogglePersonalGravity();
        }


    } // Update

    private void FixedUpdate()
    {
        // jump check
        //isGrounded = Physics2D.OverlapArea(pointA_GC.position, pointB_GC.position, whatIsGround);
        //isJumpableObject = Physics2D.OverlapArea(pointA_GC.position, pointB_GC.position, whatIsJumpableObject[0]);              
               

        // movement stuff and adjustments
        moveInputHorizonatal = Input.GetAxis("Horizontal");
        moveInputVertical = Input.GetAxis("Vertical");

        Movement();
              

    } // Fixed Update

    private void GetGravity()
    {
        if (personalGravity.personalGravityOn)
        {
            switch (personalGravity.personalGravityDirection)
            {
                case PersonalGravity.PersonalGravityDirection.Up:
                    playerGravityDirection = PlayerGravityDirection.Up;
                    break;
                case PersonalGravity.PersonalGravityDirection.Down:
                    playerGravityDirection = PlayerGravityDirection.Down;
                    break;
                case PersonalGravity.PersonalGravityDirection.Left:
                    playerGravityDirection = PlayerGravityDirection.Left;
                    break;
                case PersonalGravity.PersonalGravityDirection.Right:
                    playerGravityDirection = PlayerGravityDirection.Right;
                    break;
            }
        }
        else
        {
            switch (worldGravity.worldGravityDirection)
            {
                case WorldGravity.WorldGravityDirection.Up:
                    playerGravityDirection = PlayerGravityDirection.Up;
                    break;
                case WorldGravity.WorldGravityDirection.Down:
                    playerGravityDirection = PlayerGravityDirection.Down;
                    break;
                case WorldGravity.WorldGravityDirection.Left:
                    playerGravityDirection = PlayerGravityDirection.Left;
                    break;
                case WorldGravity.WorldGravityDirection.Right:
                    playerGravityDirection = PlayerGravityDirection.Right;
                    break;
            }
        }

    }

    private void Rotation()
    {
        switch (playerGravityDirection)
        {
            case PlayerGravityDirection.Up:
                //targetRotation = new Vector3(0, 0, 180f);
                transform.eulerAngles = new Vector3(0, 0, 180f);
                playerJumpAndAbilities.jumpDirection = Vector2.down;
                break;
            case PlayerGravityDirection.Down:
                //targetRotation = new Vector3(0, 0, 0);
                transform.eulerAngles = new Vector3(0, 0, 0);
                playerJumpAndAbilities.jumpDirection = Vector2.up;
                break;
            case PlayerGravityDirection.Left:
                //targetRotation = new Vector3(0, 0, -90f);
                transform.eulerAngles = new Vector3(0, 0, -90f);
                playerJumpAndAbilities.jumpDirection = Vector2.right;
                break;
            case PlayerGravityDirection.Right:
                //targetRotation = new Vector3(0, 0, 90f);
                transform.eulerAngles = new Vector3(0, 0, 90f);
                playerJumpAndAbilities.jumpDirection = Vector2.left;
                break;
        }

        //Vector3 smoothedRotation = Vector3.Lerp(transform.eulerAngles, targetRotation, smoothRotationFactor * Time.fixedDeltaTime);      // keeps speed the same regardless of fps
        //transform.eulerAngles = smoothedRotation;

    }

    private void CorrectInputs()
    {
        switch (cameraController.currentCameraOrientation)
        {
            case CameraController.CameraOrientation.Down:
                moveInputLeftRight = moveInputHorizonatal;
                moveInputUpDown = moveInputVertical;
                break;
            case CameraController.CameraOrientation.Left:
                moveInputLeftRight = moveInputVertical;
                moveInputUpDown = -moveInputHorizonatal;
                break;
            case CameraController.CameraOrientation.Up:
                moveInputLeftRight = -moveInputHorizonatal;
                moveInputUpDown = -moveInputVertical;
                break;
            case CameraController.CameraOrientation.Right:
                moveInputLeftRight = -moveInputVertical;
                moveInputUpDown = moveInputHorizonatal;
                break;
        }


    } // corrects the inputs to work with relation to the current Camera orientation

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

    } // flips character to face oposite direction
        
    private void Facing()
    {
        CorrectInputs();

        if (playerGravityDirection == PlayerGravityDirection.Down)
        {
            if (facingRight == false && moveInputLeftRight > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInputLeftRight < 0)
            {
                Flip();
            }
        } else if (playerGravityDirection == PlayerGravityDirection.Left)
        {
            if (facingRight == false && moveInputUpDown < 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInputUpDown > 0)
            {
                Flip();
            }
        } else if (playerGravityDirection == PlayerGravityDirection.Up) // && !cameraController.cameraFollowRotation)
        {
            if (facingRight == false && moveInputLeftRight < 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInputLeftRight > 0)
            {
                Flip();
            }
        } else if (playerGravityDirection == PlayerGravityDirection.Right)
        {
            if (facingRight == false && moveInputUpDown > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInputUpDown < 0)
            {
                Flip();
            }
        }

    } // faces player to correct direction based on inputs          
    
    private void Movement()
    {
        CorrectInputs();

        if (playerGravityDirection == PlayerGravityDirection.Up || playerGravityDirection == PlayerGravityDirection.Down)
        {
            Vector2 movementUpDown = new Vector2(moveInputLeftRight * speed, 0f);
            movementUpDown.y = rb.velocity.y;
            rb.velocity = movementUpDown;
        }
        else if (playerGravityDirection == PlayerGravityDirection.Right || playerGravityDirection == PlayerGravityDirection.Left)
        {
            Vector2 movementRight = new Vector2(0f, moveInputUpDown * speed);
            movementRight.x = rb.velocity.x;
            rb.velocity = movementRight;
        }


    } // moves player in direction of inputs
    
}
