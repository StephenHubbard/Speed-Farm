using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyLand : MonoBehaviour, IItem
{
    public void EquipItem()
    {
        InventoryManager.Instance.SetActiveEquippedItem(this);
        LandManager.Instance.ShowAvailableLandToBuy();
    }

    public void UseItem()
    {
        LandManager.Instance.BuyLand();
    }
}
