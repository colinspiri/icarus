using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private PlayerInfo playerInfo;
    private readonly float tolerance = 0.0001f;

    private void Start()
    {
        healthSlider.value = healthSlider.maxValue;
    }

    public void UpdateSliderValue() => healthSlider.value = playerInfo.currentHealthPercentage;

    public void UpdateSliderColor()
    {
        if (healthSlider.value <= healthSlider.maxValue * 1 / 3 + tolerance) healthSlider.fillRect.GetComponent<Image>().color = Color.red;
        else if (healthSlider.value <= healthSlider.maxValue * 2 / 3 + tolerance) healthSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        else healthSlider.fillRect.GetComponent<Image>().color = Color.green;
    }
};
