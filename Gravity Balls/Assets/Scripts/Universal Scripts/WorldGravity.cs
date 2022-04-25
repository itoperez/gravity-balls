using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGravity : MonoBehaviour
{
    public enum WorldGravityDirection { Down, Left, Up, Right };
    public WorldGravityDirection worldGravityDirection;

    public float lightGravity, normalGravity, heavyGravity;
    public float gravity;

    private void Start()
    {
        worldGravityDirection = WorldGravityDirection.Down;
        
        gravity = normalGravity;
    }

    private void Update()
    {
        SetGravityStrenght();

        SetGravityDirection();

    } // Update

    private void FixedUpdate()
    {
        // Apply gravity direction and strenght
        switch (worldGravityDirection)
        {
            case WorldGravityDirection.Down:
                Physics2D.gravity = new Vector2(0, -gravity);
                break;
            case WorldGravityDirection.Left:
                Physics2D.gravity = new Vector2(-gravity, 0);
                break;
            case WorldGravityDirection.Up:
                Physics2D.gravity = new Vector2(0, gravity);
                break;
            case WorldGravityDirection.Right:
                Physics2D.gravity = new Vector2(gravity, 0);
                break;
        }

    } // Fixed Update

    private void SetGravityStrenght()
    {   
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (gravity == heavyGravity)
                return;
            gravity = heavyGravity;
        } else if (Input.GetKeyDown(KeyCode.I))
        {
            if (gravity == normalGravity)
                return;
            gravity = normalGravity;
        } else if (Input.GetKeyDown(KeyCode.O))
        {
            if (gravity == lightGravity)
                return;
            gravity = lightGravity;
        }
        

    } // sets world gravity strenght

    private void SetGravityDirection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (worldGravityDirection == WorldGravityDirection.Left)            
                return;
            worldGravityDirection = WorldGravityDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (worldGravityDirection == WorldGravityDirection.Down)
                return;
            worldGravityDirection = WorldGravityDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (worldGravityDirection == WorldGravityDirection.Up)            
                return;                        
            worldGravityDirection = WorldGravityDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (worldGravityDirection == WorldGravityDirection.Right)            
                return;                        
            worldGravityDirection = WorldGravityDirection.Right;
        }

    } // sets world gravity direction

} 

