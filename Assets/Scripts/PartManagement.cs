using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class in in charge of disassembling jammo and changing the animation states based on which parts are removed
public class PartManagement : MonoBehaviour
{

    [SerializeField] private GameObject torso, arms, legs, pickupArms, pickupTorso, pickupLegs;

    private float torsotimer = 0f;
    private bool torsoCollide = false;

    private float armstimer = 0f;
    private bool armsCollide = false;

    private float legstimer = 0f;
    private bool legsCollide = false;

    private bool destroying;
    public AbilityManager ab;

    //Variables for controlling Jammo's states
    private GameObject[] jammoState = new GameObject[5];
    private ThirdPersonMovement movement;
    private CharacterController speedControl;
    private marioJump jumpControl;

    // Start is called before the first frame update
    void Start()
    {
        //Gets all of jammo's possible states
        for(int i = 0; i < 5; i++)
        {
            jammoState[i] = this.transform.parent.GetChild(i+1).gameObject;
            /*
                jammoState[0] = base Jammo
                jammoState[1] = armless Jammo, has legs and torso
                jammoState[2] = legless Jammo, has arms and torso
                jammoState[3] = limbless Jammo, has torso
                jammoState[4] = Jammo Head, no arms, legs, or torso
            */
        }

        //Gets the animator component from the parent object to set the animation states
        movement = jammoState[0].transform.parent.GetComponent<ThirdPersonMovement>();
        jumpControl = jammoState[0].transform.parent.GetComponent<marioJump>();

        //Sets the player's jump height based on current active state
        if(jammoState[0].activeSelf || jammoState[1].activeSelf){jumpControl.changeJumpHeight(1.5f);}
        else{jumpControl.changeJumpHeight(0f);}
    }

    // Update is called once per frame
    void Update()
    {
        //Remove all limbs including torso
        //Doesn't run if Jammo state is already jammo head
        if (Input.GetKeyDown(KeyCode.Alpha1) && torso.activeSelf && !jammoState[4].activeSelf)
        {
            //All states with arms remove jammo's arms
            if(jammoState[0].activeSelf || jammoState[2].activeSelf)
            {
                pickupArms.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.75f, torso.transform.position.z);
                arms.SetActive(!arms.activeSelf);
                armsCollide = false;
            }

            //All states with legs, remove jammmo's legs
            if(jammoState[0].activeSelf || jammoState[1].activeSelf)
            {
                pickupLegs.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.5f, torso.transform.position.z);
                legs.SetActive(!legs.activeSelf);
                legsCollide = false;
            }

