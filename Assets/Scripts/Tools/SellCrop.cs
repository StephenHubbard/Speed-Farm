using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellCrop : MonoBehaviour, IItem
{
    private Grid<GridGeneration.GridObject> _grid;

    private void Start() {
        _grid = GridGeneration.Instance.GetGrid();
    }

    public void EquipItem()
    {
        InventoryManager.Instance.SetActiveEquippedItem(this);
    }

    public void UseItem()
    {
        List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

        foreach (Vector3Int selectedTile in selectedTiles)
        {
            PlacedObject_Done placedObject = _grid.GetGridObject(selectedTile).PlacedObject;
            Crop crop = placedObject?.GetComponent<Crop>();

            if (placedObject != null && crop && crop.IsFullyGrown)
            {
                PlacedObjectTypeSO placedObjectTypeSO = crop.PlacedObjectTypeSO;
                ItemSO itemSO = placedObjectTypeSO.ItemSO;

                Backpack.Instance.AddItemToBackpackDumpList(itemSO);

                placedObject.DestroySelf();

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    _grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }
    }

   
}
