using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movementController : MonoBehaviour
{
    //Declare reference variables
    PlayerInput playerInput;
    CharacterController controller;
    public Transform camera;

    public float speed = 4f;

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

	public float InputX;
	public float InputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;

    //Variables to store optomized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int canJumpHash;

    //Variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isRunPressed;
    
    //constants
    float rotationFactorPerFrame = 15.0f;
    float runMultiplier = 5.0f;
    int zero = 0;

    //gravity variables
    float gravity = -9.8f;
    float groundedGravity = -.05f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 4.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isJumpAnimating = false;
    
    
    void Awake()
    {
        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
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
        /* Debug.Log("isJumping = " + isJumping);
        Debug.Log("isGrounded = " + controller.isGrounded);
        Debug.Log("isJumpPressed = " + isJumpPressed); */

        //handleRotation();
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            
        }
        InputMagnitude();

        appliedMovement.x = currentMovement.x;
        appliedMovement.z = currentMovement.z;
        controller.Move(appliedMovement * Time.deltaTime); //removing this makes Jammo jump correctly but it drifts on the X axis

        /* var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right; */

        handleGravity();
        handleJump();
        
    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }
    void InputMagnitude() {
        //Calculate Input Vectors
        InputX = Input.GetAxis ("Horizontal");
        InputZ = Input.GetAxis ("Vertical");

        //anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
        //anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

        //Calculate the Input Magnitude
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //Physically move player

        if (Speed > allowPlayerRotation) {
            anim.SetFloat ("Blend", 2.0f, StartAnimTime, Time.deltaTime);
            PlayerMoveAndRotation ();
        } else if (Speed < allowPlayerRotation) {
            anim.SetFloat ("Blend", 0f, StopAnimTime, Time.deltaTime);
        }
    }

    void PlayerMoveAndRotation() {
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
		}
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

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
