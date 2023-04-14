using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WeedManager : MonoBehaviour
{
    
    [SerializeField] private PlacedObjectTypeSO weedSO;
    [Range(0, 100)]
    [SerializeField] private int weedChance;
    [SerializeField] private PlacedObjectTypeSO rockSO;
    [Range(0, 100)]
    [SerializeField] private int rockChance;
    [SerializeField] private PlacedObjectTypeSO treeSO;
    [Range(0, 100)]
    [SerializeField] private int treeChance;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tile[] grassTiles;

    private PlacedObjectTypeSO.Dir dir;

    private void Start() {
        SpawnResource(treeSO, treeChance);
        SpawnResource(weedSO, weedChance);
        SpawnResource(rockSO, rockChance);
    }

    private void SpawnResource(PlacedObjectTypeSO placedObjectTypeSO, int spawnModifier)
    {
        var grid = GridGeneration.Instance.GetGrid();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetWidth(); y++)
            {
                Vector2Int placedObjectOrigin = new Vector2Int(x, y);
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                bool isGrassTile = false;

                foreach (Tile tile in grassTiles)
                {
                    if (grassTilemap.GetTile(tilePosition) == tile)
                    {
                        isGrassTile = true;
                    }
                }

                int randomSpawnNum = Random.Range(1, 101);

                if (randomSpawnNum >= spawnModifier || !isGrassTile) { continue; }

                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

                if (grid.GetGridObject(tilePosition).CanBuild()) {
                    PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
                    placedObject.transform.rotation = Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));

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
