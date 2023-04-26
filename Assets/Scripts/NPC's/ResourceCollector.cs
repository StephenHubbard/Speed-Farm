using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour, INPC
{
    [SerializeField] private ItemSO.ItemType _itemType;

    public void Collect()
    {
        Crate.Instance.SellItemsToCollector(_itemType);
    }
}
