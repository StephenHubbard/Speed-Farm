using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SeedTypeSO : ScriptableObject
{
    public GameObject SeedInventoryPrefab;
    public PlacedObjectTypeSO PlacedObjectTypeSO;
    public int SeedCost = 2;
}
