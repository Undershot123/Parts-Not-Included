using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlower : MonoBehaviour
{
    private bool blowing;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            blowing = true;
            Debug.Log("blowing");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("dete with obj " + other.gameObject.name);
        if (other.gameObject.layer == 8 && blowing)
        {
            Debug.Log("woo");
        }
    }

}
