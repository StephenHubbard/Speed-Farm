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

    private Vector3Int tilePos;
    private SpriteRenderer spriteRenderer;


    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        UpdateFenceCoordVector3Int();
        UpdateFenceSprite(true);
        CanBuyAdjLand();
    }

    private void CanBuyAdjLand() {
        var grid = GridGeneration.Instance.GetGrid();

        grid.GetGridObject(tilePos).canBuyLand = true;

        if (LandManager.Instance.BuyLandToggledOn) {
            GameObject showLandSprite = grid.GetGridObject(tilePos).GetBuyLandSprite();
            Color greenColor = LandManager.Instance.GreenColor;
            showLandSprite.GetComponentInChildren<SpriteRenderer>().color = new Color(greenColor.r, greenColor.g, greenColor.b, greenColor.a);
        }
    }

    public void UpdateFenceCoordVector3Int() {
        PlacedObject_Done placedObject_Done = GetComponent<PlacedObject_Done>();
        tilePos = new Vector3Int(placedObject_Done.Origin.x, placedObject_Done.Origin.y, 0);
    }

    public void UpdateFenceSprite(bool updateAjdTile) {
        var grid = GridGeneration.Instance.GetGrid();

        List<Vector3Int> adjacentTiles = GetAdjacentTiles(tilePos);

        PlacedObject_Done dirRight = grid.GetGridObject(adjacentTiles[0])?.placedObject;
        PlacedObject_Done dirLeft = grid.GetGridObject(adjacentTiles[1])?.placedObject;
        PlacedObject_Done dirUp = grid.GetGridObject(adjacentTiles[2])?.placedObject;
        PlacedObject_Done dirDown = grid.GetGridObject(adjacentTiles[3])?.placedObject;

        spriteRenderer.sprite = fenceBaseSprite;

        if (HasFenceInDirection(dirRight)) {
            spriteRenderer.sprite = fenceRightSprite;
        }

        if (HasFenceInDirection(dirLeft))
        {
            spriteRenderer.sprite = fenceLeftSprite;
        }

        if (HasFenceInDirection(dirUp))
        {
            spriteRenderer.sprite = fenceUpSprite;
        }

        if (HasFenceInDirection(dirRight) && HasFenceInDirection(dirLeft)) 
        {
            spriteRenderer.sprite = fenceLeftAndRightSprite;

            if (updateAjdTile) { 
                dirLeft.GetComponent<Fence>().UpdateFenceSprite(false);
                dirRight.GetComponent<Fence>().UpdateFenceSprite(false);
            }
        }

        if (HasFenceInDirection(dirRight) && HasFenceInDirection(dirUp))
        {
            spriteRenderer.sprite = fenceUpAndRightSprite;

            if (updateAjdTile)
            {
                dirUp.GetComponent<Fence>().UpdateFenceSprite(false);
                dirRight.GetComponent<Fence>().UpdateFenceSprite(false);
            }
        }

        if (HasFenceInDirection(dirLeft) && HasFenceInDirection(dirUp))
        {
            spriteRenderer.sprite = fenceUpAndLeftSprite;

            if (updateAjdTile)
            {
                dirLeft.GetComponent<Fence>().UpdateFenceSprite(false);
                dirUp.GetComponent<Fence>().UpdateFenceSprite(false);
            }
        }

        if (HasFenceInDirection(dirLeft) && HasFenceInDirection(dirUp) && HasFenceInDirection(dirRight))
        {
            spriteRenderer.sprite = fenceLeftRightUpSprite;

            if (updateAjdTile)
            {
                dirLeft.GetComponent<Fence>().UpdateFenceSprite(false);
                dirUp.GetComponent<Fence>().UpdateFenceSprite(false);
                dirRight.GetComponent<Fence>().UpdateFenceSprite(false);
            }
        }

        // if (HasFenceInDirection(dirDown) && HasFenceInDirection(dirUp))
        // {
        //     if (updateAjdTile)
        //     {
        //         dirDown.GetComponent<Fence>().UpdateFenceSprite(false);
        //         dirUp.GetComponent<Fence>().UpdateFenceSprite(false);
        //     }
        // }
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
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y + 1, 0)); // up right
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y - 1, 0)); // down right
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y + 1, 0)); // up left
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y - 1, 0)); // down right

        return adjacentTilePositions;
    }
}
