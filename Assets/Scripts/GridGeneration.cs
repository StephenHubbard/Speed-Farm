using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridGeneration : Singleton<GridGeneration>
{
    [SerializeField] int gridWidth = 10;
    [SerializeField] int gridHeight = 10;
    [SerializeField] float cellSize = 10f;
    [SerializeField] private Tile dirtTile;
    [SerializeField] private Tile[] grassTiles;
    [SerializeField] private Tilemap grassTilemap;

    private PlacedObjectTypeSO placedObjectTypeSO;

    public event EventHandler OnSelectedChanged;
    // public event EventHandler OnObjectPlaced;

    private Grid<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir;

    protected override void Awake()
    {
        base.Awake();

        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        // placedObjectTypeSO = null;
    }

    private void Start() {
        RefreshSelectedObjectType();
    }

    public Grid<GridObject> GetGrid() {
        return grid;
    }

    public void SetPlacedObjectTypeSO(PlacedObjectTypeSO placedObjectTypeSO) {
        this.placedObjectTypeSO = placedObjectTypeSO;
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
            LandManager.Instance.HideAvailableLandToBuy();
            LandManager.Instance.ShowAvailableLandToBuy();
            LandManager.Instance.BuyLandToggleTrue();
        }

        // public bool DoesOwnLand() {
        //     return ownsLand;
        // }

        public bool CanBuild()
        {
            return placedObject == null;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null && !InventoryManager.Instance.IsShovelEquipped)
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            grid.GetXY(mousePosition, out int x, out int y);

            Vector2Int placedObjectOrigin = new Vector2Int(x, y);
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            // Test Can Build
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild && grassTilemap.GetTile(tilePosition) == dirtTile)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

                PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
                placedObject.transform.rotation = Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }

                // OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                //DeselectObjectType();
            }
        } 

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }

        if (Input.GetMouseButtonDown(0) && InventoryManager.Instance.IsShovelEquipped)
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            PlacedObject_Done placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
            bool doesOwnLand = grid.GetGridObject(mousePosition).ownsLand;
            if (!placedObject && doesOwnLand) {
                int x = grid.GetGridObject(mousePosition).x;
                int y = grid.GetGridObject(mousePosition).y;
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                foreach (Tile tile in grassTiles)
                {
                    if (grassTilemap.GetTile(tilePosition) == tile)
                    {
                        grassTilemap.SetTile(tilePosition, dirtTile);
                        return;
                    } 
                }

                if (grassTilemap.GetTile(tilePosition) == dirtTile)
                {
                    if ((x + y) % 2 == 0) {
                        grassTilemap.SetTile(tilePosition, grassTiles[1]);
                    } else {
                        grassTilemap.SetTile(tilePosition, grassTiles[0]);
                    }
                }
            }
        } 

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            PlacedObject_Done placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
            Crop crop = placedObject?.GetComponent<Crop>();

            if (placedObject != null && crop && crop.IsFullyGrown)
            {
                crop.SellCrop();
                placedObject.DestroySelf();

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }
    }

    public void DeselectObjectType()
    {
        placedObjectTypeSO = null;
    }

    public void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        LandManager.Instance.HideAvailableLandToBuy();
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXY(worldPosition, out int x, out int z);
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
