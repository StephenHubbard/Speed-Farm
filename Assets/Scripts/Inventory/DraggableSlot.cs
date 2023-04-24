using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSlot : MonoBehaviour, IDropHandler
{
    private InventorySlot _inventorySlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.ParentAfterDrag = transform;
            IItem item = draggableItem.GetComponent<IItem>();
            _inventorySlot = GetComponentInParent<InventorySlot>();
            _inventorySlot?.FindSlottedItem(item);
        }
    }
}
