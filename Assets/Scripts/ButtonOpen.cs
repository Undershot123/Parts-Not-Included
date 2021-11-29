using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpen : MonoBehaviour
{
    [SerializeField] private GameObject door;
    public bool closed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (closed && other.gameObject.layer == 20)
        {
            door.transform.position += new Vector3(0f, -10f, 0f);
            closed = false;
        }
    }

}
