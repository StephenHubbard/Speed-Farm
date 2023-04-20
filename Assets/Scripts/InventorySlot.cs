using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private PlacedObjectTypeSO _placedObjectTypeSO;
    [SerializeField] private MonoBehaviour _item;

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return _placedObjectTypeSO;
    }

    public void ClickInventorySlot(int indexNum) {
        if (!_placedObjectTypeSO) { InventoryManager.Instance.DeselectPlacedObjecTypeSO(); }

        InventoryManager.Instance.MoveSelectionOutline(indexNum, _placedObjectTypeSO);
    }

    public void UseInventorySlot() {
        if (_item) {
            (_item as Item).UseItem();
        }
    }
}
