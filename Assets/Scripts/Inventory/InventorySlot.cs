using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private IItem _item;

    private void Awake() {
        _item = GetComponentInChildren<IItem>();
    }

    public void FindSlottedItem(IItem item) {
        _item = item;
    }

    public void FindSlottedItem() {
        _item = GetComponentInChildren<IItem>();
    }

    public void SlottedItemNull() {
        _item = null;
    }

    public void ClickInventorySlot(int indexNum) {
        InventoryManager.Instance.MoveSelectionOutline(indexNum);
    }

    public void EquipItem() {
        if (_item != null) { 
            _item.EquipItem();
        } else {
            InventoryManager.Instance.CurrentEquippedItemNull();
        }

    }
}
