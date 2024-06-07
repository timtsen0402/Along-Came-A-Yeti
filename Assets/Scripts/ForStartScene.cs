using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ForStartScene : MonoBehaviour
{
    bool isPlayerStarted = false;
    public VideoPlayer videoPlayer;
    public RawImage playshower;
    public Animator animator;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private void Update()
    {
        VideoController();
    }
    void VideoController()
    {
        if (videoPlayer == null || playshower == null)
            return;
        //正在播
        if (isPlayerStarted == false && videoPlayer.isPlaying == true)
        {
            isPlayerStarted = true;
        }
        //跳過或播完了
        if (Input.GetKeyDown(KeyCode.Space) || (isPlayerStarted == true && videoPlayer.isPlaying == false))
        {
            Destroy(videoPlayer.gameObject);
            Destroy(playshower.gameObject);
            animator.SetTrigger("VideoOver");
            FindObjectOfType<AudioManager>().Play("boom");

        }


    }
}
