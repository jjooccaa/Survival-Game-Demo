using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject Item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!Item)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragAndDrop.itemBeingDragged.transform.SetParent(transform);
            DragAndDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if(!transform.CompareTag("ActionSlot"))
            {
                DragAndDrop.itemBeingDragged.GetComponent<InventoryItem>().isInActionSlot = false;
                InventorySystem.Instance.RefreshItemList();
            }

            if (transform.CompareTag("ActionSlot"))
            {
                DragAndDrop.itemBeingDragged.GetComponent<InventoryItem>().isInActionSlot = true;
                InventorySystem.Instance.RefreshItemList();
            }
        }
    }
}
