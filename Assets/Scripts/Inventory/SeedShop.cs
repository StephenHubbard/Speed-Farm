using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedShop : MonoBehaviour, IIBuilding
{
    [SerializeField] private int _amountOfDifferentSeedsToSpawn = 2;
    [SerializeField] private List<SeedTypeSO> _seedsForSale = new List<SeedTypeSO>();
    [SerializeField] private Transform _shopSlotsContainer;
    [SerializeField] private List<ShopSlot> _shopSlots = new List<ShopSlot>();

    private bool _hasBeenOpened = false;

    private void Awake() {
        foreach (Transform shopSlot in _shopSlotsContainer)
        {
            _shopSlots.Add(shopSlot.GetComponent<ShopSlot>());
        }
    }

    public void OpenBuilding()
    {
        if (_hasBeenOpened) { return; }

        _hasBeenOpened = true;
        
        AddSeedsToShop();
    }

    public void AddSeedsToShop() {
        for (int i = 0; i < _amountOfDifferentSeedsToSpawn; i++)
        {
            foreach (Transform shopSlot in _shopSlotsContainer)
            {
                if (shopSlot.childCount == 0) {
                    DraggableItem newItem = Instantiate(_seedsForSale[i].SeedInventoryPrefab, shopSlot.transform).GetComponent<DraggableItem>();
                    newItem.CurrentAmount = (Random.Range(1, 6));
                    newItem.UpdateAmountLeftText();
                    break;
                }
            }
        }
    }
}
