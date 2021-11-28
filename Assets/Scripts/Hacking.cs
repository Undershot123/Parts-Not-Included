using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Hacking : MonoBehaviour
{
    public bool inMinigame = false;
    public int minigameCode = 0;
    [SerializeField] private Camera minigameCam1, minigameCam2, minigameCam3, minigameCam4, minigameCam5, minigameCam6;
    [SerializeField] private Camera normalCam;
    [SerializeField] private ThirdPersonMovement move;
    [SerializeField] private GameObject door1, door2, door3, door4, door5, door6;
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
                switch (other.gameObject.tag)
                {
                    case "Terminal1":
                        minigameCam1.enabled = true;
                        minigameCode = 1;
                        break;
                    case "Terminal2":
                        minigameCam2.enabled = true;
                        minigameCode = 2;
                        break;
                    case "Terminal3":
                        minigameCam3.enabled = true;
                        minigameCode = 3;
                        break;
                    case "Terminal4":
                        minigameCam4.enabled = true;
                        minigameCode = 4;
                        break;
                    case "Terminal5":
                        minigameCam5.enabled = true;
                        minigameCode = 5;
                        break;
                    case "Terminal6":
                        minigameCam6.enabled = true;
                        minigameCode = 6;
                        break;
                }
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
            switch (minigameCode)
            {
                case 1:
                    door1.transform.position += new Vector3(0f, -10f, 0f);
                    break;
                case 2:
                    door2.transform.position += new Vector3(0f, -10f, 0f);
                    break;
                case 3:
                    door3.transform.position += new Vector3(0f, -10f, 0f);
                    break;
                case 4:
                    door4.transform.position += new Vector3(0f, -10f, 0f);
                    break;
                case 5:
                    door5.transform.position += new Vector3(0f, -10f, 0f);
                    break;
                case 6:
                    door6.transform.position += new Vector3(0f, -10f, 0f);
                    break;
            }
        }
        minigameCam1.enabled = false;
        minigameCam2.enabled = false;
        minigameCam3.enabled = false;
        minigameCam4.enabled = false;
        minigameCam5.enabled = false;
        normalCam.enabled = true;
        inMinigame = false;
        move.enabled = true;
    }

}