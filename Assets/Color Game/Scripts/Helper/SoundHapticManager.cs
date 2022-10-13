using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalScripts;

public class SoundHapticManager : MonoBehaviour
{
    public Sounds[] sounds;


    public AudioSource audioSource, bgAS, footSteps, normalClockTicking;
    public AudioClip  correct, wrong, swipe, timer, confetti, walkSound, btnPress, betBtn, clapping, loose;

    GameEssentials gameEssentials;

    private void Awake()
    {
        SetupAudioSources();
    }

    void Start()
    {
        gameEssentials = GameEssentials.instance;

        audioSource = GetComponent<AudioSource>();
        bgAS = GetComponentInChildren<AudioSource>();
    }

    void SetupAudioSources()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].GetItRead( gameObject.AddComponent<AudioSource>());
        }
    }

    public void PlayAudio(string SoundName)
    {
        if (gameEssentials.sd.GetSoundState().Equals("Off"))
            return;

        Sounds s = Array.Find(sounds, sound => sound.name == SoundName);
        s.Play();
    }

    public void Vibrate(long MilliSecs)
    {
        if (gameEssentials.sd.GetHepaticState().Equals("Off"))
            return;

        if (Application.platform == RuntimePlatform.WindowsEditor)
           UGS.Log("Vibrating");
        else 
            Vibration.Vibrate(MilliSecs);
    }

    public void PlayFootSteps()
    {
        if (gameEssentials.sd.GetSoundState().Equals("Off") || footSteps == null)
            return;

        footSteps.Play();
    }

    public void StopFootSteps()
    {
        if (gameEssentials.sd.GetSoundState().Equals("Off") || footSteps == null)
            return;

        footSteps.Stop();
    }

    public void PlayNormalClockTicking()
    {
        if (gameEssentials.sd.GetSoundState().Equals("Off") || normalClockTicking == null)
            return;

        if (normalClockTicking.mute) 
        {
            normalClockTicking.mute = false;
            normalClockTicking.Play();
        }
    }

    public void StopNormalClockTicking()
    {
        if (gameEssentials.sd.GetSoundState().Equals("Off") || normalClockTicking == null)
            return;
        if (!normalClockTicking.mute)
            normalClockTicking.mute = true;
    }

    public void PlaySound(AudioClip audio)
    {
        if (gameEssentials.sd.GetSoundState().Equals("Off") || audio == null)
            return;

         audioSource.PlayOneShot(audio);
    }

    public void StopPlaying()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }


    public void PlayCorrectSound()
    {
        Vibrate(30);
        PlaySound(correct);
    }

    public void PlayWrongSound()
    {
        Vibrate(35);
        PlaySound(wrong);
    }

    public void PlaySwipeSound()
    {
        PlaySound(swipe);
    }

    public void PlayTimerSound()
    {
        PlaySound(timer);
    }

    public void PlayConfettiSound()
    {
        PlaySound(confetti);
    }

    public void PlayButtonPress()
    {
        PlaySound(btnPress);
    }

    public void PlayBetBtnPress()
    {
        PlaySound(betBtn);
    }

    public void PlayLooseSound()
    {
        PlaySound(loose);
    }

    public void PlayClapSound()
    {
        PlaySound(clapping);
    }

    public void Audio_On()
    {
        audioSource.enabled = true;
        bgAS.enabled = true;
    }
    public void Audio_Off()
    {
        audioSource.enabled = false;
        bgAS.enabled = false;
    }
}
