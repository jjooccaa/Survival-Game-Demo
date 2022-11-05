using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Singleton<PlayerState>
{
    [SerializeField] GameObject playerObject;

    //Health
    public float currentHealth;
    public float maxHealth;

    // Nutrition
    public float currentNutrition;
    public float maxNutrition;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    //Hydration
    public float currentHydration;
    public float maxHydration;
    public bool isHydrationActive;

    void Start()
    {
        currentHealth = maxHealth;
        currentNutrition = maxNutrition;
        currentHydration = maxHydration;

        StartCoroutine(DecreaseHydration());
    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(playerObject.transform.position, lastPosition);
        lastPosition = playerObject.transform.position;

        if(distanceTravelled >= 20)
        {
            distanceTravelled = 0;
            currentNutrition -= 1;
        }
    }

    IEnumerator DecreaseHydration()
    {
        while(true)
        {
            currentHydration -= 1;
            yield return new WaitForSeconds(5);
        }
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
    }

    public void SetNutrition(float nutrition)
    {
        currentNutrition = nutrition;
    }

    public void SetHydration(float hydration)
    {
        currentHydration = hydration;
    }
}
