using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar : MonoBehaviour
{
    public GameObject globalStateSystem;

    Slider slider;
    float currentHealth;
    float maxHealth;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        currentHealth = globalStateSystem.GetComponent<GlobalState>().resourceHeath;
        maxHealth = globalStateSystem.GetComponent<GlobalState>().resourceMaxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
    }
}
