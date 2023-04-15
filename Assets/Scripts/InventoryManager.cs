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

    private PlacedObjectTypeSO placedObjectTypeSO;

    private int previousIndexNum;
    private int currentIndexNum;

    private void Start() {
        MoveSelectionOutline(0, inventorySlots[0].GetComponent<InventorySlot>().GetPlacedObjectTypeSO());
        previousIndexNum = 5;
        IsShovelEquipped = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { MoveSelectionOutline(0, inventorySlots[0].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { MoveSelectionOutline(1, inventorySlots[1].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { MoveSelectionOutline(2, inventorySlots[2].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { MoveSelectionOutline(3, inventorySlots[3].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { MoveSelectionOutline(4, inventorySlots[4].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }

        if (Input.GetKeyDown(KeyCode.Tab)) { TogglePrevious(); }
    }

    public void DeselectPlacedObjecTypeSO() {
        placedObjectTypeSO = null;
    }

    public void MoveSelectionOutline(int indexNum, PlacedObjectTypeSO placedObjectTypeSO) {
        GridGeneration.Instance.RefreshSelectedObjectType();
        IsShovelEquipped = false;

        int tempIndexNum = currentIndexNum;
        previousIndexNum = tempIndexNum;
        currentIndexNum = indexNum;
        selectionOutline.SetParent(inventorySlots[indexNum], false);
        inventorySlots[indexNum].GetComponent<InventorySlot>().UseInventorySlot();

        if (placedObjectTypeSO) {
            this.placedObjectTypeSO = placedObjectTypeSO;
            GridGeneration.Instance.SetPlacedObjectTypeSO(placedObjectTypeSO);
        }
    }

    public void ClickShovelIcon() {
        IsShovelEquipped = true;
    }

    private void TogglePrevious() {
        int tempIndexNum = currentIndexNum;
        MoveSelectionOutline(previousIndexNum, inventorySlots[previousIndexNum].GetComponent<InventorySlot>().GetPlacedObjectTypeSO());
        previousIndexNum = tempIndexNum;
    }
}
