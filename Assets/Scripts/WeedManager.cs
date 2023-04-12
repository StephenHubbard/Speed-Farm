using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WeedManager : MonoBehaviour
{
    [SerializeField] private PlacedObjectTypeSO weedSO;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tile[] grassTiles;

    private PlacedObjectTypeSO.Dir dir;

    private void Start() {
        SpawnStartingWeeds();
    }

    private void SpawnStartingWeeds() {
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

                int randomNum = Random.Range(0, 10);

                if (randomNum < 8 || !isGrassTile) { continue; }

                List<Vector2Int> gridPositionList = weedSO.GetGridPositionList(placedObjectOrigin, dir);

                Vector2Int rotationOffset = weedSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

                PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, weedSO);
                Crop crop = placedObject.GetComponent<Crop>();
                crop.StraightToFullyGrown();
                placedObject.transform.rotation = Quaternion.Euler(0, 0, -weedSO.GetRotationAngle(dir));

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }

            }
        }
    }
}
