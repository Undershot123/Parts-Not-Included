using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetOpen : MonoBehaviour
{

    [SerializeField] private GameObject door;

    [SerializeField] private AudioSource doorOpen; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        door.transform.position += new Vector3(0f, -10f, 0f);
        Debug.Log("moved " + door.name);
        doorOpen.Play();
    }

}
