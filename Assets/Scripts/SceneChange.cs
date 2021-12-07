using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private AudioSource endingTheme;

    [SerializeField] private AudioSource elevatorMusic;

    [SerializeField] private AudioSource BackgroundMusic;

    [SerializeField] private AudioSource walkUp;

    private float timer;
    private bool endGame;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream

        if(elevatorMusic != null) elevatorMusic.Play();
        if(BackgroundMusic != null) BackgroundMusic.Play();
=======
        timer = 0f;
        endGame = false;
        elevatorMusic.Play();
        BackgroundMusic.Play();
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame == true)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 43.2f)
        {
            SceneManager.LoadScene("EndGame");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            Debug.Log("collided and current scene is " + SceneManager.GetActiveScene().name);
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level 1":
                    SceneManager.LoadScene("Level 2");
                    break;
                case "Level 2":
                    SceneManager.LoadScene("Level 3");
                    break;
                case "Level 3":
                    SceneManager.LoadScene("Level 4");
                    break;
                case "Level 4":
                    endGame = true;
                    endingTheme.Play();
                    BackgroundMusic.Stop();
                    walkUp.Stop();
                    break;
            }
        }
          if (other.gameObject.layer == 13)
        {
            //Plays music on long walkup to finale
                 walkUp.Play();
                 BackgroundMusic.Stop();
        }
    }

}
