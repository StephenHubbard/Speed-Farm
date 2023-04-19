using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private Transform boxVisual;

    [SerializeField] private List<Vector3Int> selectedTiles = new List<Vector3Int>();

    Vector2 startTilePos;
    Vector2 currentTilePos;

    private void Start() {
        DrawVisual();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            boxVisual.gameObject.SetActive(true);
            selectedTiles.Clear();

            int x = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).x;
            int y = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).y;

            startTilePos = new Vector2(x, y);
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
            DrawVisual();
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
            DrawVisual();
            boxVisual.gameObject.SetActive(false);
            AssignSelectedTiles();
        }
    }

    public List<Vector3Int> GetSelectedTiles() {
        return selectedTiles;
    }

    private void AssignSelectedTiles() {
        var grid = GridGeneration.Instance.GetGrid();

        List<Vector3Int> selectedXTiles = new List<Vector3Int>();

        selectedTiles.Add(grid.GetVector3Int(startTilePos));
        selectedXTiles.Add(grid.GetVector3Int(startTilePos));

        if (currentTilePos.x <= startTilePos.x) {
            // dragging left

            int amountOfTilesX = (int)startTilePos.x - (int)currentTilePos.x;

            for (int i = 1; i <= amountOfTilesX; i++)
            {
                Vector2 tileToAdd = new Vector2((int)startTilePos.x - i, (int)startTilePos.y);

                selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                selectedXTiles.Add(grid.GetVector3Int(tileToAdd));
            }

        } else {
            // dragging right

            int amountOfTilesX = (int)currentTilePos.x - (int)startTilePos.x;

            for (int i = 1; i <= amountOfTilesX; i++)
            {
                Vector2 tileToAdd = new Vector2((int)startTilePos.x + i, (int)startTilePos.y);

                selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                selectedXTiles.Add(grid.GetVector3Int(tileToAdd));
            }
        }

        for (int j = 0; j < selectedXTiles.Count; j++)
        {
            if (currentTilePos.y < startTilePos.y)
            {
                // dragging down

                int amountOfTilesY = (int)startTilePos.y - (int)currentTilePos.y;

                for (int i = 1; i <= amountOfTilesY; i++)
                {
                    Vector2 tileToAdd = new Vector2((int)selectedXTiles[j].x, (int)selectedXTiles[j].y - i);

                    selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                }

            } else {
                // dragging up

                int amountOfTilesY = (int)currentTilePos.y - (int)startTilePos.y;

                for (int i = 1; i <= amountOfTilesY; i++)
                {
                    Vector2 tileToAdd = new Vector2((int)selectedXTiles[j].x, (int)selectedXTiles[j].y + i);

                    selectedTiles.Add(grid.GetVector3Int(tileToAdd));
                }
            }
        }

    }

    private void DrawVisual() {
        int x = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).x;
        int y = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).y;

        currentTilePos = new Vector2(x, y);

        Vector2 lowerLeft = new Vector3(Mathf.Min(startTilePos.x, currentTilePos.x), Mathf.Min(startTilePos.y, currentTilePos.y));
        Vector2 upperRight = new Vector3(Mathf.Max(startTilePos.x + 1, currentTilePos.x + 1), Mathf.Max(startTilePos.y + 1, currentTilePos.y + 1));

        boxVisual.position = lowerLeft;
        boxVisual.localScale = upperRight - lowerLeft;
    }

}
