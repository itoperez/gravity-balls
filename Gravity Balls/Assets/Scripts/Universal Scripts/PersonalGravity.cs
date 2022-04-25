using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalGravity : MonoBehaviour
{
    public enum PersonalGravityDirection { Down, Left, Up, Right };         // DO NOT CHANGE ORDER IT IS IMPORTANT
    public PersonalGravityDirection personalGravityDirection;
        

    public float lightGravity, normalGravity, heavyGravity;
    public float gravity;

    private Rigidbody2D rb;

    public bool isPlayer;
    public bool personalGravityOn;    

    [SerializeField]
    Color[] gravityStrenghtColors;

    SpriteRenderer spriteRenderer;

    private Outline outline;
    private bool outlineIsOn;


    public bool isInteractableObject;
    public bool isInInteractableArea;
    public float interactableRadius;
    public Color interactableColor;
    public LayerMask whatIsPlayer;

    // TODO: FIX LATER WITH GLOBAL GRAVITIES
    private WorldGravity worldGravity;

    public Vector3 startPosition;

    private void Start()
    {
        if (isPlayer)
        {
            personalGravityOn = false;
            outline = GetComponent<Outline>();
            outlineIsOn = false;
        }
        
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        // TODO: FIX LATER WITH GLOBAL GRAVITIES
        worldGravity = GetComponent<WorldGravity>();

        startPosition = transform.position;

    }

    private void Update()
    {
        // set gravity strenght and direction is personal gravity is on else set defaults for when its later turned on
        if (personalGravityOn && isPlayer)
        {
            //SetGravityStrenght();
            //SetGravityDirectionManualInput();

        } else if (personalGravityOn && !isPlayer)
        {
            gravity = normalGravity;
            
        } else
        {        
            // set to copy whatever world gravity was when personal gravity is turned on [TODO: FIX LATER WITH GLOBAL GRAVITIES]
            gravity = worldGravity.gravity;
            switch (worldGravity.worldGravityDirection)
            {
                case WorldGravity.WorldGravityDirection.Up:
                    personalGravityDirection = PersonalGravityDirection.Up;
                    break;
                case WorldGravity.WorldGravityDirection.Down:
                    personalGravityDirection = PersonalGravityDirection.Down;
                    break;
                case WorldGravity.WorldGravityDirection.Left:
                    personalGravityDirection = PersonalGravityDirection.Left;
                    break;
                case WorldGravity.WorldGravityDirection.Right:
                    personalGravityDirection = PersonalGravityDirection.Right;
                    break;
            }
        }



        // reset objects
        if (isInteractableObject)
        {
            isInInteractableArea = Physics2D.OverlapCircle(transform.position, interactableRadius, whatIsPlayer);

            if (isInInteractableArea && Input.GetButton("Fire2"))
            {
                ResetStartPostion();
            }
        }

    }

    private void FixedUpdate()
    {
        if (personalGravityOn)
        {
            rb.gravityScale = 0;

            // Apply gravity direction and strenght
            switch (personalGravityDirection)
            {
                case PersonalGravityDirection.Down:
                    rb.AddForce(new Vector2(0, -gravity));
                    break;
                case PersonalGravityDirection.Up:
                    rb.AddForce(new Vector2(0, gravity));
                    break;
                case PersonalGravityDirection.Left:
                    rb.AddForce(new Vector2(-gravity, 0));
                    break;
                
                case PersonalGravityDirection.Right:
                    rb.AddForce(new Vector2(gravity, 0));
                    break;
            }
        }
        else
        {
            rb.gravityScale = 1;
        }              
                        
    }

    public void TogglePersonalGravity()
    {
        outlineIsOn = !outlineIsOn;
        personalGravityOn = !personalGravityOn;
        outline.enabled = outlineIsOn;
    }

    private void SetGravityStrenght()
    {   
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gravity == heavyGravity)
                return;
            gravity = heavyGravity;
            //spriteRenderer.color = gravityStrenghtColors[0];
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            if (gravity == normalGravity)
                return;
            gravity = normalGravity;
            //spriteRenderer.color = gravityStrenghtColors[1];
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            if (gravity == lightGravity)
                return;
            gravity = lightGravity;
            //spriteRenderer.color = gravityStrenghtColors[2];
        }

    } // sets personal gravity strenght

    private void SetGravityDirectionManualInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (personalGravityDirection == PersonalGravityDirection.Left)
                return;
            personalGravityDirection = PersonalGravityDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (personalGravityDirection == PersonalGravityDirection.Down)
                return;
            personalGravityDirection = PersonalGravityDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (personalGravityDirection == PersonalGravityDirection.Up)
                return;
            personalGravityDirection = PersonalGravityDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (personalGravityDirection == PersonalGravityDirection.Right)
                return;
            personalGravityDirection = PersonalGravityDirection.Right;
        }

    } // sets personal gravity direction with keyboard input
        
    public void ResetStartPostion()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = 0f;
        transform.position = startPosition;                
    }

    private void OnDrawGizmos()
    {
        if (isInteractableObject)
        {
            Gizmos.color = interactableColor;
            Gizmos.DrawSphere(transform.position, interactableRadius);
        }
        
    }


}
