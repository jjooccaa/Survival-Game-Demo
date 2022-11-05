using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] GameObject trashPopUpAlert;
    [SerializeField] Sprite trashClosed;
    [SerializeField] Sprite trashOpened;

    TextMeshProUGUI textToModify;
    Image image;
    Button yesButton, noButton;

    GameObject itemToBeDeleted;
    GameObject draggedItem
    {
        get
        {
            return DragAndDrop.itemBeingDragged;
        }
    }

    [HideInInspector] public string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            return name.Replace("(Clone)", "");
        }
    }
    
    void Start()
    {
        image = transform.Find("Background").GetComponent<Image>();

        textToModify = trashPopUpAlert.transform.Find("TopText").GetComponent<TextMeshProUGUI>();

        yesButton = trashPopUpAlert.transform.Find("YesButton").GetComponent<Button>();
        yesButton.onClick.AddListener(delegate { RemoveItem(); });

        noButton = trashPopUpAlert.transform.Find("NoButton").GetComponent<Button>();
        noButton.onClick.AddListener(delegate { CancelRemoving(); });
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(draggedItem.GetComponent<InventoryItem>().isRemoveable == true)
        {
            itemToBeDeleted = draggedItem.gameObject;

            StartCoroutine(AlertBeforeRemoving());
        }
    }

    IEnumerator AlertBeforeRemoving()
    {
        trashPopUpAlert.SetActive(true);
        textToModify.text = "Are you sure you want to throw away " + itemName + "?";
        yield return new WaitForSeconds(1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(draggedItem != null && draggedItem.GetComponent<InventoryItem>().isRemoveable == true)
        {
            image.sprite = trashOpened;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isRemoveable == true)
        {
            image.sprite = trashClosed;
        }
    }

    void RemoveItem()
    {
        image.sprite = trashClosed;
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.RefreshItemList();
        CraftingSystem.Instance.RefreshNeededItems();
        trashPopUpAlert.SetActive(false);
    }

    void CancelRemoving()
    {
        image.sprite = trashClosed;
        trashPopUpAlert.SetActive(false);
    }
}
