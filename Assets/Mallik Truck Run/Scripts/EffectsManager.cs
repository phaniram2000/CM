using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;

    public ParticleSystem coinsCollectionParticles;
    public ParticleSystem enemyCarBlast;

    private void Awake()
    {
        instance = this;
    }
}
