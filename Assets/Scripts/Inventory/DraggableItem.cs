using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }

    private Transform _parentAfterDrag;

    private Image _image;
    private InventorySlot _inventorySlot;

    private void Awake() {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
        _inventorySlot = GetComponentInParent<InventorySlot>();
        _inventorySlot?.SlottedItemNull();
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
        InventoryManager.Instance.CurrentEquippedItemNull();
    }
}
