using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmIK : MonoBehaviour
{
    //This script reassigns the position of Jammo's body to make him aim towards the object
    //for extending arm, needs to be a component of Player since it has access to all of the animators

    public Transform targetTransform; //Empty object that Jammo's arm extends towards
    public Transform aimTransform; //Raycast node found in Jammo's forearm
    public Transform bone;

    private ThirdPersonMovement movement;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        movement = this.GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = targetTransform.position;

        anim = movement.getAnimator();
        if(anim.GetBool("shoot"))
        {
            delay(1.0f);
            AimAtTarget(bone, targetPosition);
        }
        
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }

    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
