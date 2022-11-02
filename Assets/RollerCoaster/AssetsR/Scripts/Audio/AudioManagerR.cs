using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerR : MonoBehaviour
{
	public static AudioManagerR instance;

	public AudioMixerGroup mixerGroup;

	public SoundR[] sounds;
	
	private void OnEnable() => SceneManager.sceneLoaded += OnLevelLoaded;

    private void OnDisable() => SceneManager.sceneLoaded -= OnLevelLoaded;

	private void Awake()
	{
		transform.parent = null;
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (SoundR s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();

			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.bypassReverbZones = !s.enableReverb;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

    private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
		foreach (var source in GetComponents<AudioSource>())
			source.Stop();
    }

    public void Play(string sound, float volume = -1f, float pitch = -1f)
	{
		SoundR s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		if (volume > 0f)
			s.source.volume = volume * (1f + UnityEngine.Random.Range(-s.volumeVariance * volume / 2f, s.volumeVariance * volume / 2f));
		else
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));

		if (pitch > 0f)
			s.source.pitch = pitch;
		else
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		
		s.source.Play();
	}

    public void Pause(string sound)
    {
        SoundR s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Pause();
    }
}