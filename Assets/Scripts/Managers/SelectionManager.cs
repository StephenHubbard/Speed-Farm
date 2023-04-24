using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private Transform _boxVisual;
    [SerializeField] private List<Vector3Int> _selectedTiles = new List<Vector3Int>();

    private Vector2 _startTilePos;
    private Vector2 _currentTilePos;

    private bool _cancelSelection = false;

    private void Start() {
        DrawVisual();
    }

    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            ClearSelection();

            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            
            if (GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()) == null) { return; }

            _cancelSelection = false;
            _boxVisual.gameObject.SetActive(true);

            int x = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).x;
            int y = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).y;

            _startTilePos = new Vector2(x, y);
        }

        if (Input.GetMouseButton(0) && !_cancelSelection) {
            DrawVisual();
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButtonDown(1))
        {
            if (GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()) == null) { return; }

            _cancelSelection = true;
            _boxVisual.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0) && !_cancelSelection) {
            DrawVisual();
            _boxVisual.gameObject.SetActive(false);
            AssignSelectedTiles();

            if (!EventSystem.current.IsPointerOverGameObject()) { 
                InventoryManager.Instance.UseCurrentEquippedItem();
            }
        }
    }

    private void ClearSelection() {
        _selectedTiles.Clear();
        _startTilePos = new Vector2();
        _currentTilePos = new Vector2();
    }
   

    public List<Vector3Int> GetSelectedTiles()
    {
        return _selectedTiles;
    }

    private void DrawVisual()
    {
        if (GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()) == null) { return; }

        int x = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).x;
        int y = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).y;

        _currentTilePos = new Vector2(x, y);

        Vector2 lowerLeft = new Vector3(Mathf.Min(_startTilePos.x, _currentTilePos.x), Mathf.Min(_startTilePos.y, _currentTilePos.y));
        Vector2 upperRight = new Vector3(Mathf.Max(_startTilePos.x + 1, _currentTilePos.x + 1), Mathf.Max(_startTilePos.y + 1, _currentTilePos.y + 1));

        _boxVisual.position = lowerLeft;
        _boxVisual.localScale = upperRight - lowerLeft;
    }

    private void AssignSelectedTiles() {
        var grid = GridGeneration.Instance.GetGrid();

        List<Vector3Int> selectedXTiles = new List<Vector3Int>();

        _selectedTiles.Add(grid.GetVector3Int(_startTilePos));
        selectedXTiles.Add(grid.GetVector3Int(_startTilePos));

        if (_currentTilePos.x <= _startTilePos.x) {
            // dragging left

            int amountOfTilesX = (int)_startTilePos.x - (int)_currentTilePos.x;

            for (int i = 1; i <= amountOfTilesX; i++)
            {
                Vector2 tileToAdd = new Vector2((int)_startTilePos.x - i, (int)_startTilePos.y);

                _selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                selectedXTiles.Add(grid.GetVector3Int(tileToAdd));
            }

        } else {
            // dragging right

            int amountOfTilesX = (int)_currentTilePos.x - (int)_startTilePos.x;

            for (int i = 1; i <= amountOfTilesX; i++)
            {
                Vector2 tileToAdd = new Vector2((int)_startTilePos.x + i, (int)_startTilePos.y);

                _selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                selectedXTiles.Add(grid.GetVector3Int(tileToAdd));
            }
        }

        for (int j = 0; j < selectedXTiles.Count; j++)
        {
            if (_currentTilePos.y < _startTilePos.y)
            {
                // dragging down

                int amountOfTilesY = (int)_startTilePos.y - (int)_currentTilePos.y;

                for (int i = 1; i <= amountOfTilesY; i++)
                {
                    Vector2 tileToAdd = new Vector2((int)selectedXTiles[j].x, (int)selectedXTiles[j].y - i);

                    _selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                }

            } else {
                // dragging up

                int amountOfTilesY = (int)_currentTilePos.y - (int)_startTilePos.y;

                for (int i = 1; i <= amountOfTilesY; i++)
                {
                    Vector2 tileToAdd = new Vector2((int)selectedXTiles[j].x, (int)selectedXTiles[j].y + i);

                    _selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                }
            }
        }

    }

    

}
