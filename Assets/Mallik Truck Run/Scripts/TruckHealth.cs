using UnityEngine;
using UnityEngine.UI;

public class TruckHealth : MonoBehaviour
{
    [HideInInspector] public int health = 100;
    public Slider healthSlider;
    public void TakeHealth()
    {
        health -= 3;
        healthSlider.value = health;
        if(health <= 0)
        {
            GameEvents.InvokeGameLose(-1);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("enemyObj"))
        {
            TakeHealth();
        }
    }
}
