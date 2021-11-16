using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    public float jumpHeight;
    private bool jump;
    private bool fall;
    private bool canJump;
    private float speed;
    private float startTime;
    private float startPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            canJump = false;
            jump = true;
            startTime = Time.time;
            startPos = transform.position.y;
        }
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            float currentDistance = (Time.time - startTime) * speed;
            float fraction = currentDistance / jumpHeight;
            float newY = Mathf.Lerp(startPos, startPos + jumpHeight, fraction);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, startPos + jumpHeight, transform.position.z)) < 0.0001f)
            {
                jump = false;
                fall = true;
            }
        }
        if (fall)
        {
            
        }
    }
}
