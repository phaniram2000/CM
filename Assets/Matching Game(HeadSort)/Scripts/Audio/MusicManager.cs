using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] bgTracks;
    public AudioClip[] ambienceTracks;
    public float transitionInterval = 5f;

    private int lastTrackIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
        PlayAmbience();
    }

    // Update is called once per frame
    void PlayMusic()
    {
        Debug.Log("Play Music");
        AudioClip clipToPlay;
        int randIndex = 0;
        do
        {
            randIndex = Random.Range(0, bgTracks.Length);
        }
        while (randIndex == lastTrackIndex);
        clipToPlay = bgTracks[randIndex];
        AudioManagerMatch.instance.PlayMusic(clipToPlay, 2f);
        lastTrackIndex = randIndex;
        Invoke("PlayMusic", clipToPlay.length + transitionInterval);
    }

    void PlayAmbience()
    {
        var clipToPlay = ambienceTracks[Random.Range(0, ambienceTracks.Length)];
        AudioManagerMatch.instance.PlayAmbience(clipToPlay);
        Invoke("PlayAmbience", clipToPlay.length);
    }
}
