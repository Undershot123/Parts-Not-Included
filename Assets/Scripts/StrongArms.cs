using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongArms : MonoBehaviour
{

    private AbilityManager ab;
    private ThirdPersonMovement movement;
    private BoxCollider hitbox;
    private int isPunchingHash;

    private float timer;
    private bool canPunch;
    // Start is called before the first frame update
    void Start()
    {
        ab = this.transform.parent.GetComponent<AbilityManager>();
        movement = this.transform.parent.GetComponent<ThirdPersonMovement>();
        isPunchingHash = Animator.StringToHash("isPunching");
        hitbox = GetComponent<BoxCollider>();

        timer = 0f;
        canPunch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ab.armCode != 1)
        {
            return;
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && canPunch)
        {
            movement.getAnimator().SetBool(isPunchingHash, true);
            canPunch = false;
            hitbox.enabled = true;
            Debug.Log("Punch");
        }

        if(!canPunch)
        {
            timer += Time.deltaTime;
            if(timer > .8f)
            {
                movement.getAnimator().SetBool(isPunchingHash, false);
                canPunch = true;
                hitbox.enabled = false;
                timer = 0f;
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == 30)
        {
            collision.gameObject.GetComponent<HealthDamage>().TakeDamage(25f);
        }
    }

}
