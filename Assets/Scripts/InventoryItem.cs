using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    [Header("UI")]
    GameObject itemInfoUI;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDescription;
    TextMeshProUGUI itemFunctionality;

    [Header("Consumables")]
    [SerializeField] float healthEffect;
    [SerializeField] float nutritionEffect;
    [SerializeField] float hydrationEffect;
    GameObject itemPendingConsumption;

    [Header("Equipping")]
    [SerializeField] bool isEquippable;
    [HideInInspector] public bool isInActionSlot;
    [HideInInspector] public bool isSelected;

    public string inventoryName, inventoryDescription, inventoryFunctionality;
    public bool isRemoveable;
    public bool isConsumable;

    void Start()
    {
        itemInfoUI = InventorySystem.Instance.itemInfoUI;
        itemName = itemInfoUI.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemDescription = itemInfoUI.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        itemFunctionality = itemInfoUI.transform.Find("Functionality").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(isSelected)
        {
            gameObject.GetComponent<DragAndDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragAndDrop>().enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemName.text = inventoryName;
        itemDescription.text = inventoryDescription;
        itemFunctionality.text = inventoryFunctionality;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(isConsumable)
            {
                itemPendingConsumption = gameObject;
                Consume(healthEffect, nutritionEffect, hydrationEffect);
            }

            if (isEquippable && !isInActionSlot && !EquipSystem.Instance.CheckIfActionBarIsFull())
            {
                EquipSystem.Instance.AddItemToActionBar(gameObject);
                isInActionSlot = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.RefreshItemList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    void Consume(float healthEffect, float nutritionEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);

        HealthEffect(healthEffect);
        NutritionEffect(nutritionEffect);
        HydrationEffect(hydrationEffect);
    }

    static void HealthEffect(float healthEffect)
    {
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if(healthEffect != 0)
        {
            if(healthBeforeConsumption + healthEffect > maxHealth)
            {
                PlayerState.Instance.SetHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.SetHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }

    private static void NutritionEffect(float nutritionEffect)
    {
        float nutritionBeforeConsumption = PlayerState.Instance.currentNutrition;
        float maxNutrition = PlayerState.Instance.maxNutrition;

        if (nutritionEffect != 0)
        {
            if ((nutritionBeforeConsumption + nutritionEffect) > maxNutrition)
            {
                PlayerState.Instance.SetNutrition(maxNutrition);
            }
            else
            {
                PlayerState.Instance.SetNutrition(nutritionBeforeConsumption + nutritionEffect);
            }
        }
    }

    private static void HydrationEffect(float hydrationEffect)
    {
        float hydrationBeforeConsumption = PlayerState.Instance.currentHydration;
        float maxHydration = PlayerState.Instance.maxHydration;

        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.SetHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.SetHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }
}
