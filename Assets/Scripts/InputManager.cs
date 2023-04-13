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

            // Debug.Log(x + ", " + y);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
