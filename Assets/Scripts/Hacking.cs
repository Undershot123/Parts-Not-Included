using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Cinemachine;

public class Hacking : MonoBehaviour
{
    public bool inMinigame = false;
    public int minigameCode = 0;
    [SerializeField] private Camera minigameCam1, minigameCam2, minigameCam3, minigameCam4, minigameCam5, minigameCam6, minigameCam7;
    [SerializeField] private Camera normalCam;
    [SerializeField] private ThirdPersonMovement move;
    [SerializeField] private GameObject door1, door2, door3, door4, door4_2, door4_3, door4_4, door4_5, door4_6, door4_7, door5, door5_2, door6, door6_2, door6_3, door7;

    [SerializeField] private AudioSource doorOpen, hackingStart;

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
                hackingStart.Play();
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
                    case "Terminal7":
                        minigameCam7.enabled = true;
                        minigameCode = 7;
                        break;
                }
                if (minigameCode != 0)
                {
                    normalCam.enabled = false;
                    move.enabled = false;
                    inMinigame = true;
                }
                // open hacking minigame
            }
        }
    }

    public void BackToGame(bool win)
    {
        if (win)
        {
            doorOpen.Play();
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
                    door4_2.transform.position += new Vector3(0f, -10f, 0f);
                    if (SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 6)
                    {
                        door4_3.transform.position += new Vector3(0f, -10f, 0f);
                    }
                    if (SceneManager.GetActiveScene().buildIndex == 2)
                    {
                        door4_4.transform.position += new Vector3(0f, -10f, 0f);
                        door4_5.transform.position += new Vector3(0f, -10f, 0f);
                        door4_6.transform.position += new Vector3(0f, -10f, 0f);
                        door4_7.transform.position += new Vector3(0f, -10f, 0f);
                    }
                    break;
                case 5:
                    door5.transform.position += new Vector3(0f, -10f, 0f);
                    if (SceneManager.GetActiveScene().buildIndex == 3)
                    {
                        door5_2.transform.position += new Vector3(0f, -10f, 0f);
                    }
                    break;
                case 6:
                    door6.transform.position += new Vector3(0f, -10f, 0f);
                    if (SceneManager.GetActiveScene().buildIndex == 3)
                    {
                        door6_2.transform.position += new Vector3(0f, -10f, 0f);
                        door6_3.transform.position += new Vector3(0f, -10f, 0f);
                    }
                    break;
                case 7:
                    door7.transform.position += new Vector3(0f, -10f, 0f);
                    break;
            }
        }
        minigameCam1.enabled = false;
        minigameCam2.enabled = false;
        minigameCam3.enabled = false;
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 6)
        {
            minigameCam4.enabled = false;
            minigameCam5.enabled = false;
            minigameCam6.enabled = false;
        }
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            minigameCam7.enabled = false;
        }

        normalCam.enabled = true;
        inMinigame = false;
        move.enabled = true;
    }

}
