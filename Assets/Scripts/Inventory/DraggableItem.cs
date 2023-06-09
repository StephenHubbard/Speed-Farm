using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    public Transform PreviousParent { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    public int CurrentAmount { get => _currentAmount; set => _currentAmount = value; }
    public int StartingAmount { get => _startingAmount; set => _startingAmount = value; }
    public ItemSO ItemSO => _itemSO;

    [SerializeField] private int _startingAmount;
    [SerializeField] private ItemSO _itemSO;

    private int _currentAmount = 0;
    private Transform _parentAfterDrag;
    private Transform _previousParent;
    private Image _image;
    private TMP_Text _amountLeftText;

    private void Awake() {
        _image = GetComponent<Image>();
        _amountLeftText = GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
        UpdateAmountLeft(_startingAmount);
    }

    private void Update() {
        UpdateAmountLeftText();
    }
    
    public void UpdateAmountLeft(int amount) {
        _currentAmount += amount;

        if (_currentAmount <= 0) {
            
            InventorySlot inventorySlot = GetComponentInParent<InventorySlot>();
            if (inventorySlot) {
                InventoryManager.Instance.CurrentEquippedItemNull();
            }

            Destroy(this.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent.GetComponent<ShopSlot>()) { return; }

        _previousParent = transform.parent;
        InventorySlot inventorySlot = GetComponentInParent<InventorySlot>();
        inventorySlot?.SlottedItemNull();
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
    }

    public void UseItem() {
        UpdateAmountLeft(-1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.GetComponent<ShopSlot>()) { return; }

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.GetComponent<ShopSlot>()) { return; }

        transform.SetParent(_parentAfterDrag);
        transform.position = transform.parent.position;
        _image.raycastTarget = true;

        InventorySlot inventorySlot = GetComponentInParent<InventorySlot>();
        inventorySlot?.FindSlottedItem(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventorySlot inventorySlot = GetComponentInParent<InventorySlot>();

            if (inventorySlot) {
                int slotIndex = inventorySlot.transform.GetSiblingIndex();
                inventorySlot.ClickInventorySlot(slotIndex);
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right) {

            ShopSlot shopSlot = GetComponentInParent<ShopSlot>();

            if (shopSlot) {
                BuyItem();
                return;
            }

            DraggableSlot draggableSlot = GetComponentInParent<DraggableSlot>();
            InventorySlot inventorySlot = GetComponentInParent<InventorySlot>();
            CrateSlot crateSlot = GetComponentInParent<CrateSlot>();

            if (!inventorySlot && !shopSlot && Backpack.Instance._backpackContainerIsOpen && !Crate.Instance.GetComponent<Building>().MoveWindowOffScreen._windowIsOpen) {
                Transform[] allInventorySlots = InventoryManager.Instance.InventorySlots;

                foreach (Transform slot in allInventorySlots)
                {
                    if (slot.transform.childCount == 0) {
                        transform.SetParent(slot.transform);
                        transform.position = transform.parent.position;
                        slot.GetComponent<InventorySlot>().FindSlottedItem(this);
                        return;
                    }
                }
            } else if (inventorySlot && Backpack.Instance._backpackContainerIsOpen && !Crate.Instance.GetComponent<Building>().MoveWindowOffScreen._windowIsOpen) {
                Transform[] allBackPackSlots = Backpack.Instance.BackpackSlots;

                foreach (Transform backPackSlot in allBackPackSlots)
                {
                    if (backPackSlot.transform.childCount == 0)
                    {
                        transform.SetParent(backPackSlot.transform);
                        transform.position = transform.parent.position;
                        return;
                    }
                }
            } else if (crateSlot && Crate.Instance.GetComponent<Building>().MoveWindowOffScreen._windowIsOpen) {
                Transform[] allBackPackSlots = Backpack.Instance.BackpackSlots;

                foreach (Transform backPackSlot in allBackPackSlots)
                {
                    if (backPackSlot.transform.childCount == 0)
                    {
                        transform.SetParent(backPackSlot.transform);
                        transform.position = transform.parent.position;
                        return;
                    }
                }
            } else if (!shopSlot && !crateSlot && Crate.Instance.GetComponent<Building>().MoveWindowOffScreen._windowIsOpen){
                List<CrateSlot> allCrateSlots = Crate.Instance.AllCrateSlots;

                foreach (CrateSlot thisCrateSlot in allCrateSlots)
                {
                    if (thisCrateSlot.transform.childCount == 0)
                    {
                        transform.SetParent(thisCrateSlot.transform);
                        transform.position = transform.parent.position;
                        return;
                    }
                }
            }
        }
    }

    public void UpdateAmountLeftText()
    {
        _amountLeftText.text = _currentAmount.ToString();
    }

    private void BuyItem() {
        if (EconomyManager.Instance.CurrentCoinAmount <= _itemSO.ItemCost) { return; }

        EconomyManager.Instance.UpdateCurrentCoinAmount(-_itemSO.ItemCost);

        DraggableItem[] allItems = FindObjectsOfType<DraggableItem>(); 

        foreach (DraggableItem item in allItems)
        {
            ItemSO potentialItemSO = item.ItemSO;

            if (potentialItemSO == _itemSO && item != this) {
                item.GetComponent<DraggableItem>().UpdateAmountLeft(1);
                UpdateAmountLeft(-1);
                return;
            }
        }

        Backpack.Instance.AddItemToBackpack(_itemSO, true);

        UpdateAmountLeft(-1);
    }
}
