using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        
        if (transform.childCount > 0) {
            Transform itemToSwap = transform.GetChild(0);
            itemToSwap.SetParent(draggableItem.PreviousParent, false);
            InventorySlot inventorySlot = itemToSwap.GetComponentInParent<InventorySlot>();
            inventorySlot?.FindSlottedItem();
            StartCoroutine(CheckIfActiveSlotRoutine(inventorySlot));
        } else {
            InventorySlot inventorySlot = GetComponent<InventorySlot>();
            StartCoroutine(CheckIfActiveSlotRoutine(inventorySlot));
        }

        draggableItem.ParentAfterDrag = transform;
        InventoryManager.Instance.CurrentEquippedItemNull();
    }

    // Wait for next frame for transform parenting race condition
    private IEnumerator CheckIfActiveSlotRoutine(InventorySlot inventorySlot) {
        yield return null;
        InventoryManager.Instance.MoveSelectionOutline(InventoryManager.Instance.CurrentIndexNum); 
    }
}
