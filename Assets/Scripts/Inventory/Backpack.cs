using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Backpack : Singleton<Backpack>, IPointerEnterHandler, IPointerExitHandler
{
    public bool _backpackContainerIsOpen { get; private set; }
    public GameObject BackPackContainer => _backpackContainer;
    public Transform[] BackpackSlots => _backpackSlots;
    public MoveWindowOffScreen MoveWindowOffScreen => _moveWindowOffScreen;

    [SerializeField] private MoveWindowOffScreen _moveWindowOffScreen;
    [SerializeField] private Sprite _backPackDefault;
    [SerializeField] private Sprite _backPackHighlight;
    [SerializeField] private Transform[] _backpackSlots;
    [SerializeField] private GameObject _backpackContainer;

    private Image _image;

    protected override void Awake() {
        base.Awake();
        
        _image = GetComponent<Image>();
    }

    private void Start() {
        _backpackContainerIsOpen = _backpackContainer.activeInHierarchy;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) { ToggleBackpack(); }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.sprite = _backPackHighlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.sprite = _backPackDefault;
    }

    public void OpenBackPack() {
        _backpackContainerIsOpen = true;
        _moveWindowOffScreen.OpenWindow();
    }

    public void ToggleBackpack()
    {
       if (_backpackContainerIsOpen) {
            _backpackContainerIsOpen = false;
            _moveWindowOffScreen.CloseWindow();
       } else {
            _backpackContainerIsOpen = true;
            _moveWindowOffScreen.OpenWindow();
       }
    }

    public void AddItemToBackpack(ItemSO itemSO, bool boughtItem)
    {
        DraggableItem[] allItems = FindObjectsOfType<DraggableItem>();

        if (!boughtItem) {
            foreach (DraggableItem item in allItems)
            {
                ItemSO potentialItemSO = item.ItemSO;

                if (potentialItemSO == itemSO)
                {
                    item.GetComponent<DraggableItem>().UpdateAmountLeft(1);
                    return;
                }
            }
        }

        foreach (Transform backpackSlot in _backpackSlots)
        {
            if (backpackSlot.childCount == 0)
            {
                DraggableItem newItem = Instantiate(itemSO.IventoryPrefab, backpackSlot.transform).GetComponent<DraggableItem>();
                newItem.CurrentAmount = 1;
                return;
            }
        }

        Debug.Log("no available slots");
    }
}
