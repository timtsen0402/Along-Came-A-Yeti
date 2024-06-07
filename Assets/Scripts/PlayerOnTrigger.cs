using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoxFollowingAI;
using static GameCenter;

public class PlayerOnTrigger : MonoBehaviour
{
    public LayerMask whatIsMonster;
    Color lifeColorOrigin;
    bool isCatched = false;
    float timeA = 0f;
    private void Start()
    {
        lifeColorOrigin = LifeFillIMG.color;
        isCatched = false;
    }
    private void Update()
    {
        isMonsterInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsMonster);

        if (LifeFillIMG.color == Color.red) Invoke(nameof(TurnLifeBarBack), .22f);

        if (Time.time > timeA + 1f)
            isCatched = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCatched)
        {
            if (other.gameObject.CompareTag("Branch"))
            {
                isCatched = true;
                timeA = Time.time;
                remainingTime += 40f;

                Destroy(other.gameObject.transform.parent.gameObject);
                FindObjectOfType<AudioManager>().Play("get");
            }
            if (other.gameObject.CompareTag("Log"))
            {
                isCatched = true;
                timeA = Time.time;
                numberLog++;

                Destroy(other.gameObject.transform.parent.gameObject);
                FindObjectOfType<AudioManager>().Play("get");
            }
            if (other.gameObject.CompareTag("Portal"))
            {
                isCatched = true;
                timeA = Time.time;


                transform.parent.gameObject.GetComponent<CharacterController>().enabled = false;
                transform.parent.gameObject.transform.position = teleportationPoint.position;
                transform.parent.gameObject.GetComponent<CharacterController>().enabled = true;

                Destroy(other.gameObject.transform.parent.gameObject);
                FindObjectOfType<AudioManager>().Play("teleport");
            }
            if (other.gameObject.name == "Compass")
            {
                isCatched = true;
                timeA = Time.time;

                miniMap.SetActive(true);

                Destroy(other.gameObject.transform.parent.gameObject);
                FindObjectOfType<AudioManager>().Play("compass");
            }
            if (other.gameObject.name == "Protein")
            {
                isCatched = true;
                timeA = Time.time;

                energy = 101f;
                isGetProtein = true;

                Destroy(other.gameObject);
                FindObjectOfType<AudioManager>().Play("protein");
            }

            if (other.gameObject.name == "Fox")
            {
                isCatched = true;
                timeA = Time.time;

                foxisFound = true;

                if (GameObject.Find("SpotLightFox") != null) Destroy(GameObject.Find("SpotLightFox"));
                FindObjectOfType<AudioManager>().Play("fox");
            }
            if (other.gameObject.CompareTag("SnowBall"))
            {
                isCatched = true;
                timeA = Time.time;

                remainingTime -= 15f;
                LifeFillIMG.color = Color.red;
                isTouchedSnowBall = true;
                FindObjectOfType<AudioManager>().Play("ough");
            }

            if (numberLog >= requiredLog && other.gameObject.name == "FinishLine")
            {
                isCatched = true;
                timeA = Time.time;

                //���
                ScoreCalculator();
                //���}���⭱�O
                isOver = true;
                scoreIMG.enabled = true;
                ranking.enabled = true;
                detail.enabled = true;
                //��ܵ��⵲�G
                string foxFound;
                if (foxisFound)
                    foxFound = "Found";
                else
                    foxFound = "Not Found";
                detail.text = "Game Time : \n" + Second_TO_Minute(timer) + "\n\nExtra Logs : " + (numberLog - requiredLog) + "\n\nFox : " + foxFound;
                winBTN.SetActive(true);

                Destroy(other.gameObject);
                FindObjectOfType<AudioManager>().Play("yeah");
            }

        }

    }
    private void TurnLifeBarBack()
    {
        LifeFillIMG.color = lifeColorOrigin;
    }

}
