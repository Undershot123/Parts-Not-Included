using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField]
    private Transform camera;

    private float speed = 4f;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity; 

    //Copied from Jammo's original movement script
    [SerializeField]
    private float Velocity;
    [Space]

	private float InputX;
	private float InputZ;
	private Vector3 desiredMoveDirection;
	private bool blockRotationPlayer;
	private float desiredRotationSpeed = 0.1f;
    [SerializeField]
	private Animator anim;
	private float Speed;
	private float allowPlayerRotation = 0.1f;
	public Camera cam;
	//public CharacterController controller;
	private bool isGrounded;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    private float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    private float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    private float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    private float StopAnimTime = 0.15f;

    private float verticalVel = 0;
    private Vector3 moveVector;

    //Copied from Jammo's original movement script
    void Start () {
		anim = GetComponentInChildren<Animator> ();
		//cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
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
        controller.Move(moveVector);
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

    public void changeMovementSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void changeAnimator(Animator newAnimator)
    {
        anim = newAnimator;
    }
}
