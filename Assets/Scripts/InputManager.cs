using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class InputManager : MonoBehaviour
{
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            int x = GridGeneration.Instance.GetGridObject().GetGridObject(UtilsClass.GetMouseWorldPosition()).x;
            int y = GridGeneration.Instance.GetGridObject().GetGridObject(UtilsClass.GetMouseWorldPosition()).y;

            // Debug.Log(x + ", " + y);
        }
    }
}
