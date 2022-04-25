using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPersonalGravity : MonoBehaviour
{   
    public Transform upCollisionCheck;
    public Transform pointA_UCC, pointB_UCC;

    public Transform backCollisionCheck;
    public Transform pointA_BCC, pointB_BCC;

    public Transform frontCollisionCheck;
    public Transform pointA_FCC, pointB_FCC;

    public Color upCollision, backCollision, frontCollision;

    public enum LastTouchCollider { Up, Back, Front, None };
    public LastTouchCollider lastTouchCollider;
    public LastTouchCollider beforeLastTouchCollider;

    public bool _isUpCollision, _isBackCollision, _isFrontCollision;
    private bool _isUpToggle, _isBackToggle, _isFrontToggle;
    public LayerMask whatIsGround;

    public enum SwitchToGravity { Up, Back, Front };
    public SwitchToGravity switchToGravity;


    private PersonalGravity personalGravity;
    private PlayerController playerController;
    private PlayerJumpAndAbilities playerJumpAndAbilities;
    public CameraController cameraController;

    private void Start()
    {
        personalGravity = GetComponent<PersonalGravity>();
        playerController = GetComponent<PlayerController>();
        playerJumpAndAbilities = GetComponent<PlayerJumpAndAbilities>();

        lastTouchCollider = LastTouchCollider.None;
        beforeLastTouchCollider = LastTouchCollider.None;
    }

    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!IsUpCollision && !IsBackCollision && !IsFrontCollision)
            {
                return;
            } else
            {
                DecideGravity();
                SetGravity();
            }         
        }
    }
    

    private void FixedUpdate()
    {
        // collision check, keep in this order (up, back, front)
        IsUpCollision = Physics2D.OverlapArea(pointA_UCC.position, pointB_UCC.position, whatIsGround);
        IsBackCollision = Physics2D.OverlapArea(pointA_BCC.position, pointB_BCC.position, whatIsGround);
        IsFrontCollision = Physics2D.OverlapArea(pointA_FCC.position, pointB_FCC.position, whatIsGround);

        if (!IsUpCollision && !IsBackCollision && !IsFrontCollision)
        {
            lastTouchCollider = LastTouchCollider.None;
            beforeLastTouchCollider = LastTouchCollider.None;
        }

        _isUpCollision = IsUpCollision;
        _isBackCollision = IsBackCollision;
        _isFrontCollision = IsFrontCollision;

    }


    private bool CameraAndPesonalGravityOnSameAxis()
    {
        if (    ((cameraController.currentCameraOrientation == CameraController.CameraOrientation.Up || cameraController.currentCameraOrientation == CameraController.CameraOrientation.Down)
                && 
                (personalGravity.personalGravityDirection == PersonalGravity.PersonalGravityDirection.Up || personalGravity.personalGravityDirection == PersonalGravity.PersonalGravityDirection.Down))
            ||
                ((cameraController.currentCameraOrientation == CameraController.CameraOrientation.Left || cameraController.currentCameraOrientation == CameraController.CameraOrientation.Right)
                && 
                (personalGravity.personalGravityDirection == PersonalGravity.PersonalGravityDirection.Left || personalGravity.personalGravityDirection == PersonalGravity.PersonalGravityDirection.Right)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DecideGravity()
    {
        if (!IsUpCollision)
        {
            if (!IsBackCollision && !IsFrontCollision)              
            {
                return;
            }           // 0 0 0
            else if (!IsBackCollision && IsFrontCollision)        
            {
                // FRONT
                switchToGravity = SwitchToGravity.Front;
            }       // 0 0 1 
            else if (IsBackCollision && !IsFrontCollision)        
            {
                // BACK
                switchToGravity = SwitchToGravity.Back;
            }       // 0 1 0
            else if (IsBackCollision && IsFrontCollision)         
            {
                if (playerJumpAndAbilities.isGrounded)
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                        {
                            if (playerController.facingRight)
                            {
                                if (Input.GetAxis("Horizontal") > 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                            else
                            {
                                if (Input.GetAxis("Horizontal") < 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                        }
                        else
                        {                            
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
                        {
                            if (playerController.facingRight)
                            {
                                if (Input.GetAxis("Vertical") > 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                            else
                            {
                                if (Input.GetAxis("Vertical") < 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                        }
                        else
                        {
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                    }
                }
                else
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                        {
                            if (playerController.facingRight)
                            {
                                if (Input.GetAxis("Horizontal") > 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                            else
                            {
                                if (Input.GetAxis("Horizontal") < 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                        }
                        else
                        {
                            if (lastTouchCollider != LastTouchCollider.Up)
                            {
                                // LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                            } else
                            {
                                // BEFORE LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)beforeLastTouchCollider);
                            }                            
                        }
                    } 
                    else 
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
                        {
                            if (playerController.facingRight)
                            {
                                if (Input.GetAxis("Vertical") > 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                            else
                            {
                                if (Input.GetAxis("Vertical") < 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                        }
                        else
                        {
                            if (lastTouchCollider != LastTouchCollider.Up)
                            {
                                // LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                            }
                            else
                            {
                                // BEFORE LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)beforeLastTouchCollider);
                            }
                        }
                    }                    
                }
            }        // 0 1 1 ^ but needs to be tested
        } else
        {
            if (!IsBackCollision && !IsFrontCollision)              
            {
                // UP 
                switchToGravity = SwitchToGravity.Up;
            }           // 1 0 0 
            else if (!IsBackCollision && IsFrontCollision)          
            {
                if (playerJumpAndAbilities.isGrounded)
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Input.GetAxis("Vertical") > Mathf.Abs(Input.GetAxis("Horizontal")))
                        {
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Input.GetAxis("Vertical") < Mathf.Abs(Input.GetAxis("Horizontal")))
                        {
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                        else
                        {  
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) < (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))) )
                        {
                            // UPs
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Mathf.Abs(Input.GetAxis("Vertical")) > (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))) )
                        {
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                        else
                        {
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                    }
                }
                else
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Input.GetAxis("Vertical") > Mathf.Abs(Input.GetAxis("Horizontal")))
                        {                            
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Input.GetAxis("Vertical") < Mathf.Abs(Input.GetAxis("Horizontal")))
                        {                            
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                        else
                        {
                            if (lastTouchCollider != LastTouchCollider.Back)
                            {
                                // LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                            } else
                            {
                                // BEFORE LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)beforeLastTouchCollider);
                            }
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) < (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {                            
                            // UPs
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Mathf.Abs(Input.GetAxis("Vertical")) > (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {                            
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                        else
                        {
                            if (lastTouchCollider != LastTouchCollider.Back)
                            {
                                // LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                            }
                            else
                            {
                                // BEFORE LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)beforeLastTouchCollider);
                            }
                        }
                    }
                }
            }       // 1 0 1 ^ 
            else if (IsBackCollision && !IsFrontCollision)          
            {               
                if (playerJumpAndAbilities.isGrounded)
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Input.GetAxis("Vertical") > Mathf.Abs(Input.GetAxis("Horizontal")))
                        {                            
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Input.GetAxis("Vertical") < Mathf.Abs(Input.GetAxis("Horizontal")))
                        {                            
                            // BACK
                            switchToGravity = SwitchToGravity.Back;
                        }
                        else
                        {
                            // BACK
                            switchToGravity = SwitchToGravity.Back;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) < (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {                            
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Mathf.Abs(Input.GetAxis("Vertical")) > (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {                            
                            // BACK
                            switchToGravity = SwitchToGravity.Back;
                        }
                        else
                        {                            
                            // BACK
                            switchToGravity = SwitchToGravity.Back;
                        }
                    }
                }
                else
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Input.GetAxis("Vertical") > Mathf.Abs(Input.GetAxis("Horizontal")))
                        {
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Input.GetAxis("Vertical") < Mathf.Abs(Input.GetAxis("Horizontal")))
                        {
                            // BACK
                            switchToGravity = SwitchToGravity.Back;
                        }
                        else
                        {
                            if (lastTouchCollider != LastTouchCollider.Front)
                            {
                                // LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                            } else
                            {
                                // BEFORE LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)beforeLastTouchCollider);
                            }
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) < (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Mathf.Abs(Input.GetAxis("Vertical")) > (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {
                            // BACK
                            switchToGravity = SwitchToGravity.Back;
                        }
                        else
                        {
                            if (lastTouchCollider != LastTouchCollider.Front)
                            {
                                // LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                            }
                            else
                            {
                                // BEFORE LAST TOUCH COLLIDER
                                switchToGravity = (SwitchToGravity)((int)beforeLastTouchCollider);
                            }
                        }
                    }
                }
            }       // 1 1 0 ^
            else if (IsBackCollision && IsFrontCollision)           
            {
                if (playerJumpAndAbilities.isGrounded)
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Input.GetAxis("Vertical") > Mathf.Abs(Input.GetAxis("Horizontal")))
                        {
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Input.GetAxis("Vertical") < Mathf.Abs(Input.GetAxis("Horizontal")))
                        {                            
                            if (Input.GetAxis("Horizontal") < 0) 
                            {                                
                                // BACK
                                switchToGravity = SwitchToGravity.Back;
                            }
                            else
                            {                                
                                // FRONT
                                switchToGravity = SwitchToGravity.Front;
                            }                            
                        }
                        else
                        {                            
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) < (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {                            
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Mathf.Abs(Input.GetAxis("Vertical")) > (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {
                            if (Input.GetAxis("Vertical") < 0)
                            {                                
                                // BACK
                                switchToGravity = SwitchToGravity.Back;
                            }
                            else
                            {
                                // FRONT
                                switchToGravity = SwitchToGravity.Front;
                            }
                        }
                        else
                        {
                            // FRONT
                            switchToGravity = SwitchToGravity.Front;
                        }
                    }
                }
                else
                {
                    if (CameraAndPesonalGravityOnSameAxis())
                    {
                        if (Input.GetAxis("Vertical") > Mathf.Abs(Input.GetAxis("Horizontal")))
                        {                            
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Input.GetAxis("Vertical") < Mathf.Abs(Input.GetAxis("Horizontal")))
                        {
                            if (playerController.facingRight)
                            {
                                if (Input.GetAxis("Horizontal") > 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                            else
                            {
                                if (Input.GetAxis("Horizontal") < 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {                                    
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                } 
                            }                                                                                                                                  
                        }  
                        else
                        {
                            // LAST TOUCH COLLIDER
                            switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) < (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {
                            // UP
                            switchToGravity = SwitchToGravity.Up;
                        }
                        else if (Mathf.Abs(Input.GetAxis("Vertical")) > (playerJumpAndAbilities.CameraRelativeJumpDirectionVector2().x * (Input.GetAxis("Horizontal"))))
                        {                            
                            if (playerController.facingRight)
                            {
                                if (Input.GetAxis("Vertical") > 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                            else
                            {
                                if (Input.GetAxis("Vertical") < 0)
                                {
                                    // FRONT
                                    switchToGravity = SwitchToGravity.Front;
                                }
                                else
                                {
                                    // BACK
                                    switchToGravity = SwitchToGravity.Back;
                                }
                            }
                        }
                        else
                        {
                            // LAST TOUCH COLLIDER
                            switchToGravity = (SwitchToGravity)((int)lastTouchCollider);
                        }
                    }
                }
            }        // 1 1 1 ^
        }
    }

    private void SetGravity()
    {
        bool facingRight = playerController.facingRight;
        int currentPersonalGravityIndex = (int)personalGravity.personalGravityDirection;
        int direction;
        int newPersonalGravityIndex;
        
        // decide if going clockwise [1] ( Down, Left, Up, Right ) or counterclockwise [-1] ( Down, Rigth, Up, Left )
        if (facingRight)
        {
            direction = 1;
        } else
        {
            direction = -1;
        }
        
        
        if (switchToGravity == SwitchToGravity.Up)
        {
            newPersonalGravityIndex = currentPersonalGravityIndex + (direction * 2);
            newPersonalGravityIndex = normalizeRange(newPersonalGravityIndex);                       
            personalGravity.personalGravityDirection = (PersonalGravity.PersonalGravityDirection)(newPersonalGravityIndex);

        } else if (switchToGravity == SwitchToGravity.Back)
        {
            newPersonalGravityIndex = currentPersonalGravityIndex + (direction);
            newPersonalGravityIndex = normalizeRange(newPersonalGravityIndex);
            personalGravity.personalGravityDirection = (PersonalGravity.PersonalGravityDirection)(newPersonalGravityIndex);

        } else if (switchToGravity == SwitchToGravity.Front)
        {
            newPersonalGravityIndex = currentPersonalGravityIndex - (direction);
            newPersonalGravityIndex = normalizeRange(newPersonalGravityIndex);
            personalGravity.personalGravityDirection = (PersonalGravity.PersonalGravityDirection)(newPersonalGravityIndex);

        }   
      
    }

    public static int normalizeRange(int indexZeroToThree)
    {
        if (indexZeroToThree < 0)
        {
            return indexZeroToThree + 4;
        } else if (indexZeroToThree > 3)
        {
            return indexZeroToThree - 4;
        }
        else
        {
            return indexZeroToThree;
        }



        /*

        if (indexZeroToThree == -1)
        {
            return 3;
        } else if (indexZeroToThree == -2)
        {
            return 2;
        }
        else if (indexZeroToThree == 5)
        {
            return 1;
        }
        else if (indexZeroToThree == 4)
        {
            return 0;
        }
        else
        {
            return indexZeroToThree;
        }
        */

    }

    public bool IsUpCollision
    {
        get { return _isUpToggle; }
        set
        {
            if (_isUpToggle != value)
            {
                _isUpToggle = value;
                if (value)
                {
                    beforeLastTouchCollider = lastTouchCollider;
                    lastTouchCollider = LastTouchCollider.Up;
                }
            }
        }
    }

    public bool IsBackCollision
    {
        get { return _isBackToggle; }
        set
        {
            if (_isBackToggle != value)
            {
                _isBackToggle = value;
                if (value)
                {
                    beforeLastTouchCollider = lastTouchCollider;
                    lastTouchCollider = LastTouchCollider.Back;
                }
            }
        }
    }

    public bool IsFrontCollision
    {
        get { return _isFrontToggle; }
        set
        {
            if (_isFrontToggle != value)
            {
                _isFrontToggle = value;
                if (value)
                {
                    beforeLastTouchCollider = lastTouchCollider;
                    lastTouchCollider = LastTouchCollider.Front;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // to keep rotation, not sure how it works, also had to use .localPostion instead of .position
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = upCollision;
        Gizmos.DrawCube(upCollisionCheck.localPosition, new Vector3(0.50f, 2f, 0.01f));

        Gizmos.color = backCollision;
        Gizmos.DrawCube(backCollisionCheck.localPosition, new Vector3(2f, 0.50f, 0.01f));

        Gizmos.color = frontCollision;
        Gizmos.DrawCube(frontCollisionCheck.localPosition, new Vector3(2f, 0.50f, 0.01f));
    }

}
