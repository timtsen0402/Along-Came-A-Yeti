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
        //玩家找到前
        if (!foxisFound) gameObject.transform.LookAt(player);

        //玩家找到後
        else
        {
            animator.SetBool("isFound", true);

            //追向玩家
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
            //停在玩家前
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
