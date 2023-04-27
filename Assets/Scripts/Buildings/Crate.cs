using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Singleton<Crate>
{
    public List<CrateSlot> AllCrateSlots => _allCrateSlots;
    private List<CrateSlot> _allCrateSlots = new List<CrateSlot>();

    [SerializeField] private Transform _crateContainerGridLayoutGroup;

    protected override void Awake() {
        base.Awake();

        foreach (Transform crateSlot in _crateContainerGridLayoutGroup)
        {
            _allCrateSlots.Add(crateSlot.GetComponent<CrateSlot>());
        }
    }

    public void SellItemsToCollector(ItemSO.ItemType itemType) {

        int amountOfGoldCollected = 0;

        foreach (Transform slot in _crateContainerGridLayoutGroup)
        {
            DraggableSlot draggableSlot = slot.GetComponent<DraggableSlot>();
            Transform slotTranform = draggableSlot?.transform;

            if (slotTranform.childCount > 0)
            {
                Transform itemInSlot = slotTranform?.GetChild(0);
                ItemSO itemSO = itemInSlot.GetComponent<DraggableItem>().ItemSO;

                if (itemSO.thisItemType == itemType)
                {
                    int amountInSlot = itemInSlot.GetComponent<DraggableItem>().CurrentAmount;
                    amountOfGoldCollected += (itemSO.ItemSellAmount * amountInSlot);
                    Destroy(itemInSlot.gameObject);
                }
            }
        }

        EventLogManager.Instance.NewEventLog("Resources from crate collected for a total of " + amountOfGoldCollected.ToString() + " gold.");
    }

   
}
