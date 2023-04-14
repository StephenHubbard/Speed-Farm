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
        MoveSelectionOutline(0, inventorySlots[0].GetComponent<InventorySlot>().GetPlacedObjectTypeSO());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { MoveSelectionOutline(0, inventorySlots[0].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { MoveSelectionOutline(1, inventorySlots[1].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { MoveSelectionOutline(2, inventorySlots[2].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { MoveSelectionOutline(3, inventorySlots[3].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { MoveSelectionOutline(4, inventorySlots[4].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }

        if (Input.GetKeyDown(KeyCode.Tab)) { ToggleShovel(); }
    }

    public void DeselectPlacedObjecTypeSO() {
        placedObjectTypeSO = null;
    }

    public void MoveSelectionOutline(int indexNum, PlacedObjectTypeSO placedObjectTypeSO) {
        IsShovelEquipped = false;
        previousIndexNum = indexNum;
        selectionOutline.SetParent(inventorySlots[indexNum], false);

        if (placedObjectTypeSO) {
            this.placedObjectTypeSO = placedObjectTypeSO;
            GridGeneration.Instance.SetPlacedObjectTypeSO(placedObjectTypeSO);
        }

        GridGeneration.Instance.RefreshSelectedObjectType();
    }

    public void ClickShovelIcon() {
        IsShovelEquipped = true;
    }

    private void ToggleShovel() {
        if (!IsShovelEquipped) {
            selectionOutline.SetParent(shovelSlot, false);
            IsShovelEquipped = true;
        } else {
            MoveSelectionOutline(previousIndexNum, inventorySlots[previousIndexNum].GetComponent<InventorySlot>().GetPlacedObjectTypeSO());
            IsShovelEquipped = false;
        }
    }
}
