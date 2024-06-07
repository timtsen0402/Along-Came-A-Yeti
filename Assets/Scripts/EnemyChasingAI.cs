using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static GameCenter;

public class EnemyChasingAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public Animator animator;

    //Patroling
    public Vector3 walkPoint;
    bool isWalkPointSet;
    public float walkPointRange;

    //States
    bool hasFoundPlayer = false;

    bool tempA = true;
    private void Start()
    {
        hasFoundPlayer = false;
        tempA = true;

    }
    void Update()
    {
        if (!isEnoughLog) sightRange = 200f;
        else sightRange = 500f;

        //非追擊範圍
        if (!isMonsterInSightRange)
        {
            //若曾發現過玩家將巡查
            if (hasFoundPlayer)
                Patroling();

            animator.SetBool("IsInSight", false);
            FindObjectOfType<AudioManager>().Enable("chasing", false);

        }

        //追擊範圍
        else
        {
            hasFoundPlayer = true;

            if ((transform.position - player.position).magnitude >= 15f)
            {
                animator.SetBool("IsWalkingPointSet", false);
                animator.SetBool("IsInSight", true);
                ChasePlayer();
            }
            else
            {
                KillPlayer();
            }
            if(!isOver)
                FindObjectOfType<AudioManager>().Enable("chasing", true);
            else
                FindObjectOfType<AudioManager>().Enable("chasing", false);

        }
    }
    void Patroling()
    {
        agent.speed = 5f;

        if (!isWalkPointSet)
            SearchWalkPoint();
        if (isWalkPointSet)
        {
            animator.SetBool("IsWalkingPointSet", true);
            agent.SetDestination(walkPoint);
        }
        if ((walkPoint - transform.position).magnitude < 3f)
            isWalkPointSet = false;
    }
    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            isWalkPointSet = true;

    }
    void ChasePlayer()
    {
        if (isEnoughLog)
            agent.speed = 30f;
        else
            agent.speed = 20f;
        agent.SetDestination(player.position);
    }
    void KillPlayer()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("Attack", true);
        ShowSomethingSlowly(catchedIMG, Time.deltaTime * .2f);
        ShowSomethingSlowly(catchedBTN.GetComponent<Image>(), Time.deltaTime * .2f);
        if (tempA && !isOver)
        {
            FindObjectOfType<AudioManager>().Play("catched");
            tempA = false;
        }
        isOver = true;
    }
}
