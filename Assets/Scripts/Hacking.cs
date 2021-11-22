using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Hacking : MonoBehaviour
{
    public bool inMinigame = false;
    [SerializeField] private Camera minigameCam;
    [SerializeField] private Camera normalCam;
    [SerializeField] private ThirdPersonMovement move;
    [SerializeField] private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inMinigame && Input.GetKeyDown(KeyCode.Escape))
        {
            BackToGame(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            if (Input.GetKeyDown(KeyCode.E) && !inMinigame)
            {
                normalCam.enabled = false;
                minigameCam.enabled = true;
                move.enabled = false;
                inMinigame = true;
                // open hacking minigame
            }
        }
    }

    public void BackToGame(bool win)
    {
        if (win)
        {
            door.transform.position += new Vector3(0f, -10f, 0f);
        }
        minigameCam.enabled = false;
        normalCam.enabled = true;
        inMinigame = false;
        move.enabled = true;
    }

}
