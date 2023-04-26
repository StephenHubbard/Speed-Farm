using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{
    [SerializeField] private List<ItemSO> _itemsPackContains = new List<ItemSO>();

    public void BuyPack() {
        int randomAmountOfResourcesToAdd = Random.Range(4, 8);

        for (int i = 0; i < randomAmountOfResourcesToAdd; i++)
        {
            int randomResourceIndex = Random.Range(0, _itemsPackContains.Count);

            PackManager.Instance.AddResourceToCollect(_itemsPackContains[randomResourceIndex]);
        }
    }
}
