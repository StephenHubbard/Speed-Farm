using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private GameObject _showAvailableLandToBuyPrefab;
    [SerializeField] private Sprite _fenceBaseSprite;
    [SerializeField] private Sprite _fenceUpSprite;
    [SerializeField] private Sprite _fenceRightSprite;
    [SerializeField] private Sprite _fenceLeftSprite;
    [SerializeField] private Sprite _fenceLeftAndRightSprite;
    [SerializeField] private Sprite _fenceUpAndRightSprite;
    [SerializeField] private Sprite _fenceUpAndLeftSprite;
    [SerializeField] private Sprite _fenceLeftRightUpSprite;
    [SerializeField] private PlacedObjectTypeSO _fence_PlacedObjectTypeSO;

    private Vector3Int _tilePos;
    private SpriteRenderer _spriteRenderer;


    private void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        UpdateFenceCoordVector3Int();
        UpdateFenceSprite(true);
        CanBuyAdjLand();
    }

    private void CanBuyAdjLand() {
        var grid = GridGeneration.Instance.GetGrid();

        grid.GetGridObject(_tilePos).canBuyLand = true;

        if (LandManager.Instance.BuyLandToggledOn) {
            // GameObject showLandSprite = grid.GetGridObject(_tilePos).GetBuyLandSprite();
            // showLandSprite.GetComponentInChildren<SpriteRenderer>().color = new Color(greenColor.r, greenColor.g, greenColor.b, greenColor.a);

            if (grid.GetGridObject(_tilePos).buyLandSprite == null) {
                Color greenColor = LandManager.Instance.GreenColor;
                GameObject showLandSpritePrefab = Instantiate(_showAvailableLandToBuyPrefab, new Vector2(_tilePos.x, _tilePos.y), Quaternion.identity);
                LandManager.Instance.AllShowLandSprites.Add(showLandSpritePrefab);
                grid.GetGridObject(_tilePos).SetBuyLandSprite(showLandSpritePrefab);
                showLandSpritePrefab.GetComponentInChildren<SpriteRenderer>().color = new Color(greenColor.r, greenColor.g, greenColor.b, greenColor.a);
            }
        }
    }

    public void UpdateFenceCoordVector3Int() {
        PlacedObject_Done placedObject_Done = GetComponent<PlacedObject_Done>();
        _tilePos = new Vector3Int(placedObject_Done.Origin.x, placedObject_Done.Origin.y, 0);
    }

    public void UpdateFenceSprite(bool updateAjdTile) {
        var grid = GridGeneration.Instance.GetGrid();

        List<Vector3Int> adjacentTiles = GetAdjacentTiles(_tilePos);

        PlacedObject_Done dirRight = grid.GetGridObject(adjacentTiles[0])?.placedObject;
        PlacedObject_Done dirLeft = grid.GetGridObject(adjacentTiles[1])?.placedObject;
        PlacedObject_Done dirUp = grid.GetGridObject(adjacentTiles[2])?.placedObject;
        PlacedObject_Done dirDown = grid.GetGridObject(adjacentTiles[3])?.placedObject;

        _spriteRenderer.sprite = _fenceBaseSprite;

        if (HasFenceInDirection(dirRight)) {
            _spriteRenderer.sprite = _fenceRightSprite;
        }

        if (HasFenceInDirection(dirLeft))
        {
            _spriteRenderer.sprite = _fenceLeftSprite;
        }

        if (HasFenceInDirection(dirUp))
        {
            _spriteRenderer.sprite = _fenceUpSprite;
        }

        if (HasFenceInDirection(dirRight) && HasFenceInDirection(dirLeft)) 
        {
            _spriteRenderer.sprite = _fenceLeftAndRightSprite;

            if (updateAjdTile) { 
                dirLeft.GetComponent<Fence>().UpdateFenceSprite(false);
                dirRight.GetComponent<Fence>().UpdateFenceSprite(false);
            }
        }

        if (HasFenceInDirection(dirRight) && HasFenceInDirection(dirUp))
        {
            _spriteRenderer.sprite = _fenceUpAndRightSprite;

            if (updateAjdTile)
            {
                dirUp.GetComponent<Fence>().UpdateFenceSprite(false);
                dirRight.GetComponent<Fence>().UpdateFenceSprite(false);
            }
        }

        if (HasFenceInDirection(dirLeft) && HasFenceInDirection(dirUp))
        {
            _spriteRenderer.sprite = _fenceUpAndLeftSprite;

            if (updateAjdTile)
            {
                dirLeft.GetComponent<Fence>().UpdateFenceSprite(false);
                dirUp.GetComponent<Fence>().UpdateFenceSprite(false);
            }
        }

        if (HasFenceInDirection(dirLeft) && HasFenceInDirection(dirUp) && HasFenceInDirection(dirRight))
        {
            _spriteRenderer.sprite = _fenceLeftRightUpSprite;

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
        if (placedObject_Done && placedObject_Done.PlacedObjectTypeSO == _fence_PlacedObjectTypeSO)
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
