using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private int armCode, legCode;

    // Start is called before the first frame update
    void Start()
    {
        armCode = 1;
        legCode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.Tab))
        { // legs
            legCode++;
            if (legCode == 4)
            {
                legCode = 1;
            }
            SwitchUpgrade("legs", legCode);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.Tab))
        {
            armCode++;
            if (armCode == 6)
            {
                armCode = 1;
            }
            SwitchUpgrade("arms", armCode);
        } 
    }

    public void SwitchUpgrade(string bodyPart, int code)
    {
        if (bodyPart == "arms")
        {
            switch (code)
            {
                case 1:
                    Debug.Log("Strong Arms Equipped");
                    break;
                case 2:
                    Debug.Log("Extending Arms Equipped");
                    break;
                case 3:
                    Debug.Log("Anchor Arms Equipped");
                    break;
                case 4:
                    Debug.Log("Grappling Arms Equipped");
                    break;
                case 5:
                    Debug.Log("Windblowing Arms Equipped");
                    break;
            }
        } else if (bodyPart == "legs")
        {
            switch (code)
            {
                case 1:
                    Debug.Log("Speed Legs Equipped");
                    break;
                case 2:
                    Debug.Log("Super Jump Legs Equipped");
                    break;
                case 3:
                    Debug.Log("Sturdy Legs Equipped");
                    break;
            }
        } else
        {
            Debug.Log("Applying change to nonexistent body part");
        }
    }

}
