using UnityEngine;

public class MetaAudioManager : MonoBehaviour
{
	public static MetaAudioManager instance;
	public AudioSource audioSource1, audioSource2, audiosource3;
	public AudioClip moneyCollect, tapSound, buildingUnlock, uiTap, buying, moneyCollected;

	private void Awake()
	{
		if (instance)
			Destroy(gameObject);
		else
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}

		Vibration.Init();
	}

	public void TurnOnAS3() => audiosource3.enabled = true;

	public void TurnOFFAS3() => audiosource3.enabled = false;

	public void PlayTapSound()
	{
		if (!audioSource1.isPlaying)
			audioSource1.PlayOneShot(tapSound);
	}

	public void PlaymoneyCollectSound() => audioSource1.PlayOneShot(moneyCollect);

	public void PlayUiTapSound() => audioSource1.PlayOneShot(uiTap);

	public void StopLaserSound()
	{
		audioSource2.Stop();
		audioSource2.mute = true;
	}

	public void PlayBuildingUnlockSound() => audioSource1.PlayOneShot(buildingUnlock);

	public void PlayBuyingSound() => audioSource1.PlayOneShot(buying);

	public void PlayMoneyCollectedSound() => audioSource1.PlayOneShot(moneyCollected);
}