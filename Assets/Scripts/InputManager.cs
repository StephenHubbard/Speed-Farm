using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            int x = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).x;
            int y = GridGeneration.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).y;

            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            // Debug.Log(x);
            // Debug.Log(y);

            var grid = GridGeneration.Instance.GetGrid();

            // Debug.Log(grid.GetGridObject(tilePosition).PlacedObject);
            // Debug.Log(grid.GetGridObject(tilePosition).OwnsLand);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        // if (Input.GetKeyDown(KeyCode.R)) {
        //     SceneManager.LoadScene(0);
        // }
    }
}
