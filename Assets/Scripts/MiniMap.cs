using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameCenter;
public class MiniMap : MonoBehaviour
{ 
    private void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;

        transform.position = newPos;
        transform.rotation = Quaternion.Euler(90f,player.eulerAngles.y, 0f);
    }
}
