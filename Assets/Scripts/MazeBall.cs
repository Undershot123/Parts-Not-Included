using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBall : MonoBehaviour
{
    [SerializeField] private Hacking hacking;
    [SerializeField] private GameObject background;
    [SerializeField] private int thisCode;
    private Rigidbody rb;
    private Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (hacking.inMinigame && hacking.minigameCode == thisCode)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.AddForce(background.transform.forward * 10f, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.AddForce(background.transform.forward * -10f, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.AddForce(background.transform.up * 10f, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.AddForce(background.transform.up * -10f, ForceMode.Impulse);
            }
        } else
        {
            transform.position = startingPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 15)
        {
            hacking.BackToGame(true);
            // win
        }
        else if (collision.gameObject.layer == 16)
        {
            transform.position = startingPos;
            // reset
        }
    }

}
