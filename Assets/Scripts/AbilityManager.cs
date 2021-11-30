using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public int armCode, legCode;
    public ThirdPersonMovement move;

    private GameObject[] jammoState = new GameObject[5];
    private PartManagement jammoUpdate;

    // Start is called before the first frame update
    void Start()
    {
        armCode = 1;
        legCode = 1;
        
        //Object used to check what Jammo state is currently active
        jammoUpdate = this.GetComponentInChildren<PartManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.Tab))
        { // legs
            legCode++;
            if (legCode == 3)
            {
                legCode = 1;
            }

            //Only runs if Base Jammo or Armless Jammo are active
            if(jammoUpdate.isJammoStateActive(0) || jammoUpdate.isJammoStateActive(1))
            {
                SwitchUpgrade("legs", legCode);
            }
            else{
                Debug.Log("You have no legs!!");
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.Tab))
        { //arms
            armCode++;
            if (armCode == 3)
            {
                armCode = 1;
            }

            //Only runs if Base Jammo or Legless Jammo are active
            if(jammoUpdate.isJammoStateActive(0) || jammoUpdate.isJammoStateActive(2))
            {
                SwitchUpgrade("arms", armCode);
            }
            else{
                Debug.Log("You have no arms!!");
            }
            
        } 
        /* if (Input.GetKey(KeyCode.Mouse0) && armCode == 3)
        {
            move.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            move.enabled = true;
        } */
    }

    public void SwitchUpgrade(string bodyPart, int code)
    {
        if (bodyPart == "arms")
        {
            switch (code)
            {
                case 1:
                    Debug.Log("Strong Arms Equipped");
                    move.enabled = true;
                    break;
                case 2:
                    Debug.Log("Extending Arms Equipped");
                    move.enabled = true;
                    break;
                default:
                    break; //nothing happens
            }
        } else if (bodyPart == "legs")
        {
            switch (code)
            {
                case 1:
                    Debug.Log("Speed Legs Equipped");
                    move.changeMovementSpeed(8.0f);
                    break;
                case 2:
                    Debug.Log("Super Jump Legs Equipped");
                    break;
                default:
                    break; //nothing happens
            }
        } else
        {
            Debug.Log("Applying change to nonexistent body part");
        }
    }

}
