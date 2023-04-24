using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private IItem _item;

    private void Awake() {
        _item = GetComponentInChildren<IItem>();
    }

    public void ClickInventorySlot(int indexNum) {

        InventoryManager.Instance.MoveSelectionOutline(indexNum);
    }

    public void EquipItem() {
        if (_item == null) { return; }

        _item.EquipItem();

    }
}
