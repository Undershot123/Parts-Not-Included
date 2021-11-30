using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
            }
        }
    }

}
