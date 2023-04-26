using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrateSlot : MonoBehaviour
{
    // Define a custom event for when a new child is added
    public event Action<Transform> OnChildAdded;

    private int childCount;

    private void Start()
    {
        childCount = transform.childCount;
    }

    public void ItemRemoved(ItemSO itemSO, int amount) {
        for (int i = 0; i < amount; i++)
        {
            Crate.Instance.RemoveItemFromCrate(itemSO);
        }
    }

    // This method is called whenever a child is added or removed
    protected virtual void OnTransformChildrenChanged()
    {
        // Check if a new child has been added
        if (transform.childCount > childCount)
        {
            // Get the newly added child Transform
            Transform newChild = transform.GetChild(childCount);

            // Invoke the OnChildAdded event
            OnChildAdded?.Invoke(newChild);

            // Perform desired actions with the new child Transform
            DraggableItem draggableItem = transform.GetChild(0).GetComponent<DraggableItem>();
            ItemSO itemSO = draggableItem.ItemSO;

            for (int i = 0; i < draggableItem.CurrentAmount; i++)
            {
                Crate.Instance.PutItemInCrate(itemSO);
            }
        }

        // Update the child count
        childCount = transform.childCount;
    }
}
