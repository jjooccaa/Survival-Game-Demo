using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] string itemName;

    public bool isPlayerInRange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isPlayerInRange && SelectionManager.Instance.inOnTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if (InventorySystem.Instance.CheckAvailableSlots(1))
            {
                InventorySystem.Instance.AddItemToInventory(itemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Name.TagPlayer))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Name.TagPlayer))
        {
            isPlayerInRange = false;
        }
    }

    public string GetItemName()
    {
        return itemName;
    }
}
