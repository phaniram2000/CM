using UnityEngine;

public class ControlledParticles : MonoBehaviour {

    public RectTransform targetForParticles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            ParticleControl.PlayControlParticles(collision.transform.position, targetForParticles);
        }
    }
}
