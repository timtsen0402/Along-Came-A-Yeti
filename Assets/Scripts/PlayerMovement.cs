using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameCenter;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 15f;
    public float gravity = -9.8f;

    Vector3 velocity;
    float x, z;

    void Update()
    {
        x = 0;
        z = 0;
        if (!isOver)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        WalkRun(15f, 30f);
    }
    void WalkRun(float walkSpeed, float runSpeed)
    {
        //Idle
        if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && 
             !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))|| isOver)
        {
            FindObjectOfType<AudioManager>().Enable("runStep", false);
            FindObjectOfType<AudioManager>().Enable("walkStep", false);
            return;
        }
        
        //run
        if(Input.GetKey(KeyCode.LeftShift) && energy >= 0f)
        {
            speed = runSpeed;
            FindObjectOfType<AudioManager>().Enable("runStep", true);
            FindObjectOfType<AudioManager>().Enable("walkStep", false);
            //get protein
            if (energy == 101f)
                return;
            energy -= .05f;
        }
        //walk
        else
        {
            speed = walkSpeed;
            FindObjectOfType<AudioManager>().Enable("runStep", false);
            FindObjectOfType<AudioManager>().Enable("walkStep", true);

        }

    }
}
