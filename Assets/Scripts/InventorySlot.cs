using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSO;
    [SerializeField] private MonoBehaviour item;

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

    public void ClickInventorySlot(int indexNum) {
        if (!placedObjectTypeSO) { InventoryManager.Instance.DeselectPlacedObjecTypeSO(); }

        InventoryManager.Instance.MoveSelectionOutline(indexNum, placedObjectTypeSO);

        if (item) {
            (item as Item).UseItem();
        }
    }
}
