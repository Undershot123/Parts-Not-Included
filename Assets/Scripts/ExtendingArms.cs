using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendingArms : MonoBehaviour
{
    public AbilityManager ab;
    public float armSpeed;
    [SerializeField] private GameObject arm;
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
    private Animator anim;
    private int shootHash;

    // Start is called before the first frame update
    void Start()
    {
        movement = this.transform.parent.GetComponent<ThirdPersonMovement>();
        shootHash = Animator.StringToHash("shoot");
    }

    // Update is called once per frame
    void Update()
    {
        if (ab.armCode != 2)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire)
        {
            fired = true;
            startTime = Time.time;
            startPos = arm.transform.position;
            anim = movement.getAnimator();
            anim.SetBool(shootHash, true);
            Debug.Log("Shoot: "+ shootHash);
            //Debug.Log("GEtting input, start at " + startTime + " with position " + startPos);
        }
    }

    private void FixedUpdate()
    {
        if (fired)
        {
            int layerMask = 1 << 2;
            layerMask = ~layerMask;
            if (canFire && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 50f, layerMask))
            {
                Debug.Log("hit " + hit.collider.gameObject.name);
                hitSomething = true;
                distance = Vector3.Distance(arm.transform.position, hit.transform.position);
                canFire = false;
                Debug.Log("hit at " + hit.transform.position);
                Debug.DrawLine(arm.transform.position, hit.transform.position);
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
            float currentDistance = (Time.time - startTime) * armSpeed;
            float fraction = currentDistance / distance;
            arm.transform.position = Vector3.Lerp(startPos, hit.transform.position, fraction);
            if (Vector3.Distance(arm.transform.position, hit.transform.position) < 0.00001f)
            {
                hitSomething = false;
                retracting = true;
                startTime = Time.time;
                goalPos = startPos;
                startPos = hit.transform.position;
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
            if (Vector3.Distance(arm.transform.position, goalPos) < 0.00001f)
            {
                retracting = false;
                canFire = true;
                arm.transform.position = player.transform.position + new Vector3(0.75f, -0.32f, 0f);
                anim.SetBool(shootHash, false);
            }
        }
    }

}
