using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Backpack : Singleton<Backpack>, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject BackPackContainer => _backpackContainer;
    public Transform[] BackpackSlots => _backpackSlots;
    public bool _backPackOpen { get; private set; }

    [SerializeField] private Sprite _backPackDefault;
    [SerializeField] private Sprite _backPackHighlight;
    [SerializeField] private Transform[] _backpackSlots;
    [SerializeField] private GameObject _backpackContainer;

    private List<ItemSO> _itemsToAddToBackpack = new List<ItemSO>();
    private Image _image;
    private MoveWindowOffScreen _moveWindowOffScreen;


    protected override void Awake() {
        base.Awake();
        
        _image = GetComponent<Image>();
        _moveWindowOffScreen = _backpackContainer.GetComponentInChildren<MoveWindowOffScreen>();
    }

    private void Start() {
        _backPackOpen = _backpackContainer.gameObject.activeInHierarchy;
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

    public void ToggleBackpack()
    {
        if (_backPackOpen)
        {
            _backPackOpen = false;

            _moveWindowOffScreen.CloseWindow();
        }
        else
        {
            OpenBackPack();
        }
    }

    public void OpenBackPack()
    {
        if (_backPackOpen) { return; }

        _moveWindowOffScreen.OpenWindow();
        _backPackOpen = true;
        DumpItemsInBackpack();
    }

    public void AddItemToBackpackDumpList(ItemSO itemSO) {
        _itemsToAddToBackpack.Add(itemSO);

        DumpItemsInBackpack();
    }

    private void DumpItemsInBackpack() {
        foreach (ItemSO item in _itemsToAddToBackpack)
        {
            AddItemToBackpack(item, false);
        }

        _itemsToAddToBackpack.Clear();
    }

    public void AddItemToBackpack(ItemSO itemSO, bool boughtItem)
    {
        if (_backpackContainer.activeInHierarchy) {

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
                    newItem.UpdateAmountLeftText();
                    return;
                }
            }

            Debug.Log("no available slots");
        } else {
            _itemsToAddToBackpack.Add(itemSO);
        }
    }
}
