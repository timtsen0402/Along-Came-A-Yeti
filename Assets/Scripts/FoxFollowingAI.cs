using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using static GameCenter;

public class FoxFollowingAI : MonoBehaviour
{
    public static bool foxisFound = false;
    public float followingDistance = 20f;
    public Animator animator;
    void Update()
    {
        //���a���e
        if (!foxisFound) gameObject.transform.LookAt(player);

        //���a����
        else
        {
            animator.SetBool("isFound", true);

            //�l�V���a
            if ((transform.position - player.position).magnitude >= followingDistance)
            {
                //RUN
                if ((transform.position - player.position).magnitude >= 40f)
                {
                    animator.SetBool("tooFar", true);
                    agentFOX.speed = 30f;
                }
                else
                {
                    animator.SetBool("tooFar", false);
                    agentFOX.speed = 5f;
                }
                agentFOX.SetDestination(player.position);
                animator.SetBool("besidePlayer", false);

            }
            //���b���a�e
            else
            {
                animator.SetBool("besidePlayer", true);

                agentFOX.speed = 0f;
                agentFOX.SetDestination(transform.position); 
                gameObject.transform.LookAt(player);

            }
        }
    }
}
