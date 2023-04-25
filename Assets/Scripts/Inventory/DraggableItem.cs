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
    public ItemSO ItemSO => _itemSO;

    [SerializeField] private ItemSO _itemSO;
    [SerializeField] private int _startingAmount;

    private int _currentAmount = 0;

    private Transform _parentAfterDrag;
    private Transform _previousParent;

    private Image _image;
    private TMP_Text _amountLeftText;


    private void Awake() {
        _image = GetComponent<Image>();
        _amountLeftText = GetComponentInChildren<TMP_Text>();
        _currentAmount = _startingAmount;
    }

    private void Start() {
        UpdateAmountLeftText();
    }
    
    public void UpdateAmountLeft(int amount) {
        _currentAmount += amount;
        UpdateAmountLeftText();

        if (_currentAmount <= 0) {
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
        UpdateAmountLeftText();
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

        IItem item = GetComponent<IItem>();
        InventorySlot inventorySlot = GetComponentInParent<InventorySlot>();
        inventorySlot?.FindSlottedItem(item);
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

       if (GetComponent<DraggableItem>()) {
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

            InventoryManager.Instance.AddItemToBackpack(this.gameObject);
       }

        UpdateAmountLeft(-1);
    }
}
