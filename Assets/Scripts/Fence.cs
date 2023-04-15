using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private Sprite fenceBaseSprite;
    [SerializeField] private Sprite fenceUpSprite;
    [SerializeField] private Sprite fenceRightSprite;
    [SerializeField] private Sprite fenceLeftSprite;
    [SerializeField] private Sprite fenceLeftAndRightSprite;
    [SerializeField] private Sprite fenceUpAndRightSprite;
    [SerializeField] private Sprite fenceUpAndLeftSprite;
    [SerializeField] private Sprite fenceLeftRightUpSprite;
    [SerializeField] private PlacedObjectTypeSO fence_PlacedObjectTypeSO;

    private Vector3Int thisTilePos;
    private SpriteRenderer spriteRenderer;


    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        UpdateFenceCoordVector3Int();
        UpdateFenceSprite();
    }

    public void UpdateFenceCoordVector3Int() {
        PlacedObject_Done placedObject_Done = GetComponent<PlacedObject_Done>();
        thisTilePos = new Vector3Int(placedObject_Done.Origin.x, placedObject_Done.Origin.y, 0);
    }

    public void UpdateFenceSprite() {
        var grid = GridGeneration.Instance.GetGrid();

        List<Vector3Int> adjacentTiles = GetAdjacentTiles(thisTilePos);

        PlacedObject_Done dirRight = grid.GetGridObject(adjacentTiles[0]).placedObject;
        PlacedObject_Done dirLeft = grid.GetGridObject(adjacentTiles[1]).placedObject;
        PlacedObject_Done dirUp = grid.GetGridObject(adjacentTiles[2]).placedObject;
        PlacedObject_Done dirDown = grid.GetGridObject(adjacentTiles[3]).placedObject;

        spriteRenderer.sprite = fenceBaseSprite;

        if (HasFenceInDirection(dirRight)) {
            spriteRenderer.sprite = fenceRightSprite;
        }

        if (HasFenceInDirection(dirLeft))
        {
            spriteRenderer.sprite = fenceLeftSprite;
            Vector3Int twoTilesLeft = new Vector3Int(thisTilePos.x - 2, thisTilePos.y, thisTilePos.z);
            PlacedObject_Done twoLeftPlacedObject = grid.GetGridObject(twoTilesLeft).placedObject;
            twoLeftPlacedObject?.GetComponent<Fence>().UpdateFenceSprite();
        }

        if (HasFenceInDirection(dirUp))
        {
            spriteRenderer.sprite = fenceUpSprite;
        }

        if (HasFenceInDirection(dirRight) && HasFenceInDirection(dirLeft)) 
        {
            spriteRenderer.sprite = fenceLeftAndRightSprite;
        }

        if (HasFenceInDirection(dirRight) && HasFenceInDirection(dirUp))
        {
            spriteRenderer.sprite = fenceUpAndRightSprite;
        }

        if (HasFenceInDirection(dirLeft) && HasFenceInDirection(dirUp))
        {
            spriteRenderer.sprite = fenceUpAndLeftSprite;
        }

        if (HasFenceInDirection(dirLeft) && HasFenceInDirection(dirUp) && HasFenceInDirection(dirRight))
        {
            spriteRenderer.sprite = fenceLeftRightUpSprite;
        }
    }

    private bool HasFenceInDirection(PlacedObject_Done placedObject_Done) {
        if (placedObject_Done && placedObject_Done.PlacedObjectTypeSO == fence_PlacedObjectTypeSO)
        {
            return true;
        } else {
            return false;
        }
    }

    private List<Vector3Int> GetAdjacentTiles(Vector3Int tilePosition)
    {
        List<Vector3Int> adjacentTilePositions = new List<Vector3Int>
        {
            new Vector3Int(tilePosition.x + 1, tilePosition.y, 0), // right
            new Vector3Int(tilePosition.x - 1, tilePosition.y, 0), // left
            new Vector3Int(tilePosition.x, tilePosition.y + 1, 0), // up
            new Vector3Int(tilePosition.x, tilePosition.y - 1, 0)  // down
        };

        // You can decide to include diagonal adjacent tiles by commenting or uncommenting the following lines
        // adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y + 1, 0)); // up right
        // adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y - 1, 0)); // down right
        // adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y + 1, 0)); // up left
        // adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y - 1, 0)); // down right

        return adjacentTilePositions;
    }
}
