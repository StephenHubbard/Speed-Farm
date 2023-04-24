using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Shovel : MonoBehaviour, IItem
{
    [SerializeField] private Tilemap _grassTilemap;
    [SerializeField] private List<Tile> _grassTiles = new List<Tile>();
    [SerializeField] private Tile _dirtTile;

    private Grid<GridGeneration.GridObject> _grid;

    private void Start()
    {
        _grid = GridGeneration.Instance.GetGrid();
    }

    public void EquipItem()
    {
        InventoryManager.Instance.SetActiveEquippedItem(this);
    }

    public void UseItem()
    {
        List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

        List<Vector3Int> ownedTiles = new List<Vector3Int>();

        foreach (Vector3Int selectedTile in selectedTiles)
        {
            if (_grid.GetGridObject(selectedTile).OwnsLand)
            {
                ownedTiles.Add(selectedTile);
            }
        }

        bool anyTilesAreGrass = false;

        foreach (Vector3Int ownedTile in ownedTiles)
        {
            if (_grassTiles.Contains((Tile)_grassTilemap.GetTile(ownedTile)))
            {
                anyTilesAreGrass = true;
                break;
            }
        }

        foreach (Vector3Int selectedTile in selectedTiles)
        {
            PlacedObject_Done placedObject = _grid.GetGridObject(selectedTile).PlacedObject;
            bool doesOwnLand = _grid.GetGridObject(selectedTile).OwnsLand;
            if (!placedObject && doesOwnLand)
            {

                if (anyTilesAreGrass)
                {
                    _grassTilemap.SetTile(selectedTile, _dirtTile);
                    continue;
                }
                else
                {
                    if ((selectedTile.x + selectedTile.y) % 2 == 0)
                    {
                        _grassTilemap.SetTile(selectedTile, _grassTiles[1]);
                    }
                    else
                    {
                        _grassTilemap.SetTile(selectedTile, _grassTiles[0]);
                    }
                }
            }
        }
    }
}
