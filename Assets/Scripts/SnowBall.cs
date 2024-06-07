using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameCenter;

public class SnowBall : MonoBehaviour
{
    float bornTime;
    private void Start()
    {
        bornTime = Time.time;
    }
    void Update()
    {
        if (isTouchedSnowBall || Time.time > bornTime + 15f)
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;

            DisappearSomethingSlowly(gameObject.GetComponent<MeshRenderer>().material, Time.deltaTime * .3f);
            //print(gameObject.GetComponent<MeshRenderer>().material.name);
        }
        if (gameObject.transform.position.y < -30f)
        {
            Destroy(gameObject);
        }
    }
}
