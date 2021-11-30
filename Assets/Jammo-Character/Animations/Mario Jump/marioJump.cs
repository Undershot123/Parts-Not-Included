using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class marioJump : MonoBehaviour
{
    //Declare reference variables
    PlayerInput playerInput;
    CharacterController controller;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity; 

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;
    public float verticalVel;
    private Vector3 moveVector;

    //Copied from Jammo's original movement script
    public float Velocity;
    [Space]

	public Animator anim;


    //Variables to store optomized setter/getter parameter IDs
    int isJumpingHash;
    int canJumpHash;

    //Variables to store player input values
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private Vector3 appliedMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    
    //constants
    private float rotationFactorPerFrame = 15.0f;
    private float runMultiplier = 5.0f;
    private int zero = 0;

    //gravity variables
    private float gravity = -9.8f;
    private float groundedGravity = -.05f;

    // jumping variables
    private bool isJumpPressed = false;
    private float initialJumpVelocity;
    [SerializeField]
    private float maxJumpHeight = 1.5f;
    private float maxJumpTime = 0.75f;
    [SerializeField]
    private bool isJumping = false;
    private bool isJumpAnimating = false;
    
    
    void Awake()
    {
        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        isJumpingHash = Animator.StringToHash("isJumping");
        canJumpHash = Animator.StringToHash("canJump");

        playerInput.CharacterControls.Move.started += onMovementInput; //listens for when button is pressed
        playerInput.CharacterControls.Move.canceled += onMovementInput; //listens for when button is let go
        playerInput.CharacterControls.Move.performed += onMovementInput; //same purpose but for controllers

        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    // Update is called once per frame
    void Update()
    {

        //handleRotation();
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;


        controller.Move(appliedMovement * Time.deltaTime); //appliedMovement only affects Jammo in the y-axis

        handleGravity();
        handleJump();
        
    }
    

    void handleRotation()
    {
        Vector3 posToLookAt;

        posToLookAt.x = currentMovement.x;
        posToLookAt.y = 0.0f;
        posToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(posToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        //apply proper gravity if the player is grounded or not
        if(controller.isGrounded)
        {
            if(isJumpAnimating)
            {
                //set animator here
                anim.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }
            
            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f, -20.0f); //Mathf.Max() puts a cap on the deceleration speed
        }
        else{
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f; 

        }
    }

    void handleJump()
    {
        if(!isJumping && controller.isGrounded && isJumpPressed){
            //set animator here
            anim.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            
            isJumping = true;
            /* float previousYVelocity = currentMovement.y;
            float newYVelocity = (currentMovement.y + initialJumpVelocity);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;

            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;       */ 

            currentMovement.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;     
        } else if(!isJumpPressed && isJumping && controller.isGrounded){
            isJumping = false;
        }
    }

    public void changeJumpHeight(float newHeight)
    {
        maxJumpHeight = newHeight;
        setupJumpVariables();
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
