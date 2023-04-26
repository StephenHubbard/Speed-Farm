using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour, IIBuilding
{
    [SerializeField] private int _amountOfStartingItems = 2;
    [SerializeField] private float _shopRefreshTime = 10f;
    [SerializeField] private List<ItemSO> _itemsForSale = new List<ItemSO>();
    [SerializeField] private Transform _shopSlotsContainer;
    
    private List<ShopSlot> _shopSlots = new List<ShopSlot>();

    private bool _hasBeenOpened = false;
    private int _amountOfItemsToRefreshShop = 0;
    private Coroutine _refreshItemsCoroutine;

    private void Awake() {
        foreach (Transform shopSlot in _shopSlotsContainer)
        {
            _shopSlots.Add(shopSlot.GetComponent<ShopSlot>());
        }
    }

    private void Start() {
        _refreshItemsCoroutine = StartCoroutine(RefreshItemsRoutine());
    }

    public void OpenBuilding()
    {
        if (_refreshItemsCoroutine != null) { StopCoroutine(_refreshItemsCoroutine); }
        _refreshItemsCoroutine = StartCoroutine(RefreshItemsRoutine());

        AddItemsToShop(_amountOfItemsToRefreshShop);

        if (!_hasBeenOpened) { 
            _hasBeenOpened = true;
            AddItemsToShop(_amountOfStartingItems);
        }
    }

    private void AddItemsToShop(int amountOfItemsToAdd) {
        for (int i = 0; i < amountOfItemsToAdd; i++)
        {
            bool itemAlreadyExists = false;

            int randomIndexNum = Random.Range(0, _itemsForSale.Count);

            foreach (Transform shopSlot in _shopSlotsContainer)
            {
                if (shopSlot.childCount > 0) {
                    Transform shopSlotChild = shopSlot.GetChild(0);
                    DraggableItem draggableItem = shopSlotChild?.GetComponent<DraggableItem>();
                    
                    if (draggableItem && draggableItem.ItemSO == _itemsForSale[randomIndexNum]) {
                        itemAlreadyExists = true;
                        break;
                    }
                }
            }

            if (itemAlreadyExists) { 
                bool itemFound = false;

                foreach (Transform shopSlot in _shopSlotsContainer)
                {
                    if (shopSlot.childCount > 0)
                    {
                        Transform shopSlotChild = shopSlot.GetChild(0);
                        DraggableItem draggableItem = shopSlotChild?.GetComponent<DraggableItem>();

                        if (draggableItem && draggableItem.ItemSO == _itemsForSale[randomIndexNum])
                        {
                            draggableItem.UpdateAmountLeft(Random.Range(1, 6));
                            itemFound = true;
                            break;
                        }
                    }
                }

                if (itemFound) { continue; }
            }

            foreach (Transform shopSlot in _shopSlotsContainer)
            {
                if (shopSlot.childCount == 0) {
                    DraggableItem newItem = Instantiate(_itemsForSale[randomIndexNum].IventoryPrefab, shopSlot.transform).GetComponent<DraggableItem>();
                    newItem.CurrentAmount = (Random.Range(1, 6));
                    newItem.UpdateAmountLeftText();
                    break;
                }
            }
        }

        _amountOfItemsToRefreshShop = 0;
    }

    private IEnumerator RefreshItemsRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(_shopRefreshTime);

            _amountOfItemsToRefreshShop++;

            if (_shopSlotsContainer.gameObject.activeInHierarchy) { 
                OpenBuilding();
            }
        }
    }
}
