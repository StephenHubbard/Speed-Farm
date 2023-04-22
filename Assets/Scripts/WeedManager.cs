using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WeedManager : MonoBehaviour
{

    [SerializeField] private PlacedObjectTypeSO _weedSO;
    [Range(0, 100)]
    [SerializeField] private int _weedChance;
    [SerializeField] private PlacedObjectTypeSO _rockSO;
    [Range(0, 100)]
    [SerializeField] private int _rockChance;
    [SerializeField] private PlacedObjectTypeSO _treeSO;
    [Range(0, 100)]
    [SerializeField] private int _treeChance;
    [SerializeField] private Tilemap _grassTilemap;
    [SerializeField] private Tile _dirtTile;
    [SerializeField] private List<TileBase> _grassTilesList = new List<TileBase>();

    private Grid<GridGeneration.GridObject> _grid;

    private void Start()
    {
        _grid = GridGeneration.Instance.GetGrid();

        SpawnResource(_treeSO, _treeChance);
        SpawnResource(_weedSO, _weedChance);
        SpawnResource(_rockSO, _rockChance);
    }

    private void SpawnResource(PlacedObjectTypeSO placedObjectTypeSO, int spawnModifier)
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);

                Vector2Int placedObjectOrigin = new Vector2Int(tilePos.x, tilePos.y);

                TileBase thisTile = _grassTilemap.GetTile(tilePos);

                int randomSpawnNum = Random.Range(0, 100);

                if (randomSpawnNum >= spawnModifier || !_grassTilesList.Contains(thisTile)) { continue; }

                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin);

                bool canBuild = true;

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    if (!_grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                        canBuild = false;
                        break;
                    }
                }
                
                if (canBuild)
                {
                    PlacedObject_Done placedObject = PlacedObject_Done.Create(tilePos, placedObjectOrigin, placedObjectTypeSO);

                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        _grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }
                }
            }
        }
    }
}