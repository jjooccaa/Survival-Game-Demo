using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : Singleton<EquipSystem>
{
    [SerializeField] GameObject toolHolder;

    [Header("UI")]
    [SerializeField] GameObject actionBar;
    [SerializeField] GameObject numbersHolder;

    [HideInInspector] public List<GameObject> actBarSlotList = new List<GameObject>();

    [HideInInspector] public int selectedNumber = -1;
    [HideInInspector] public GameObject selectedItem;
    [HideInInspector] public GameObject selectedItemModel;

    void Start()
    {
        GenerateActionSlotList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectActionSlot(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectActionSlot(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectActionSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectActionSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectActionSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectActionSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectActionSlot(7);
        }
    }

    void GenerateActionSlotList()
    {
        foreach(Transform child in actionBar.transform)
        {
            if(child.CompareTag("ActionSlot"))
            {
                actBarSlotList.Add(child.gameObject);
            }
        }
    }

    public void AddItemToActionBar(GameObject itemToEquip)
    {
        GameObject availableSlot = FindNextEmptySlot();
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.RefreshItemList();
    }

    GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in actBarSlotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    void SelectActionSlot(int number)
    {
        if (CheckIfSlotIsFull(number))
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                // Unselect previosly selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);

                //Change color of slots number
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = Color.red;
                }

                TextMeshProUGUI text = numbersHolder.transform.Find("Number" + number).transform.Find("Text").GetComponent<TextMeshProUGUI>();
                text.color = Color.white;
            }
            else // we select the same slot number
            {
                // Unselect previosly selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null) // temp solution
                {
                    DestroyImmediate(selectedItemModel);
                    selectedItemModel = null;
                }

                // Change color of slot number
                numbersHolder.transform.GetChild(selectedNumber - 1).Find("Text").GetComponent<TextMeshProUGUI>().color = Color.red;

                selectedNumber = -1;
            }
        }
    }

    bool CheckIfSlotIsFull(int slotNumber)
    {
        if (actBarSlotList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null) // temp solution
        {
            DestroyImmediate(selectedItemModel);
            selectedItemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)","");
        GameObject itemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), 
            new Vector3(2.9f, -0.3f, 3.5f), Quaternion.Euler(-15, -90f, -10f));
        itemModel.transform.SetParent(toolHolder.transform, false);
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return actBarSlotList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    public bool CheckIfActionBarIsFull()
    {
        int counter = 0;

        foreach(GameObject slot in actBarSlotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter++;
            }
        }

        if(counter == 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
