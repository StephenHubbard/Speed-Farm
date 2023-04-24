using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;


public class LandManager : Singleton<LandManager>
{
    public Color GreenColor { get { return _greenColor; } }
    public Color RedColor { get { return _redColor; } }
    public List<GameObject> AllShowLandSprites => _allShowLandSprites;
    public int AmountOfPlotsOwned => _amountOfPlotsOwned;

    [SerializeField] private Tilemap _grassTilemap;
    [SerializeField] private Tile _dirtTile;
    [SerializeField] private PlacedObjectTypeSO _fenceSO;
    [SerializeField] private GameObject _showAvailableLandToBuyPrefab;
    [SerializeField] private Color _greenColor;
    [SerializeField] private Color _redColor;

    private List<GameObject> _allShowLandSprites = new List<GameObject>();
    private Grid<GridGeneration.GridObject> _grid;

    private int _amountOfPlotsOwned = 0;

    private void Start() {
        _grid = GridGeneration.Instance.GetGrid();


        DetermineStartingLandOwnership();
        SpawnFencesBottomRow();
    }


    public void IncreasePlotAmount() {
        _amountOfPlotsOwned++;
    }

    public void BuyLand()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
    
        List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

        bool anyTileCanBeBought = false;

        foreach (Vector3Int selectedTile in selectedTiles)
        {
            if (_grid.GetGridObject(selectedTile).CanBuyLand)
            {
                anyTileCanBeBought = true;
            }
        }

        if (anyTileCanBeBought && EconomyManager.Instance.BuyLandPlots(selectedTiles.Count)) {

            foreach (Vector3Int selectedTile in selectedTiles)
            {
                _grid.GetGridObject(selectedTile).BuyLand();
                
                List<Vector3Int> adjacentTilePositions = GetAdjacentTiles(selectedTile);

                foreach (Vector3Int adjacentTilePosition in adjacentTilePositions)
                {
                    _grid.GetGridObject(adjacentTilePosition).CanBuyLandTrue();
                }
            }
        } else { 
            return;
        }

