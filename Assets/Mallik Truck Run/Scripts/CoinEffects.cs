using DG.Tweening;
using UnityEngine;

public class CoinEffects : Singleton<CoinEffects>
{
    public GameObject coinsParticle;
    public AudioClip coinefftsund;
    private AudioSource _audioSource => GetComponent<AudioSource>();
    private void Start()
    {
        gameObject.AddComponent<AudioSource>().clip = coinefftsund;
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    public void PlayCoinEffects(RectTransform coinIconRect, Vector3 startPos)
    {
        coinsParticle.GetComponent<ParticleControlScript>().PlayControlledParticles(coinIconRect, startPos);
        DOVirtual.DelayedCall(1, () =>
        {

            _audioSource.Play();
        });
    }
}
