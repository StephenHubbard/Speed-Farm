using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;


public class LandManager : Singleton<LandManager>
{
    public bool BuyLandToggledOn { get; private set; }
    public Color GreenColor { get { return greenColor; } }
    public Color RedColor { get { return redColor; } }

    [SerializeField] private Tilemap grassTilemap;
    // [SerializeField] private Tilemap fenceTilemap;
    [SerializeField] private Tile dirtTile;
    // [SerializeField] private RuleTile fenceTile;
    [SerializeField] private PlacedObjectTypeSO fenceSO;
    [SerializeField] private GameObject showAvailableLandToBuyPrefab;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;

    private List<GameObject> allShowLandSprites = new List<GameObject>();

    private void Start() {
        BuyLandToggleFalse();
    }

    private void Update() {
        BuyLand();
    }

    public void BuyLandToggleTrue() {
        BuyLandToggledOn = true;
    }

    public void BuyLandToggleFalse()
    {
        BuyLandToggledOn = false;
    }

    private void BuyLand() {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.GetMouseButtonUp(0) && BuyLandToggledOn) {
            var grid = GridGeneration.Instance.GetGrid();

            List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

            foreach (Vector3Int selectedTile in selectedTiles)
            {
                if (grid.GetGridObject(selectedTile).ownsLand || !grid.GetGridObject(selectedTile).canBuyLand) { return ;}

                grid.GetGridObject(selectedTile).BuyLand();

                List<Vector3Int> adjacentTiles = GetAdjacentTiles(selectedTile);

                foreach (Vector3Int adjTile in adjacentTiles)
                {
                    if (!grid.GetGridObject(adjTile).ownsLand)
                    {
                        PlacedObject_Done currentPlacedObject = grid.GetGridObject(adjTile).GetPlacedObject();
                        PlacedObjectTypeSO placedObjectTypeSO = currentPlacedObject?.PlacedObjectTypeSO;
                        List<Vector2Int> gridPositionList = currentPlacedObject?.GetGridPositionList();
                        currentPlacedObject?.DestroySelf();

                        if (currentPlacedObject) {
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                            }
                        }

                        Vector2Int adjPlacedObjOrigin = new Vector2Int(adjTile.x, adjTile.y);
                        PlacedObject_Done adjPlacedObj = PlacedObject_Done.Create(adjTile, adjPlacedObjOrigin, PlacedObjectTypeSO.Dir.Down, fenceSO);
                        grid.GetGridObject(adjTile).SetPlacedObject(adjPlacedObj);
                    }
                }

                DetectProperFenceDisplay(selectedTile);
            }

        }
    }

    private void DetectProperFenceDisplay(Vector3Int vector3Int) {
        var grid = GridGeneration.Instance.GetGrid();

        PlacedObject_Done currentPlacedObject = grid.GetGridObject(vector3Int).GetPlacedObject();

        currentPlacedObject?.DestroySelf();

        List<Vector2Int> gridPositionList = currentPlacedObject?.GetGridPositionList();

        if (currentPlacedObject) {
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }

    }

    public void ShowAvailableLandToBuy() {
        var grid = GridGeneration.Instance.GetGrid();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (!grid.GetGridObject(tilePosition).ownsLand) {
                   GameObject showLandSpritePrefab = Instantiate(showAvailableLandToBuyPrefab, new Vector2(x, y), Quaternion.identity);
                   allShowLandSprites.Add(showLandSpritePrefab);
                   grid.GetGridObject(tilePosition).SetBuyLandSprite(showLandSpritePrefab);
                }

                if (!grid.GetGridObject(tilePosition).ownsLand)
                {
                    if (!grid.GetGridObject(tilePosition).canBuyLand)
                    {
                        GameObject showLandSprite = grid.GetGridObject(tilePosition).GetBuyLandSprite();
                        showLandSprite.GetComponentInChildren<SpriteRenderer>().color = new Color(redColor.r, redColor.g, redColor.b, redColor.a);
                    } else {
                        GameObject showLandSprite = grid.GetGridObject(tilePosition).GetBuyLandSprite();
                        showLandSprite.GetComponentInChildren<SpriteRenderer>().color = new Color(greenColor.r, greenColor.g, greenColor.b, greenColor.a);
                    }
                }
            }
        }
    }



    public void HideAvailableLandToBuy() {
        if (allShowLandSprites.Count == 0) { return; }

        foreach (GameObject showLandSprite in allShowLandSprites)
        {
            Destroy(showLandSprite);
        }

        BuyLandToggleFalse();
    }

    public void DetermineStartingLandOwnership() {
        var grid = GridGeneration.Instance.GetGrid();
        List<Vector3Int> startingTiles = new List<Vector3Int>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (grassTilemap.GetTile(tilePosition) == dirtTile) {
                    grid.GetGridObject(tilePosition).ownsLand = true;

                    startingTiles.Add(tilePosition);

                    if (grassTilemap.GetTile(tilePosition) != dirtTile) { 
                        Vector2Int placedObjectOrigin = new Vector2Int(tilePosition.x, tilePosition.y);
                        PlacedObject_Done placedObject = PlacedObject_Done.Create(tilePosition, placedObjectOrigin, PlacedObjectTypeSO.Dir.Down, fenceSO);
                        grid.GetGridObject(tilePosition.x, tilePosition.y).SetPlacedObject(placedObject);
                    }

                    List<Vector3Int> adjacentTiles = GetAdjacentTiles(tilePosition);

                    foreach (Vector3Int adjTile in adjacentTiles)
                    {
                        PlacedObject_Done currentPlacedObject = grid.GetGridObject(adjTile).GetPlacedObject();
                        PlacedObjectTypeSO placedObjectTypeSO = currentPlacedObject?.PlacedObjectTypeSO;

                        if (currentPlacedObject && placedObjectTypeSO != fenceSO)
                        {
                            currentPlacedObject.DestroySelf();

                            List<Vector2Int> gridPositionList = currentPlacedObject.GetGridPositionList();

                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                            }
                        }

                        if (grid.GetGridObject(adjTile.x, adjTile.y).CanBuild() && grassTilemap.GetTile(adjTile) != dirtTile) {
                            Vector2Int adjPlacedObjOrigin = new Vector2Int(adjTile.x, adjTile.y);
                            PlacedObject_Done adjPlacedObj = PlacedObject_Done.Create(adjTile, adjPlacedObjOrigin, PlacedObjectTypeSO.Dir.Down, fenceSO);
                            grid.GetGridObject(adjTile.x, adjTile.y).SetPlacedObject(adjPlacedObj);
                        }
                    }
                }
            }
        }

        // StartingFenceDisplay(startingTiles);
    }

    private void StartingFenceDisplay(List<Vector3Int> startingTiles) {
        var grid = GridGeneration.Instance.GetGrid();

        foreach (Vector3Int tile in startingTiles)
        {
            PlacedObject_Done currentPlacedObject = grid.GetGridObject(tile).GetPlacedObject();
            currentPlacedObject.DestroySelf();
            grid.GetGridObject(tile).ClearPlacedObject();
        }
    }

    private List<Vector3Int> GetAdjacentTiles(Vector3Int tilePosition)
    {
        List<Vector3Int> adjacentTilePositions = new List<Vector3Int>
        {
            new Vector3Int(tilePosition.x + 1, tilePosition.y, tilePosition.z),
            new Vector3Int(tilePosition.x - 1, tilePosition.y, tilePosition.z),
            new Vector3Int(tilePosition.x, tilePosition.y + 1, tilePosition.z),
            new Vector3Int(tilePosition.x, tilePosition.y - 1, tilePosition.z)
        };

        // You can decide to include diagonal adjacent tiles by commenting or uncommenting the following lines
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y + 1, tilePosition.z));
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y - 1, tilePosition.z));
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y + 1, tilePosition.z));
        adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y - 1, tilePosition.z));

        return adjacentTilePositions;
    }

    public List<Vector3Int> GetAdjacentTilesDistance(Vector3Int tilePosition, int distance)
    {
        List<Vector3Int> adjacentTilePositions = new List<Vector3Int>();

        for (int x = -distance; x <= distance; x++)
        {
            for (int y = -distance; y <= distance; y++)
            {
                // Skip the center tile and diagonal tiles when distance is 1
                if (distance == 1 && (x == 0 || y == 0 || Mathf.Abs(x) == Mathf.Abs(y)))
                {
                    continue;
                }

                // Skip the center tile
                if (x == 0 && y == 0)
                {
                    continue;
                }

                Vector3Int adjacentTilePosition = new Vector3Int(tilePosition.x + x, tilePosition.y + y, tilePosition.z);
                adjacentTilePositions.Add(adjacentTilePosition);
            }
        }

        return adjacentTilePositions;
    }

   
}
