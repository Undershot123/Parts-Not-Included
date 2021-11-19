using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform camera;

    public float speed = 4f;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity; 

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
	//public CharacterController controller;
	public bool isGrounded;

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

	private int canJumpHash;
	private int isJumpingHash;
	[SerializeField]
	private float timer = 0f;
	[SerializeField]
	private bool canJump = true;

	//gravity variables
    float gravity = -9.8f;
    float groundedGravity = -.05f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 2.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
	Vector3 appliedMovement;

    //Copied from Jammo's original movement script
    void Start () {
		anim = GetComponentInChildren<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
		canJumpHash = Animator.StringToHash("canJump");
		isJumpingHash = Animator.StringToHash("isJumpingHash");
		setupJumpVariables();
	}

    // Update is called once per frame
    void Update()
    {
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

        InputMagnitude ();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel = 0f;
        }
        else
        {
            verticalVel -= 1f;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);

		if (!canJump)
		{
			//timer += Time.deltaTime;
			if(isGrounded/*timer > 1.0f*/)
			{
				anim.SetBool(canJumpHash, true);
				canJump = true;
				//timer = 0f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(canJump)
			{
				verticalVel = initialJumpVelocity;
				anim.SetBool(canJumpHash, false);
				canJump = false;
			}
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

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
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

	void handleGravity()
    {
        bool isFalling = verticalVel < 0;
        float fallMultiplier = 2.0f;

        //apply proper gravity if the player is grounded or not
        if(controller.isGrounded)
        {
            /* if(isJumpAnimating)
            {
                //set animator here
                anim.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            } */
            

            verticalVel = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYVelocity = moveVector.y;
            moveVector.y = moveVector.y + (gravity * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + moveVector.y) * .5f, -20.0f); //Mathf.Max() puts a cap on the deceleration speed
            verticalVel = appliedMovement.y;
        }
        else{
            float previousYVelocity = moveVector.y;
            moveVector.y = moveVector.y + (gravity * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + moveVector.y) * .5f; 

            verticalVel = appliedMovement.y;
        }
    }

	 void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
}
