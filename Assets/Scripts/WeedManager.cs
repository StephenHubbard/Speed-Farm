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

    private PlacedObjectTypeSO.Dir _dir;

    private void Start()
    {
        SpawnResource(_treeSO, _treeChance);
        SpawnResource(_weedSO, _weedChance);
        SpawnResource(_rockSO, _rockChance);
        LandManager.Instance.DetermineStartingLandOwnership();
    }

    private void SpawnResource(PlacedObjectTypeSO placedObjectTypeSO, int spawnModifier)
    {
        var grid = GridGeneration.Instance.GetGrid();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector2Int placedObjectOrigin = new Vector2Int(x, y);
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, _dir);

                bool allGrassTiles = false;

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    allGrassTiles = false;

                    Vector3Int checkAdjTilePos = new Vector3Int(gridPosition.x, gridPosition.y, 0);

                    TileBase thisTile = _grassTilemap.GetTile(checkAdjTilePos);

                    if (_grassTilesList.Contains(thisTile)) {
                        allGrassTiles = true;
                    } else {
                        allGrassTiles = false;
                        break;
                    }
                   
                }

                int randomSpawnNum = Random.Range(0, 100);

                if (randomSpawnNum >= spawnModifier || !allGrassTiles) { continue; }

                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(_dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

                if (grid.GetGridObject(tilePosition).CanBuild())
                {
                    PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, _dir, placedObjectTypeSO);
                    placedObject.transform.rotation = Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(_dir));

                    Crop crop = placedObject.GetComponent<Crop>();
                    crop?.StraightToFullyGrown();

                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }
                }
            }
        }
    }
}