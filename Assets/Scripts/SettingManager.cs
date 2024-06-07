using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GameCenter;

public class SettingManager : MonoBehaviour
{
    #region SettingPanel
    public void ClosePanel()
    {
        Time.timeScale = 1f;
        settingPanel.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void SetSound(float volume)
    {
        Sound[] sounds = Audio.GetComponent<AudioManager>().sounds;
        foreach (Sound s in sounds)
        {
            s.source.volume = volume;
        }
    }
    public void SetSensitivity(float volume)
    {
        mouseSensitive = volume;
    }
    #endregion
    public void AfterClickLogHintOK()
    {
        isEnoughLog = true;
        sightRange = 500f;

        logFinishHINT.SetActive(false);
        exitLight.enabled = true;
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().Play("zap");
    }
}
