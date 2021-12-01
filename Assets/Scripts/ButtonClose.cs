using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClose : MonoBehaviour
{
    [SerializeField] private GameObject door;
    public bool open;

    public AudioClip doorAudio;

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
        if (open && other.gameObject.layer == 20)
        {
            AudioSource.PlayClipAtPoint(doorAudio, transform.position);
            door.transform.position += new Vector3(0f, 10f, 0f);
            open = false;
        }
    }

}
