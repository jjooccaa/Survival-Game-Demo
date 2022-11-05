using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : Singleton<InventorySystem>
{
    [SerializeField] GameObject inventoryUI;
    public GameObject itemInfoUI;

    List<GameObject> slotList = new List<GameObject>();
    [HideInInspector] public List<string> itemList = new List<string>();

    GameObject itemToAdd;
    GameObject whatSlotToEquip;

    [HideInInspector]public bool isOpen;

    //Pickup Popup
    [SerializeField] GameObject pickupPopup;
    [SerializeField] TextMeshProUGUI pickUpText;

    void Start()
    {
        isOpen = false;

        GenerateSlotList();

        Cursor.visible = false; //change later to game manager
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            OpenInventory();
        }
        else if(Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            CloseInventory();
        }
    }

    void GenerateSlotList()
    {
        foreach(Transform child in inventoryUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void OpenInventory()
    {
        inventoryUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        isOpen = true;

        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
    }

    void CloseInventory()
    {
        inventoryUI.SetActive(false);
        if (!CraftingSystem.Instance.isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
        }
        isOpen = false;
    }

    public void AddItemToInventory(string itemName)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.pickUpItemSound);

        whatSlotToEquip = FindNextAvailableSlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);
        itemList.Add(itemName);

        ShowPickupPopUp(itemName);

        RefreshItemList();
        CraftingSystem.Instance.RefreshNeededItems();
        
    }

    void ShowPickupPopUp(string itemName)
    {
        pickupPopup.SetActive(true);
        pickUpText.text = itemName;
    }

    public bool CheckAvailableSlots(int slotsNeeded)
    {
        int emptySlots = 0;

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount <= 0)
            {
                emptySlots++;
            }
        }

        if (emptySlots >= slotsNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    GameObject FindNextAvailableSlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public void RemoveItemFromInventory(string itemName, int amount)
    {
        int counter = amount;

        for(var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == itemName + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter--;
                }
            }
        }

        RefreshItemList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    public void RefreshItemList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name.Replace("(Clone)", "");

                itemList.Add(name);
            }
        }
    }
}
