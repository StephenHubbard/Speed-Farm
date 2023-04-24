using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public int CurrentIndexNum => _currentIndexNum;

    [SerializeField] private Transform _inventoryContainer;
    [SerializeField] private Transform[] _inventorySlots;
    [SerializeField] private Transform[] _backpackSlots;
    [SerializeField] private Transform _shovelSlot;
    [SerializeField] private Transform _selectionOutline;
    [SerializeField] private GameObject _backpackContainer;

    [SerializeField] private IItem _currentEquippedItem;

    private int _previousIndexNum;
    private int _currentIndexNum;

    private void Start() {
        MoveSelectionOutline(0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { MoveSelectionOutline(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { MoveSelectionOutline(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { MoveSelectionOutline(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { MoveSelectionOutline(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { MoveSelectionOutline(4); }

        if (Input.GetKeyDown(KeyCode.Tab)) { TogglePrevious(); }
        if (Input.GetKeyDown(KeyCode.R)) { MoveSelectionOutline(5); }
        if (Input.GetKeyDown(KeyCode.T)) { MoveSelectionOutline(6); }
        if (Input.GetKeyDown(KeyCode.Space)) { MoveSelectionOutline(7); }
        if (Input.GetKeyDown(KeyCode.B)) { ToggleBackpack(); }
    }

    public void UseCurrentEquippedItem() {
        if (_currentEquippedItem == null) { return; }
        
        _currentEquippedItem.UseItem();
    }

    public void MoveSelectionOutline(int indexNum) {
        GridGeneration.Instance.RefreshSelectedObjectType();

        if (_currentIndexNum != indexNum) {
            _previousIndexNum = _currentIndexNum;
        }

        _currentIndexNum = indexNum;

        _selectionOutline.transform.position = _inventorySlots[_currentIndexNum].transform.position;
        _inventorySlots[_currentIndexNum].GetComponent<InventorySlot>().EquipItem();
    }

    public void SetActiveEquippedItem(MonoBehaviour item) {
        _currentEquippedItem = (item as IItem);
    }

    public void CurrentEquippedItemNull() {
        _currentEquippedItem = null;
    }

    private void TogglePrevious() {
        int tempIndexNum = _currentIndexNum;
        MoveSelectionOutline(_previousIndexNum);
        _previousIndexNum = tempIndexNum;
    }

    public void ToggleBackpack() {
        if (_backpackContainer.activeInHierarchy) {
            CloseWindow[] allCloseWindows = FindObjectsOfType<CloseWindow>();

            foreach (CloseWindow closeWindow in allCloseWindows)
            {
                closeWindow.WindowClose();
            }
        } else {
            _backpackContainer.SetActive(true);
        }
    }

    public void OpenBackPack() {
        _backpackContainer.SetActive(true);
    }

    public void AddItemToBackpack(GameObject itemToAdd) {
        foreach (Transform backpackSlot in _backpackSlots)
        {
            if (backpackSlot.childCount == 0) {
                DraggableItem newItem = Instantiate(itemToAdd, backpackSlot.transform).GetComponent<DraggableItem>();
                newItem.CurrentAmount = 1;
                newItem.UpdateAmountLeftText();
                return;
            } 
        }

        Debug.Log("no available slots");
    }
}
