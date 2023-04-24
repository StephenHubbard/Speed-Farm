using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    public Transform PreviousParent { get => _parentAfterDrag; set => _parentAfterDrag = value; }

    private Transform _parentAfterDrag;
    private Transform _previousParent;

    private Image _image;
    private InventorySlot _inventorySlot;

    private void Awake() {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _previousParent = transform.parent; 
        _inventorySlot = GetComponentInParent<InventorySlot>();
        _inventorySlot?.SlottedItemNull();
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_parentAfterDrag);
        transform.position = transform.parent.position;
        _image.raycastTarget = true;

        IItem item = GetComponent<IItem>();
        _inventorySlot = GetComponentInParent<InventorySlot>();
        _inventorySlot?.FindSlottedItem(item);
    }
}
