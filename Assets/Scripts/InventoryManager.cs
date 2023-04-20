using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public bool IsShovelEquipped { get; private set; }

    [SerializeField] private Transform _inventoryContainer;
    [SerializeField] private Transform[] _inventorySlots;
    [SerializeField] private Transform _shovelSlot;
    [SerializeField] private Transform _selectionOutline;

    private PlacedObjectTypeSO _placedObjectTypeSO;

    private int _previousIndexNum;
    private int _currentIndexNum;

    private void Start() {
        MoveSelectionOutline(0, _inventorySlots[0].GetComponent<InventorySlot>().GetPlacedObjectTypeSO());
        _previousIndexNum = 5;
        IsShovelEquipped = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { MoveSelectionOutline(0, _inventorySlots[0].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { MoveSelectionOutline(1, _inventorySlots[1].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { MoveSelectionOutline(2, _inventorySlots[2].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { MoveSelectionOutline(3, _inventorySlots[3].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { MoveSelectionOutline(4, _inventorySlots[4].GetComponent<InventorySlot>().GetPlacedObjectTypeSO()); }

        if (Input.GetKeyDown(KeyCode.Tab)) { TogglePrevious(); }
    }

    public void DeselectPlacedObjecTypeSO() {
        _placedObjectTypeSO = null;
    }

    public void MoveSelectionOutline(int indexNum, PlacedObjectTypeSO placedObjectTypeSO) {
        GridGeneration.Instance.RefreshSelectedObjectType();
        IsShovelEquipped = false;

        int tempIndexNum = _currentIndexNum;
        _previousIndexNum = tempIndexNum;
        _currentIndexNum = indexNum;
        _selectionOutline.SetParent(_inventorySlots[indexNum], false);
        _inventorySlots[indexNum].GetComponent<InventorySlot>().UseInventorySlot();

        if (placedObjectTypeSO) {
            this._placedObjectTypeSO = placedObjectTypeSO;
            GridGeneration.Instance.SetPlacedObjectTypeSO(placedObjectTypeSO);
        }
    }

    public void ClickShovelIcon() {
        IsShovelEquipped = true;
    }

    private void TogglePrevious() {
        int tempIndexNum = _currentIndexNum;
        MoveSelectionOutline(_previousIndexNum, _inventorySlots[_previousIndexNum].GetComponent<InventorySlot>().GetPlacedObjectTypeSO());
        _previousIndexNum = tempIndexNum;
    }
}