            for(int i = 0; i < jammoState.Length-1; i++){jammoState[i].SetActive(false);} //Disables all states except head jammo
            pickupTorso.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.5f, torso.transform.position.z);
            torso.SetActive(false);
            torsoCollide = false;
            switchState(4); //Switch to Jammo Head state
        }

        //Remove legs
        //only runs if base jammo or armless jammo are active
        if (Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKey(KeyCode.Tab) && legs.activeSelf && (jammoState[0].activeSelf || jammoState[1].activeSelf))
        {
            //Possibilities
            //Player is using base jammo, switch to legless jammo
            if(jammoState[0].activeSelf)
            {
                jammoState[0].SetActive(false);
                switchState(2); //switch to Jammo state
            }
            //Player is using armless jammo, switch to limbless jammo
            else{
                jammoState[1].SetActive(false);
                switchState(3); //switch to Jammo state
            }

            //Drops "leg" so Jammo can pick it up
            pickupLegs.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.5f, torso.transform.position.z);
            legs.SetActive(!legs.activeSelf);
            legsCollide = false;
        }

        //Remove arms
        //only runs if base jammo or legless jammo are active
        if (Input.GetKeyDown(KeyCode.Alpha3) && !Input.GetKey(KeyCode.Tab) && arms.activeSelf && (jammoState[0].activeSelf || jammoState[2].activeSelf))
        {
            //Possibilities
            //Player is using base jammo, switch to armless jammo
            if(jammoState[0].activeSelf)
            {
                jammoState[0].SetActive(false);
                switchState(1);
            }
            //Player is using legless jammo, switch to limbless jammo
            else{
                jammoState[2].SetActive(false);
                switchState(3);
            }
            
            //Drops "arm" so jammo can pick it up
            pickupArms.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.75f, torso.transform.position.z);
            arms.SetActive(!arms.activeSelf);
            armsCollide = false;
        }

        //Adds a timer so Jammo doesn't pick up his body parts immediately
        if (!torso.activeSelf && !torsoCollide)
        {
            torsotimer += Time.deltaTime;
        }
        if (torsotimer >= 3.0f)
        {
            torsoCollide = true;
            pickupTorso.GetComponent<BoxCollider>().enabled = true;
            pickupTorso.GetComponent<BoxCollider>().isTrigger = true;
            torsotimer = 0f;
        }
        if (!legs.activeSelf && !legsCollide)
        {
            legstimer += Time.deltaTime;
        }
        if (legstimer >= 3.0f)
        {
            legsCollide = true;
            pickupLegs.GetComponent<BoxCollider>().enabled = true;
            pickupLegs.GetComponent<BoxCollider>().isTrigger = true;
            legstimer = 0f;
        }
        if (!arms.activeSelf && !armsCollide)
        {
            armstimer += Time.deltaTime;
        }
        if (armstimer >= 3.0f)
        {
            armsCollide = true;
            pickupArms.GetComponent<BoxCollider>().enabled = true;
            pickupArms.GetComponent<BoxCollider>().isTrigger = true;
            armstimer = 0f;
        }
        if (ab.armCode != 1)
        {
            destroying = false;
            return;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            destroying = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && destroying)
        {
            GameObject.Destroy(other.gameObject);
        }
        if (other.gameObject == pickupTorso)
        {
            //Possibilities
            //Jammo head is the only state in which this should run
            if(jammoState[4].activeSelf)
            {
                pickupTorso.transform.position = new Vector3(pickupTorso.transform.position.x, pickupTorso.transform.position.y - 5, pickupTorso.transform.position.z);
                pickupTorso.GetComponent<BoxCollider>().enabled = false;
                pickupTorso.GetComponent<BoxCollider>().isTrigger = false;
                torso.SetActive(true);

                jammoState[4].SetActive(false);
                switchState(3);
                Debug.Log("Picked up torso, turn to limbless jammo");
            }
            
        }
        if (other.gameObject == pickupLegs)
        {
            //Possibilities
            //Jammo head tries to pick up legs, cant do it because he needs his torso back
            if(jammoState[4].activeSelf)
            {
                Debug.Log("Jammo needs his torso first!!");
            }
            else{
                pickupLegs.transform.position = new Vector3(pickupTorso.transform.position.x, pickupTorso.transform.position.y - 5, pickupTorso.transform.position.z);
                pickupLegs.GetComponent<BoxCollider>().enabled = false;
                pickupLegs.GetComponent<BoxCollider>().isTrigger = false;
                legs.SetActive(true);

                //legless jammo picks up legs, turns into base jammo
                if(jammoState[2].activeSelf)
                {
                    jammoState[2].SetActive(false);
                    switchState(0);
                }

                //limbless jammo picks up legs, turns into armless jammo
                else if(jammoState[3].activeSelf)
                {
                    jammoState[3].SetActive(false);
                    switchState(1);
                }
            }
        }
        if (other.gameObject == pickupArms)
        {
            //Possibilities
            //Jammo head tries to pick up arms, can't pick it up without torso active
            if(jammoState[4].activeSelf)
            {
            }
            else{
                //Limbless jammo picks up arms, turns into legless jammo
                if(jammoState[3].activeSelf)
                {
                    jammoState[3].SetActive(false);
                    switchState(2);
                }
                //Armless jammo picks up arms, turns into base jammo
                if(jammoState[1].activeSelf)
                {
                    jammoState[1].SetActive(false);
                    switchState(0);
                }
                pickupArms.transform.position = new Vector3(pickupTorso.transform.position.x, pickupTorso.transform.position.y - 5, pickupTorso.transform.position.z);
                pickupArms.GetComponent<BoxCollider>().enabled = false;
                pickupArms.GetComponent<BoxCollider>().isTrigger = false;
                arms.SetActive(true);
            }
        }
    }

    //Helper method to help switch between jammo's different states and animations
    private void switchState(int state)
    {
        switch (state)
        {
            case 0:
                jammoState[0].SetActive(true); //Switches Jammo State to base Jammo
                movement.anim = jammoState[0].GetComponent<Animator>(); //Switches Animator Controller to base Jammo
                movement.speed = 4.0f;
                jumpControl.changeJumpHeight(1.5f);
                jumpControl.anim = jammoState[0].GetComponent<Animator>();
                break;

            case 1:
                jammoState[1].SetActive(true); //Switches Jammo State to armless Jammo
                movement.anim = jammoState[1].GetComponent<Animator>(); //Switches Animator Controller to armless Jammo
                movement.speed = 4.0f;
                jumpControl.changeJumpHeight(1.5f);
                jumpControl.anim = jammoState[1].GetComponent<Animator>();
                break;

            case 2:
                jammoState[2].SetActive(true); //Switches Jammo State to legless Jammo
                movement.anim = jammoState[2].GetComponent<Animator>(); //Switches Animator Controller to legless Jammo
                movement.speed = 2.0f; //Reduced movement speed
                jumpControl.changeJumpHeight(0f); //Reduced Jump Height
                jumpControl.anim = jammoState[2].GetComponent<Animator>();
                break;

            case 3:
                jammoState[3].SetActive(true); //Switches Jammo State to limbless Jammo
                movement.anim = jammoState[3].GetComponent<Animator>(); //Switches Animator Controller to limbless Jammo
                movement.speed = 2.0f; //Reduced movement speed
                jumpControl.changeJumpHeight(0f); //Can't jump
                jumpControl.anim = jammoState[3].GetComponent<Animator>();
                break;

            case 4:
                jammoState[4].SetActive(true); //Switches Jammo State to Jammo Head
                movement.anim = jammoState[4].GetComponent<Animator>(); //Switches Animator Controller to Jammo Head
                movement.speed = 2.0f; //Reduced movement speed
                jumpControl.changeJumpHeight(0f); //Can't jump
                jumpControl.anim = jammoState[4].GetComponent<Animator>();
                break;

            default:
                print("Invalid state");
                break;

        }
    }

}
