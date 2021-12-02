using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmIK : MonoBehaviour
{
    public Transform targetTransform; //Empty object that Jammo's arm extends towards
    public Transform aimTransform; //Raycast node
    public Transform bone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = targetTransform.position;
        AimAtTarget(bone, targetPosition);
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }
}
