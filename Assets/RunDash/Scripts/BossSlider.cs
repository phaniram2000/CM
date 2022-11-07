using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossSlider : MonoBehaviour
{
    [SerializeField] public Slider slider_;
    [SerializeField] public Gradient gradient;
    [SerializeField] public Image fill;
    public void setmaxhealth(int health)
    {
        slider_.maxValue = health;
        slider_.value = health;
        fill.color = gradient.Evaluate(1f);
    }
    public void sethealth(int health)
    {
        slider_.value = health;
        fill.color = gradient.Evaluate(slider_.normalizedValue);
    }
}