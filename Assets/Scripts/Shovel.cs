using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour, Item
{
    public void UseItem()
    {
        InventoryManager.Instance.ClickShovelIcon();
    }
}
