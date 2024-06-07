using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioClip BGM_now;
    public static AudioSource BGM_source;


    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = false;

            if (s.name == "runStep" || s.name == "walkStep" || s.name == "chasing")
            {
                s.source.playOnAwake = true;
                s.source.loop = true;
                s.source.enabled = false;
            }

        }
    }
    public void Enable(string name, bool enable)
    {
        Sound s = Array.Find(sounds, x => x.name == name);
        if (s == null)
            return;
        s.source.enabled = enable;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, x => x.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
}
