using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendingArms : MonoBehaviour
{
    [SerializeField] private LayerMask ignore;
    public AbilityManager ab;
    public float armSpeed;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject fist;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject player;
    private bool fired = false;
    private bool canFire = true;
    private bool hitSomething = false;
    private bool retracting = false;
    private Vector3 startPos;
    private Vector3 goalPos;
    private float startTime;
    private float distance;
    private RaycastHit hit;

    private ThirdPersonMovement movement;
    private PartManagement jammoState;
    private Animator anim;
    private int shootHash;
    
    //Used for the Jammo's fist when fired
    [SerializeField]
    private GameObject MainCamera;
    [SerializeField]
    private GameObject ArmCamera;

    private float timer;

    [SerializeField] private AudioSource extendingArmSound;

    // Start is called before the first frame update
    void Start()
    {
        movement = this.transform.parent.GetComponent<ThirdPersonMovement>();
        jammoState = this.transform.parent.GetComponentInChildren<PartManagement>();
        shootHash = Animator.StringToHash("shoot");
    }

    // Update is called once per frame
    void Update()
    {
        if (ab.armCode != 2)
        {
            return;
        }
        //Only allows for player to shoot if Base Jammo or legless jammo are active
        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire && (jammoState.getJammoState(0).activeSelf || jammoState.getJammoState(2).activeSelf))
        {
            

            fired = true;
            startTime = Time.time;
            startPos = arm.transform.position;
            anim = movement.getAnimator();
            anim.SetBool(shootHash, true);
            //Debug.Log("GEtting input, start at " + startTime + " with position " + startPos);
            extendingArmSound.Play();
        }
    }

    private void FixedUpdate()
    {
        if (fired)
        {
            if (canFire && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 50f, ignore))
            {
                Debug.Log("hit " + hit.collider.gameObject.name);
                hitSomething = true;
                distance = Vector3.Distance(arm.transform.position, hit.point);
                canFire = false;
                Debug.Log("hit at " + hit.point);
                fired = false;
            } else
            {
                hitSomething = false;
                fired = false;
                canFire = true;
            }
        }
        if (hitSomething)
        {
            timer += Time.deltaTime;
            float currentDistance = (Time.time - startTime) * armSpeed;
            float fraction = currentDistance / distance;
            arm.transform.position = Vector3.Lerp(startPos, hit.point, fraction);
            movement.enabled = false;
            fist.SetActive(true);
            MainCamera.SetActive(false);
            ArmCamera.SetActive(true);
            if (Vector3.Distance(arm.transform.position, hit.point) < 0.00001f || timer > 5.0f)
            {
                hitSomething = false;
                retracting = true;
                startTime = Time.time;
                goalPos = startPos;
                startPos = hit.point;
            }
        }
        if (retracting)
        {
            float currentDistance = (Time.time - startTime) * armSpeed;
            float fraction = currentDistance / distance;
            arm.transform.position = Vector3.Lerp(startPos, goalPos, fraction);
            if (hit.collider.gameObject.layer == 25)
            {
                Debug.Log("hit object " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<TargetOpen>().OpenDoor();
            } else if (hit.collider.gameObject.layer == 30)
            {
                Debug.Log("hit object " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<HealthDamage>().TakeDamage(25f);
            }
            if (Vector3.Distance(arm.transform.position, goalPos) < 0.00001f || timer > 5.0f)
            {
                timer = 0f;
                movement.enabled = true;
                MainCamera.SetActive(true);
                ArmCamera.SetActive(false);
                fist.SetActive(false);
                retracting = false;
                canFire = true;
                arm.transform.position = player.transform.position + new Vector3(0.75f, -0.32f, 0f);
                anim.SetBool(shootHash, false);
            }
        }
    }

}
