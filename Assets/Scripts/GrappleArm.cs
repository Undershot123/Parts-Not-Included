using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleArm : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 target;
    private bool grappling;
    private SpringJoint springJoint;
    [SerializeField] private AbilityManager ab;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask canGrappleTo;
    [SerializeField] private Transform FPScam;
    [SerializeField] private Transform arm;
    [SerializeField] private GameObject player;

    [SerializeField] private float springValue;
    [SerializeField] private float damperValue;
    [SerializeField] private float massScaleValue;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ab.armCode != 4)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            grappling = true;
            Grapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            grappling = false;
            lineRenderer.positionCount = 0;
            Destroy(springJoint);
        }
        if (springJoint)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    private void Grapple()
    {
        RaycastHit point;
        if (Physics.Raycast(arm.position, FPScam.forward, out point, maxDistance, canGrappleTo))
        {
            target = point.point;
            springJoint = player.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = target;
            float distance = Vector3.Distance(player.transform.position, target);

            springJoint.maxDistance = distance * 0.7f;
            springJoint.minDistance = distance * 0.3f;

            springJoint.spring = springValue;
            springJoint.damper = damperValue;
            springJoint.massScale = massScaleValue;

        }

    }

    private void ReleaseGrapple()
    {

    }

}
