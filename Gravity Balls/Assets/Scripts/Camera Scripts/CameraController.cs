using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraOrientation { Down, Up, Left, Right };
    public CameraOrientation previousCameraOrientation, currentCameraOrientation;

    public Transform player;
    public Vector3 playerLayerOffset;

    private bool cameraToggle;
    public bool cameraFollowRotation;

    [Range(1,10)]
    public float smoothCameraFactor;
    private Vector3 boundPosition;
    public Vector3 minHorizonalValues, maxHorizonalValues;
    public Vector3 minVerticalValues, maxVerticalValues;

    private bool random;

    private void Start()
    {
        cameraToggle = false;
        cameraFollowRotation = false;
        random = false;
        currentCameraOrientation = CameraOrientation.Down;
    }

    private void Update()
    {

        //print(currentCameraOrientation);
        //print(transform.rotation.eulerAngles.z);


        // always rotate camera or rotate only if key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            cameraToggle = !cameraToggle;
        }

        if (cameraToggle)
        {
            cameraFollowRotation = true;
        } else
        {
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                cameraFollowRotation = true;
            }
            else
            {
                cameraFollowRotation = false;
            }
        }

    }

    private void FixedUpdate()
    {
        GetOrientation();
        
        // follow player smoothly and set camera bounds
        Follow();      

        // rotate camera to player orientation (if LEFTSHIFT is not held down) or to default down orientation
        if (cameraFollowRotation && player.GetComponent<PersonalGravity>().personalGravityOn)
        {   
            if (transform.rotation.z != player.rotation.z)
            {
                //print("here");
                previousCameraOrientation = currentCameraOrientation;                
                //print("Previous Orientation: " + previousCameraOrientation);                
                random = true;
            }
            transform.rotation = player.rotation;                          // don't just modify z, idk why, gut feeling
            if (random)
            {
                GetOrientation();
                //print("Current Orientation: " + currentCameraOrientation);
                random = false;
            }
            
        } else if (!player.GetComponent<PersonalGravity>().personalGravityOn)
        {
            if (transform.rotation.eulerAngles.z != GetWorldGravityZRotation())
            {
                //print("there");
                previousCameraOrientation = currentCameraOrientation;           
                //print("Previous Orientation: " + previousCameraOrientation);
                random = true;
            }
            transform.rotation = Quaternion.Euler(0, 0, GetWorldGravityZRotation());                // don't just modify z, idk why, gut feeling
            if (random)
            {
                GetOrientation();
                //print("Current Orientation: " + currentCameraOrientation);
                random = false;
            }
        } else
        {
            return;
        }
        
    }

    private void Follow()
    {
        // get camera in front of player not on same layer
        Vector3 targetPosition = player.position + playerLayerOffset;

        // check and limit bounds which camera can be in
        if (currentCameraOrientation == CameraOrientation.Up || currentCameraOrientation == CameraOrientation.Down)
        {
            boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minHorizonalValues.x, maxHorizonalValues.x),
            Mathf.Clamp(targetPosition.y, minHorizonalValues.y, maxHorizonalValues.y),
            Mathf.Clamp(targetPosition.z, minHorizonalValues.z, maxHorizonalValues.z));

        } else if (currentCameraOrientation == CameraOrientation.Left || currentCameraOrientation == CameraOrientation.Right)
        {
            boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minVerticalValues.x, maxVerticalValues.x),
            Mathf.Clamp(targetPosition.y, minVerticalValues.y, maxVerticalValues.y),
            Mathf.Clamp(targetPosition.z, minVerticalValues.z, maxVerticalValues.z));
        }               

        // smooth out follow transition, have it lag to make it look cool 
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundPosition, smoothCameraFactor * Time.fixedDeltaTime);      // keeps speed the same regardless of fps
        transform.position = smoothedPosition;
    }

    private void GetOrientation()
    {
        if (transform.rotation.eulerAngles.z == 180f)
        {
            currentCameraOrientation = CameraOrientation.Up;
        } else if(transform.rotation.eulerAngles.z == 0f)
        {
            currentCameraOrientation = CameraOrientation.Down;
        }
        else if (transform.rotation.eulerAngles.z == 270f)
        {
            currentCameraOrientation = CameraOrientation.Left;
        }
        else if (transform.rotation.eulerAngles.z == 90f)
        {
            currentCameraOrientation = CameraOrientation.Right;
        }
    }

    private float GetWorldGravityZRotation()
    {
        switch (player.GetComponent<WorldGravity>().worldGravityDirection)
        {
            case WorldGravity.WorldGravityDirection.Down:
                return 0f;
            case WorldGravity.WorldGravityDirection.Left:
                return 270f;
            case WorldGravity.WorldGravityDirection.Up:
                return 180f;
            case WorldGravity.WorldGravityDirection.Right:
                return 90f;
            default:
                return 0f;                
        }
    }


}
