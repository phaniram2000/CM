using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sounds 
{
    public string name;

    public bool mute;
    public bool loop;

    [Range(0f,1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch;
    [Range(0,256)]
    public int priority;
   
    public AudioClip clip;
    public AudioSource source;

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void AudioSourceState(bool state)
    {
        source.enabled = state;
    }
  
    public void GetItRead(AudioSource AS)
    {
        source = AS;
        source.clip = clip;
        source.mute = mute;
        source.loop = loop;
        source.pitch = pitch;
        source.volume = volume;
        source.priority = priority;
    }
}
