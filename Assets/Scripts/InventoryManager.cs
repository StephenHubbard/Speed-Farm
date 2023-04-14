using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public bool IsShovelEquipped { get; private set; }

    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private Transform[] inventorySlots;
    [SerializeField] private Transform shovelSlot;
    [SerializeField] private Transform selectionOutline;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;

    private PlacedObjectTypeSO placedObjectTypeSO;

    private int previousIndexNum;

    private void Start() {
        placedObjectTypeSO = placedObjectTypeSOList[0]; 
        IsShovelEquipped = false;
        MoveSelectionOutline(0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { MoveSelectionOutline(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { MoveSelectionOutline(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { MoveSelectionOutline(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { MoveSelectionOutline(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { MoveSelectionOutline(4); }

        if (Input.GetKeyDown(KeyCode.Tab)) { ToggleShovel(); }
    }

    public void MoveSelectionOutline(int indexNum) {
        IsShovelEquipped = false;
        previousIndexNum = indexNum;
        selectionOutline.SetParent(inventorySlots[indexNum], false);
        placedObjectTypeSO = placedObjectTypeSOList[indexNum];
        GridGeneration.Instance.SetPlacedObjectTypeSO(placedObjectTypeSO);
        GridGeneration.Instance.RefreshSelectedObjectType();
    }

    private void ToggleShovel() {
        if (!IsShovelEquipped) {
            selectionOutline.SetParent(shovelSlot, false);
            IsShovelEquipped = true;
        } else {
            MoveSelectionOutline(previousIndexNum);
            IsShovelEquipped = false;
        }

    }
}
