using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private int _amountOfStartingItems = 2;
    [SerializeField] private float _shopRefreshTime = 10f;
    [SerializeField] private List<ItemSO> _itemsForSale = new List<ItemSO>();
    [SerializeField] private Transform _shopSlotsContainer;
    
    private List<ShopSlot> _shopSlots = new List<ShopSlot>();

    private Coroutine _refreshItemsCoroutine;

    private void Awake() {
        foreach (Transform shopSlot in _shopSlotsContainer)
        {
            _shopSlots.Add(shopSlot.GetComponent<ShopSlot>());
        }
    }

    private void Start() {
        AddItemsToShop(_amountOfStartingItems);

        _refreshItemsCoroutine = StartCoroutine(RefreshItemsRoutine());
    }

    private void AddItemsToShop(int amountOfItemsToAdd) {
        for (int i = 0; i < amountOfItemsToAdd; i++)
        {
            bool itemExists = false;

            int randomIndexNum = Random.Range(0, _itemsForSale.Count);

            foreach (Transform shopSlot in _shopSlotsContainer)
            {
                if (shopSlot.childCount > 0) {
                    Transform shopSlotChild = shopSlot.GetChild(0);
                    DraggableItem draggableItem = shopSlotChild?.GetComponent<DraggableItem>();
                    
                    if (draggableItem && draggableItem.ItemSO == _itemsForSale[randomIndexNum]) {
                        draggableItem.UpdateAmountLeft(Random.Range(1, 3));
                        itemExists = true;
                        break;
                    }
                }
            }

            if (itemExists) { continue; }

            foreach (Transform shopSlot in _shopSlotsContainer)
            {
                if (shopSlot.childCount == 0) {
                    DraggableItem newItem = Instantiate(_itemsForSale[randomIndexNum].IventoryPrefab, shopSlot.transform).GetComponent<DraggableItem>();
                    newItem.UpdateAmountLeft(Random.Range(1, 3));
                    break;
                }
            }
        }

    }

    private IEnumerator RefreshItemsRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(_shopRefreshTime);

            AddItemsToShop(1);
        }
    }
}
