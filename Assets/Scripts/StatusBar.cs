using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum Bar
{
    None = 0,
    Health = 1,
    Nutrition = 2,
    Hydration = 3
}

public class StatusBar : MonoBehaviour
{

    [SerializeField] Bar bar;
    [SerializeField] GameObject playerState;

    Slider slider;
    TextMeshProUGUI barText;

    private float currentValue, maxValue;

    void Awake()
    {
        slider = GetComponent<Slider>();
        barText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        switch (bar)
        {
            case Bar.None:
                break;

            case Bar.Health:
                currentValue = playerState.GetComponent<PlayerState>().currentHealth;
                maxValue = playerState.GetComponent<PlayerState>().maxHealth;
                break;

            case Bar.Nutrition:
                currentValue = playerState.GetComponent<PlayerState>().currentNutrition;
                maxValue = playerState.GetComponent<PlayerState>().maxNutrition;
                break;

            case Bar.Hydration:
                currentValue = playerState.GetComponent<PlayerState>().currentHydration;
                maxValue = playerState.GetComponent<PlayerState>().maxHydration;
                break;

        }

        float fillValue = currentValue / maxValue;
        slider.value = fillValue;

        barText.text = currentValue + "/" + maxValue;
    }
}
