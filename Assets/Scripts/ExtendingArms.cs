using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendingArms : MonoBehaviour
{
    [SerializeField] private LayerMask ignore;
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
    private Transform Shoulderbone;

    [SerializeField] private AudioSource extendingArmSound;

    // Start is called before the first frame update
    void Start()
    {
        movement = this.transform.parent.GetComponent<ThirdPersonMovement>();
        shootHash = Animator.StringToHash("shoot");
        //Shoulderbone = this.transform.Find("Jammo_Player/Armature.001/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm");
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
            float currentDistance = (Time.time - startTime) * armSpeed;
            float fraction = currentDistance / distance;
            arm.transform.position = Vector3.Lerp(startPos, hit.point, fraction);
            //Shoulderbone.transform.position = Vector3.Lerp(startPos, hit.point, fraction);
            if (Vector3.Distance(arm.transform.position, hit.point) < 0.00001f)
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
