using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendingArms : MonoBehaviour
{
    public AbilityManager ab;
    public float armSpeed;
    [SerializeField] private GameObject arm;
    private bool fired = false;
    private bool canFire = true;
    private bool hitSomething = false;
    private bool retracting = false;
    private Vector3 startPos;
    private Vector3 goalPos;
    private float startTime;
    private float distance;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
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
            Debug.Log("GEtting input, start at " + startTime + " with position " + startPos);
        }
    }

    private void FixedUpdate()
    {
        if (fired)
        {
            if (canFire && Physics.Raycast(arm.transform.position, arm.transform.forward, out hit, 50f))
            {
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
            if (Vector3.Distance(arm.transform.position, goalPos) < 0.00001f)
            {
                retracting = false;
                canFire = true;
            }
        }
    }

}
