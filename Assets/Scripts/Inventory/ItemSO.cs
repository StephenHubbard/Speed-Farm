using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public GameObject IventoryPrefab;
    public Sprite resourceSprite;
    public int ItemCost = 2;
    public int ItemSellAmount = 3;
    public ItemType thisItemType;

    public enum ItemType {
        VegType,
        Hardware,
        Seeds,
        AgriType,
        GatherType
    }
}
