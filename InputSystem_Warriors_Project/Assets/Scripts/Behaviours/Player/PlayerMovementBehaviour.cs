using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody playerRigidbody;

    [Header("Movement Settings")]
    public float movementSpeed = 3f;
    public float turnSpeed = 0.1f;
    public float JumpIntensity = 10;
    [SerializeField] float maxJumpHeight = 4.0f;
    [SerializeField] float jumpPeakTime = 0.4f;
    protected Vector2 currentVelocity;
    float Gravity { get { return maxJumpHeight * 2 / (jumpPeakTime * jumpPeakTime); } }
    public float JumpSpeed { get { return Gravity * jumpPeakTime; } }
    private bool canjump = true;
    //Stored Values
    private Camera mainCamera;
    private Vector3 movementDirection;

    public void SetupBehaviour()
    {
        SetGameplayCamera();
    }

    void SetGameplayCamera()
    {
        mainCamera = CameraManager.Instance.GetGameplayCamera();
    }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        movementDirection = newMovementDirection;
    }

    void FixedUpdate()
    {
        MoveThePlayer();
        TurnThePlayer();
        ApplyGravity();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }



    }

    void MoveThePlayer()
    {
        Vector3 movement = CameraDirection(movementDirection) * movementSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }
    
    void TurnThePlayer()
    {
        if(movementDirection.sqrMagnitude > 0.01f)
        {

             Quaternion rotation = Quaternion.Slerp(playerRigidbody.rotation,
                                                  Quaternion.LookRotation (CameraDirection(movementDirection)),
                                                  turnSpeed);

            playerRigidbody.MoveRotation(rotation);

        }
    }


    Vector3 CameraDirection(Vector3 movementDirection)
    {
        var cameraForward = mainCamera.transform.forward;
        var cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        
        return cameraForward * movementDirection.z + cameraRight * movementDirection.x; 
   
    }
    void ApplyGravity()
    {
        currentVelocity.y -= Gravity * Time.fixedDeltaTime;
    }

    void Jump()
    {
        playerRigidbody.AddForce(new Vector3(0, JumpIntensity, 0), ForceMode.Impulse);
        //currentVelocity.y = JumpSpeed;
    }
}
