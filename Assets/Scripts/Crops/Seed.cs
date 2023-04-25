using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Seed : MonoBehaviour, IItem
{
    public PlacedObjectTypeSO PlacedObjectTypeSO => _placedObjectTypeSO;

    [SerializeField] private PlacedObjectTypeSO _placedObjectTypeSO;
   
    private Grid<GridGeneration.GridObject> _grid;
    private DraggableItem _draggableItem;

    private void Awake() {
        _draggableItem = GetComponent<DraggableItem>();
    }

    private void Start() {
        _grid = GridGeneration.Instance.GetGrid();
    }

    public void EquipItem()
    {
        InventoryManager.Instance.SetActiveEquippedItem(this);
    }

    public void UseItem() {
        List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

        List<Vector3Int> validDirtTiles = new List<Vector3Int>();

        foreach (Vector3Int selectedTile in selectedTiles)
        {
            // saving build check for if later on I have 2x2 crop trees or something like that

            _grid.GetXY(selectedTile, out int x, out int y);

            Vector2Int placedObjectOrigin = new Vector2Int(x, y);
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            List<Vector2Int> gridPositionList = _placedObjectTypeSO.GetGridPositionList(placedObjectOrigin);

            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!_grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild && GridGeneration.Instance.GrassTilemap.GetTile(tilePosition) == GridGeneration.Instance.DirtTile)
            {
                validDirtTiles.Add(selectedTile);
            }
        }

        if (EnoughSeeds(validDirtTiles.Count))
        {
            Debug.Log("Not enough seeds!");
            return;
        }

        foreach (Vector3Int validDirtTile in validDirtTiles)
        {
            Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(validDirtTile.x, validDirtTile.y) * _grid.GetCellSize();
            
            _grid.GetXY(validDirtTile, out int x, out int y);

            Vector2Int placedObjectOrigin = new Vector2Int(x, y);

            PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, _placedObjectTypeSO);

            List<Vector2Int> gridPositionList = _placedObjectTypeSO.GetGridPositionList(placedObjectOrigin);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                _grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            _draggableItem.UseItem();
        }
    }

    private bool EnoughSeeds(int amountOfPlotsSelected) {
        if (amountOfPlotsSelected <= _draggableItem.CurrentAmount) {
            return false;
        } else {
            return true;
        }
    }

}
