using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraMovement : MonoBehaviour
{
    private characterControl charCon;
    private Vector3 offset;
    public float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Vector3 speed;
    public Vector3 startingPosition;
    public Vector3 positionBeforeJump;
    public Vector3 positionBeforeFall;
    public Vector3 targetPosition;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineFramingTransposer virtualTransposer;
    public float regularPosition;
    public float lookAbovePosition;
    public float lookBelowPosition;
    public float currentPosition;
    public int cameraMoveDirection;
    public float bottomPosition;
    public float topPosition;
    public float waitToLookTime;
    public float waitToLookTimeCounter;

    void Start()
    {
        currentPosition = regularPosition;
        charCon = GameObject.FindObjectOfType<characterControl>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        virtualTransposer = virtualCamera.GetComponentInChildren<CinemachineFramingTransposer>();
        transform.position = new Vector3(charCon.transform.position.x, charCon.transform.position.y, -10f);
        startingPosition = transform.position;
        offset = new Vector3(0f, 2f, -10f);
        
    }

    void FixedUpdate()
    {   
        if(speed.x == 0 && (!charCon.isMoving()))
        {
            if(Input.GetKey(KeyCode.LeftControl))
            {
                cameraMoveDirection = -2;
                bottomPosition = lookBelowPosition;
                topPosition = regularPosition;
            }
            else if(Input.GetKey(KeyCode.W))
            {
                cameraMoveDirection = 2;
                bottomPosition = regularPosition;
                topPosition = lookAbovePosition;
            }
            else cameraMoveDirection = 0;
        }
        else cameraMoveDirection = 0;
        if(waitToLookTimeCounter < 0 || !(currentPosition < regularPosition + 0.1f && currentPosition > regularPosition - 0.1f))
        {
            currentPosition = Mathf.Clamp(currentPosition + Time.fixedDeltaTime * cameraMoveDirection, bottomPosition, topPosition);
            virtualTransposer.m_TrackedObjectOffset = new Vector3(virtualTransposer.m_TrackedObjectOffset.x, currentPosition, virtualTransposer.m_TrackedObjectOffset.z);
        }
        if(cameraMoveDirection == 0)
        {
            waitToLookTimeCounter = waitToLookTime;
            if(currentPosition < regularPosition) currentPosition += Time.fixedDeltaTime * 4;
            else if(currentPosition > regularPosition) currentPosition -= Time.fixedDeltaTime * 4;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.LeftControl)) waitToLookTimeCounter -= Time.fixedDeltaTime;
    }
    /* Default camera code
    private void DefaultCamera()
    {
        speed = charCon.myrigidbody.velocity * 3f;
        if(charCon.myrigidbody.velocity.y == 0)
        {
            positionBeforeJump = charCon.transform.position;
        }
        if(charCon.myrigidbody.velocity.y > 0)
        {
            positionBeforeFall = charCon.transform.position;
        }
        if(speed.x > 0) 
        {   
            if(startingPosition.x - 2 >= charCon.transform.position.x ||
            startingPosition.x + 2 <= charCon.transform.position.x)
            {
                offset = new Vector3(2.2f, 2f, -10f);
            }
        }
        else if(speed.x < 0)
        {   
            if(startingPosition.x - 2 >= charCon.transform.position.x ||
            startingPosition.x + 2 <= charCon.transform.position.x)
            {
                offset = new Vector3(-2.2f, 2f, -10f);
            }
        }
        else
        {
            startingPosition = transform.position;
            offset = new Vector3(0f, 2f, -10f);
            if(Input.GetKey(KeyCode.LeftAlt) && charCon.isGrounded() && (!charCon.isMoving()))
            {
                offset = new Vector3(0f, -3.2f, -10f);
            }
            if(Input.GetKey(KeyCode.W) && charCon.isGrounded() && (!charCon.isMoving()))
            {
               offset = new Vector3(0f, 5.2f, -10f); 
            }
        }
        if(charCon.isWalled())
        {
            offset = new Vector3(0, offset.y, offset.z); 
            velocity = velocity + speed / 10;
            velocity.x = 0;
        }
        else velocity = velocity + speed / 10;
        targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void VerticalOffsetModification()
    {
        offset = new Vector3(offset.x, offset.y - 1, offset.z); 
    }*/
}

