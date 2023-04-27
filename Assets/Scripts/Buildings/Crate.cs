using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Singleton<Crate>
{
    [SerializeField] private Transform _crateContainerGridLayoutGroup;

    [SerializeField] private List<ItemSO> _itemsInCrate = new List<ItemSO>();

    private List<ItemSO> _itemsToRemove = new List<ItemSO>();

    public void PutItemInCrate(ItemSO itemSO) {
        _itemsInCrate.Add(itemSO);
    }

    public void RemoveItemFromCrate(ItemSO itemSO) {
        _itemsInCrate.Remove(itemSO);
    }

    public void SellItemsToCollector(ItemSO.ItemType itemType) {
        int amountOfGoldCollected = 0;

        foreach (ItemSO item in _itemsInCrate)
        {
            if (item.thisItemType == itemType) {
                amountOfGoldCollected+= item.ItemSellAmount;
                EconomyManager.Instance.UpdateCurrentCoinAmount(item.ItemSellAmount);
                _itemsToRemove.Add(item);
            }
        }


        foreach (Transform slot in _crateContainerGridLayoutGroup)
        {
            DraggableSlot draggableSlot = slot.GetComponent<DraggableSlot>();
            Transform slotTranform = draggableSlot?.transform;

            if (slotTranform.childCount > 0)
            {
                Transform itemInSlot = slotTranform?.GetChild(0);
                ItemSO itemSO = itemInSlot.GetComponent<DraggableItem>().ItemSO;

                if (itemInSlot && _itemsToRemove.Contains(itemSO))
                {
                    _itemsToRemove.Clear();
                    _itemsInCrate.RemoveAll(item => Object.ReferenceEquals(item, itemSO));
                    Destroy(itemInSlot.gameObject);
                }
            }
        }

        EventLogManager.Instance.NewEventLog("Resources from crate collected for a total of " + amountOfGoldCollected.ToString() + " gold.");
    }

   
}
