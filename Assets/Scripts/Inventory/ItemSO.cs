using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public GameObject IventoryPrefab;
    public int ItemCost = 2;
    public int ItemSellAmount = 3;
    public ItemType thisItemType;

    public enum ItemType {
        Veggie,
        Hardware,
        Seeds,
        Agriculture,
        Gather
    }
}
