using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridGeneration : Singleton<GridGeneration>
{
    public event EventHandler OnSelectedChanged;

    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _cellSize = 10f;
    [SerializeField] private Tile _dirtTile;
    [SerializeField] private List<Tile> _grassTiles = new List<Tile>();
    [SerializeField] private Tilemap _grassTilemap;

    private PlacedObjectTypeSO _placedObjectTypeSO;
    private Grid<GridObject> _grid;
    private PlacedObjectTypeSO.Dir _dir;

    protected override void Awake()
    {
        base.Awake();

        _grid = new Grid<GridObject>(_gridWidth, _gridHeight, _cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        // placedObjectTypeSO = null;
    }

    private void Start() {
        RefreshSelectedObjectType();
    }

    public Grid<GridObject> GetGrid() {
        return _grid;
    }

    public void SetPlacedObjectTypeSO(PlacedObjectTypeSO placedObjectTypeSO) {
        this._placedObjectTypeSO = placedObjectTypeSO;
    }

    public class GridObject
    {
        private Grid<GridObject> grid;
        public int x;
        public int y;
        public PlacedObject_Done placedObject;
        public GameObject buyLandSprite;
        public bool ownsLand = false;
        public bool canBuyLand = false;

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject_Done GetPlacedObject()
        {
            return placedObject;
        }

        public void SetBuyLandSprite(GameObject buyLandSprite) {
            this.buyLandSprite = buyLandSprite;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearBuyLandSprite()
        {
            buyLandSprite = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public GameObject GetBuyLandSprite()
        {
            return buyLandSprite;
        }


        public void BuyLand() {
            ownsLand = true;
            Destroy(buyLandSprite);
            placedObject?.DestroySelf();
            ClearPlacedObject();
        }


        public bool CanBuild()
        {
            return placedObject == null;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.GetMouseButtonUp(0) && _placedObjectTypeSO != null && !InventoryManager.Instance.IsShovelEquipped)
        {
            List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

            foreach (Vector3Int selectedTile in selectedTiles)
            {
                // Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                _grid.GetXY(selectedTile, out int x, out int y);

                Vector2Int placedObjectOrigin = new Vector2Int(x, y);
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Test Can Build
                List<Vector2Int> gridPositionList = _placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, _dir);

                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    if (!_grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                    {
                        canBuild = false;
                        break;
                    }
                }

                if (canBuild && _grassTilemap.GetTile(tilePosition) == _dirtTile)
                {
                    Vector2Int rotationOffset = _placedObjectTypeSO.GetRotationOffset(_dir);
                    Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * _grid.GetCellSize();

                    PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, _dir, _placedObjectTypeSO);
                    placedObject.transform.rotation = Quaternion.Euler(0, 0, -_placedObjectTypeSO.GetRotationAngle(_dir));

                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        _grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }
                }
            }

        } 

        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     dir = PlacedObjectTypeSO.GetNextDir(dir);
        // }

        if (Input.GetMouseButtonUp(0) && InventoryManager.Instance.IsShovelEquipped)
        {
            List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

            List<Vector3Int> ownedTiles = new List<Vector3Int>();

            foreach (Vector3Int selectedTile in selectedTiles)
            {
                if (_grid.GetGridObject(selectedTile).ownsLand) {
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
                PlacedObject_Done placedObject = _grid.GetGridObject(selectedTile).GetPlacedObject();
                bool doesOwnLand = _grid.GetGridObject(selectedTile).ownsLand;
                if (!placedObject && doesOwnLand) {

                    if (anyTilesAreGrass) {
                        _grassTilemap.SetTile(selectedTile, _dirtTile);
                        continue;
                    } else {
                        if ((selectedTile.x + selectedTile.y) % 2 == 0) {
                            _grassTilemap.SetTile(selectedTile, _grassTiles[1]);
                        } else {
                            _grassTilemap.SetTile(selectedTile, _grassTiles[0]);
                        }
                    }
                }
            }
        } 

        if (Input.GetMouseButtonUp(1)) {
            List<Vector3Int> selectedTiles = SelectionManager.Instance.GetSelectedTiles();

            foreach (Vector3Int selectedTile in selectedTiles)
            {
                PlacedObject_Done placedObject = _grid.GetGridObject(selectedTile).GetPlacedObject();
                Crop crop = placedObject?.GetComponent<Crop>();

                if (placedObject != null && crop && crop.IsFullyGrown)
                {
                    crop.SellCrop();
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        _grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }

        }
    }

    public void DeselectObjectType()
    {
        _placedObjectTypeSO = null;
    }

    public void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        LandManager.Instance.HideAvailableLandToBuy();
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        _grid.GetXY(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    // public Vector3 GetMouseWorldSnappedPosition()
    // {
    //     Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
    //     grid.GetXY(mousePosition, out int x, out int y);

    //     if (placedObjectTypeSO != null)
    //     {
    //         Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
    //         Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
    //         return placedObjectWorldPosition;
    //     }
    //     else
    //     {
    //         return mousePosition;
    //     }
    // }

    // public Quaternion GetPlacedObjectRotation()
    // {
    //     if (placedObjectTypeSO != null)
    //     {
    //         return Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));
    //     }
    //     else
    //     {
    //         return Quaternion.identity;
    //     }
    // }

    // public PlacedObjectTypeSO GetPlacedObjectTypeSO()
    // {
    //     return placedObjectTypeSO;
    // }

}
