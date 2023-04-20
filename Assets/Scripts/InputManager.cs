using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class InputManager : MonoBehaviour
{
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            int x = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).x;
            int y = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).y;

            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            var grid = GridGeneration.Instance.GetGrid();

            // Debug.Log(grid.GetGridObject(tilePosition).placedObject);
            // Debug.Log(grid.GetGridObject(tilePosition).ownsLand);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
