using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && torso.activeSelf)
        {
            pickupTorso.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.5f, torso.transform.position.z);
            torso.SetActive(false);
            torsoCollide = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKey(KeyCode.Tab) && legs.activeSelf)
        {
            pickupLegs.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.5f, torso.transform.position.z);
            legs.SetActive(!legs.activeSelf);
            legsCollide = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !Input.GetKey(KeyCode.Tab) && arms.activeSelf)
        {
            pickupArms.transform.position = new Vector3(torso.transform.position.x, torso.transform.position.y - 0.75f, torso.transform.position.z);
            arms.SetActive(!arms.activeSelf);
            armsCollide = false;
        }
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
            pickupTorso.transform.position = new Vector3(pickupTorso.transform.position.x, pickupTorso.transform.position.y - 5, pickupTorso.transform.position.z);
            pickupTorso.GetComponent<BoxCollider>().enabled = false;
            pickupTorso.GetComponent<BoxCollider>().isTrigger = false;
            torso.SetActive(true);
        }
        if (other.gameObject == pickupLegs)
        {
            pickupLegs.transform.position = new Vector3(pickupTorso.transform.position.x, pickupTorso.transform.position.y - 5, pickupTorso.transform.position.z);
            pickupLegs.GetComponent<BoxCollider>().enabled = false;
            pickupLegs.GetComponent<BoxCollider>().isTrigger = false;
            legs.SetActive(true);
        }
        if (other.gameObject == pickupArms)
        {
            pickupArms.transform.position = new Vector3(pickupTorso.transform.position.x, pickupTorso.transform.position.y - 5, pickupTorso.transform.position.z);
            pickupArms.GetComponent<BoxCollider>().enabled = false;
            pickupArms.GetComponent<BoxCollider>().isTrigger = false;
            arms.SetActive(true);
        }
    }

}
