using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameCenter;

public class MouseLook : MonoBehaviour
{
    float xRotation = 0f;
    void Update()
    {
        if (Time.timeScale == 0f || isOver) { Cursor.lockState = CursorLockMode.None; }
        else { Cursor.lockState = CursorLockMode.Locked; }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitive * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitive * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
