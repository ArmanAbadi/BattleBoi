using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider HealthBarGreen;

    public void UpdateHealthBar(float percent)
    {
        HealthBarGreen.value = percent;
    }
}
