using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // reference to player camera, need to move camera into it
    public Camera playerCamera;

    //where to keep track of our states
    //public Text StateTracker;

    // walk speed
    public float walkSpeed = 6f;

    // run speed 
    public float runSpeed = 9f;

    // force applied when jumping
    public float jumpPower = 1f;
    public float wallJumpPower = 2f;

    //force of gravity
    public float gravity = 9.81f;

    //how fast the camera snaps with the mouse
    public float lookSpeed = 2f;

    // look up and down limit
    public float lookXLimit = 90f;

    // height of character when not crouched
    public float defaultHeight = 2f;

   // character height crouched 
    public float crouchHeight = 1f;

    // crouched movement speed
    public float crouchSpeed = 3f;


    // keeps track of movement direction using a vector
    private Vector3 moveDirection = Vector3.zero;

    // tracks the rotation of camera
    private float rotationX = 0;

    // refers ti the character controller component
    private CharacterController characterController;

    // is the player allowed to move?s
    private bool canMove = true;
    //is the player crouched
    private bool isRunning = false;

    private bool isADS = false;
    public int ADSmovementSpeed;

    public enum MovementState{IDLE, WALKING, RUNNING, JUMPING, FALLING, CROUCHING }

    private MovementState currentState;

    //refer to the gun object so we can move it later
    public Transform Glock;

    private Vector3 originalPosition;

    public Vector3 ADSposition = new Vector3(0f,1f,0f);

    // starts when the scene starts
    void Start()
    {
        //moves gun to default position
        originalPosition = Glock.localPosition;
        // initializes the character controller
        characterController = GetComponent<CharacterController>();
        
        //locks and hides the cursor in the middle of the screen 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentState = MovementState.IDLE;
    }

   void Update()
    {
        //StateTracker.text = currentState.ToString();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //check to see if player is aiming down sights
        Input.GetMouseButton(0);
        if (Input.GetMouseButton(0))
        {
            if(!isRunning)
            {
                isADS = true;
                Glock.localPosition = ADSposition;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isADS = false;
            Glock.localPosition = originalPosition;
        }

        // Checks to see if player is running or not
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isADS) // Only allow running if not ADS
            {
                isRunning = true;
                currentState = MovementState.RUNNING;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        // Calculate movement speed based on running or walking
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        // Combine the forward and right movement
        Vector3 horizontalMove = (forward * curSpeedX) + (right * curSpeedY);

        // Handle jumping and gravity
        if (characterController.isGrounded)
        {
            //Player cannot jump from crouched
            if (Input.GetButton("Jump") && canMove && currentState != MovementState.CROUCHING)
            {
                moveDirection.y = jumpPower; // Apply jump force
                currentState = MovementState.JUMPING;
            }
            else
            {
                moveDirection.y = -1f; // Ensure character stays grounded
                if (curSpeedX == 0 && curSpeedY == 0)
                {
                    currentState = MovementState.IDLE;
                }
                else
                {
                    currentState = isRunning ? MovementState.RUNNING : MovementState.WALKING;
                }
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime; // Apply gravity when not grounded
            if (moveDirection.y < 0)
            {
                currentState = MovementState.FALLING;
            }
        }

        // Combine horizontal and vertical movements
        moveDirection.x = horizontalMove.x;
        moveDirection.z = horizontalMove.z;


        //crouch mechanic is we only want to be crouched when holding the button
        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
            currentState = MovementState.CROUCHING;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 9f; // Ensure it matches your desired default run speed
        }

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            // Vertical clamps
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Vertical rotation of camera
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Applies horizontal rotation
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    public MovementState GetCurrentState()
    {
        return currentState;
    }

}