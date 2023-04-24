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
    public Tilemap GrassTilemap => _grassTilemap;
    public Tile DirtTile => _dirtTile;

    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _cellSize = 10f;
    [SerializeField] private Tile _dirtTile;
    [SerializeField] private List<Tile> _grassTiles = new List<Tile>();
    [SerializeField] private Tilemap _grassTilemap;

    private Grid<GridObject> _grid;

    protected override void Awake()
    {
        base.Awake();

        _grid = new Grid<GridObject>(_gridWidth, _gridHeight, _cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    private void Start() {
        StartCoroutine(RefreshSelectedObjectTypeRoutine());
    }

    public Grid<GridObject> GetGrid() {
        return _grid;
    }

    private IEnumerator RefreshSelectedObjectTypeRoutine() {
            yield return null;
            RefreshSelectedObjectType();
    }

    public class GridObject
    {
        public PlacedObject_Done PlacedObject => placedObject;
        public GameObject BuyLandSprite => buyLandSprite;
        public bool OwnsLand => ownsLand;
        public bool CanBuyLand => canBuyLand;
        public int x { get; private set; }
        public int y { get; private set; }

        private Grid<GridObject> grid;
        private PlacedObject_Done placedObject;
        private GameObject buyLandSprite;
        private bool ownsLand = false;
        private bool canBuyLand = false;

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public void CanBuyLandTrue()
        {
            canBuyLand = true;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject)
        {
            this.placedObject = placedObject;
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
        }

        public void SetBuyLandSprite(GameObject buyLandSprite) {
            this.buyLandSprite = buyLandSprite;
        }

        public void ClearBuyLandSprite()
        {
            buyLandSprite = null;
        }

        public GameObject GetBuyLandSprite()
        {
            return buyLandSprite;
        }

        public void BuyLand() {
            ownsLand = true;
            LandManager.Instance.IncreasePlotAmount();
            if (buyLandSprite != null) {
                Destroy(buyLandSprite);
            }
        }

        public bool CanBuild()
        {
            return PlacedObject == null;
        }
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
}
