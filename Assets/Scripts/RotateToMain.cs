using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMain : MonoBehaviour
{
    [SerializeField] private GameObject mainCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCam.transform.rotation;
    }
}