        SpawnFences(selectedTiles);
    }

    private void SpawnFences(List<Vector3Int> selectedTiles) {

        List<Vector3Int> tilesToSpawnFence = new List<Vector3Int>();

        foreach (Vector3Int selectedTile in selectedTiles)
        {
            List<Vector3Int> adjacentTilePositions = GetAdjacentTiles(selectedTile);

            foreach (Vector3Int adjacentTilePosition in adjacentTilePositions)
            {
                if (!tilesToSpawnFence.Contains(adjacentTilePosition) && !_grid.GetGridObject(adjacentTilePosition).OwnsLand && _grid.GetGridObject(adjacentTilePosition).y >= 1) {
                    tilesToSpawnFence.Add(adjacentTilePosition);
                }
            }

            PlacedObject_Done currentPlacedObject = _grid.GetGridObject(selectedTile).PlacedObject;

            if (currentPlacedObject && _grid.GetGridObject(selectedTile).y >= 1)
            {
                currentPlacedObject.DestroySelf();

                List<Vector2Int> gridPositionList = currentPlacedObject.GetGridPositionList();

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    _grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }

        foreach (Vector3Int tileToSpawnFence in tilesToSpawnFence)
        {
            PlacedObject_Done currentPlacedObject = _grid.GetGridObject(tileToSpawnFence).PlacedObject;

            if (currentPlacedObject)
            {
                currentPlacedObject.DestroySelf();

                List<Vector2Int> gridPositionList = currentPlacedObject.GetGridPositionList();

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    _grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }

            Vector2Int adjPlacedObjOrigin = new Vector2Int(tileToSpawnFence.x, tileToSpawnFence.y);

            PlacedObject_Done adjPlacedObj = PlacedObject_Done.Create(tileToSpawnFence, adjPlacedObjOrigin, _fenceSO);

            _grid.GetGridObject(tileToSpawnFence).SetPlacedObject(adjPlacedObj);
        }

    }

    public void ShowAvailableLandToBuy() {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                PlacedObject_Done placedObject = _grid.GetGridObject(tilePosition).PlacedObject;
                PlacedObjectTypeSO placedObjectTypeSO = placedObject?.PlacedObjectTypeSO;

                if (placedObject && placedObjectTypeSO == _fenceSO && _grid.GetGridObject(tilePosition).y > 0) {
                    GameObject showLandSpritePrefab = Instantiate(_showAvailableLandToBuyPrefab, new Vector2(x, y), Quaternion.identity);
                    _allShowLandSprites.Add(showLandSpritePrefab);
                    _grid.GetGridObject(tilePosition).SetBuyLandSprite(showLandSpritePrefab);
                    showLandSpritePrefab.GetComponentInChildren<SpriteRenderer>().color = new Color(_greenColor.r, _greenColor.g, _greenColor.b, _greenColor.a);
                }
            }
        }
    }

    public void HideAvailableLandToBuy() {
        if (_allShowLandSprites.Count == 0) { return; }

        foreach (GameObject showLandSprite in _allShowLandSprites)
        {
            Destroy(showLandSprite);
        }
    }

    public void DetermineStartingLandOwnership() {
        List<Vector3Int> startingTiles = new List<Vector3Int>();

        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (_grassTilemap.GetTile(tilePosition) == _dirtTile) {
                    _grid.GetGridObject(tilePosition).BuyLand();

                    startingTiles.Add(tilePosition);
                }
            }
        }

        foreach (Vector3Int startingTile in startingTiles)
        {
            List<Vector3Int> adjacentTilePositions = GetAdjacentTiles(startingTile);

            foreach (Vector3Int adjacentTilePosition in adjacentTilePositions)
            {
                _grid.GetGridObject(adjacentTilePosition).CanBuyLandTrue();
            }
        }

        SpawnFences(startingTiles);
    }

    private void SpawnFencesBottomRow() {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y == 0) {
                    PlacedObject_Done currentPlacedObject = _grid.GetGridObject(tilePosition).PlacedObject;

                    if (currentPlacedObject)
                    {
                        currentPlacedObject.DestroySelf();

                        List<Vector2Int> gridPositionList = currentPlacedObject.GetGridPositionList();

                        foreach (Vector2Int gridPosition in gridPositionList)
                        {
                            _grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                        }
                    }

                    if (_grid.GetGridObject(tilePosition).PlacedObject == null && !_grid.GetGridObject(tilePosition).OwnsLand)
                    {

                        Vector2Int adjPlacedObjOrigin = new Vector2Int(tilePosition.x, tilePosition.y);

                        PlacedObject_Done adjPlacedObj = PlacedObject_Done.Create(tilePosition, adjPlacedObjOrigin, _fenceSO);

                        _grid.GetGridObject(tilePosition).SetPlacedObject(adjPlacedObj);
                    }
                }
            }
        }
    }

  
    private List<Vector3Int> GetAdjacentTiles(Vector3Int tilePosition)
    {
        List<Vector3Int> adjacentTilePositions = new List<Vector3Int>();

        // Check if adjacent tiles are on the grid before adding them to the list
        if (tilePosition.x < _grid.GetWidth() - 1) adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y, tilePosition.z));
        if (tilePosition.x > 0) adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y, tilePosition.z));
        if (tilePosition.y < _grid.GetHeight() - 1) adjacentTilePositions.Add(new Vector3Int(tilePosition.x, tilePosition.y + 1, tilePosition.z));
        if (tilePosition.y > 0) adjacentTilePositions.Add(new Vector3Int(tilePosition.x, tilePosition.y - 1, tilePosition.z));

        // You can decide to include diagonal adjacent tiles by uncommenting the following lines
        if (tilePosition.x < _grid.GetWidth() - 1 && tilePosition.y < _grid.GetHeight() - 1) adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y + 1, tilePosition.z));
        if (tilePosition.x < _grid.GetWidth() - 1 && tilePosition.y > 0) adjacentTilePositions.Add(new Vector3Int(tilePosition.x + 1, tilePosition.y - 1, tilePosition.z));
        if (tilePosition.x > 0 && tilePosition.y < _grid.GetHeight() - 1) adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y + 1, tilePosition.z));
        if (tilePosition.x > 0 && tilePosition.y > 0) adjacentTilePositions.Add(new Vector3Int(tilePosition.x - 1, tilePosition.y - 1, tilePosition.z));

        return adjacentTilePositions;
    }

    // public List<Vector3Int> GetAdjacentTilesDistance(Vector3Int tilePosition, int distance)
    // {
    //     List<Vector3Int> adjacentTilePositions = new List<Vector3Int>();

    //     for (int x = -distance; x <= distance; x++)
    //     {
    //         for (int y = -distance; y <= distance; y++)
    //         {
    //             // Skip the center tile and diagonal tiles when distance is 1
    //             if (distance == 1 && (x == 0 || y == 0 || Mathf.Abs(x) == Mathf.Abs(y)))
    //             {
    //                 continue;
    //             }

    //             // Skip the center tile
    //             if (x == 0 && y == 0)
    //             {
    //                 continue;
    //             }

    //             Vector3Int adjacentTilePosition = new Vector3Int(tilePosition.x + x, tilePosition.y + y, tilePosition.z);
    //             adjacentTilePositions.Add(adjacentTilePosition);
    //         }
    //     }

    //     return adjacentTilePositions;
    // }

   
}
