using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
}

public class ArmIK : MonoBehaviour
{
    //This script reassigns the position of Jammo's body to make him aim towards the object
    //for extending arm, needs to be a component of Player since it has access to all of the animators

    public Transform targetTransform; //Empty object that Jammo's arm extends towards
    public Transform aimTransform; //Raycast node found in Jammo's forearm
    public Transform aimHead;

    private ThirdPersonMovement movement;
    private Animator anim;

    public int iterations = 10;
    [Range(0,1)]
    public float weight = 1.0f;

    public Transform Shoulderbone;
    public Transform Head;
    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        movement = this.GetComponent<ThirdPersonMovement>();
        anim = movement.getAnimator();

        /* boneTransforms = new Transform[humanBones.Length];
        for(int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = anim.GetBoneTransform(humanBones[i].bone);
        } */
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(aimTransform == null || targetTransform == null)
        {
            return;
        }
        Vector3 targetPosition = GetTargetPosition();
        

        anim = movement.getAnimator();
        for(int i = 0; i < iterations; i++)
        {
            if(anim.GetBool("shoot"))
            {
                //delay(1.0f);
                //Transform bone = boneTransforms[b];
                anim.SetBool("shoot", true);
            }
            
            AimAtTarget(Shoulderbone, targetPosition, weight); //Put back in if statement later
            AimAtTarget(Head, targetPosition, weight);
        }
    }

    //Checks to see if Jammo is looking in a direction of 360 degrees, does not allow player to fire if jammo's camera is facing -90 to 90 degrees
    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        
        if(targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if(targetDistance < distanceLimit) //target is too close
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}
