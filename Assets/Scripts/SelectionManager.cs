using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Transform boxVisual;

    Vector2 startPosition;
    Vector2 endPositon;

    private void Awake() {
        startPosition = Vector2.zero;
        endPositon = Vector2.zero;
    }

    private void Start() {
        DrawVisual();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            boxVisual.gameObject.SetActive(true);
            startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButton(0)) {
            DrawVisual();
        }

        if (Input.GetMouseButtonUp(0)) {
            startPosition = Vector2.zero;
            endPositon = Vector2.zero;
            DrawVisual();
            boxVisual.gameObject.SetActive(false);
        }
    }

    private void DrawVisual() {
        Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 lowerLeft = new Vector3(Mathf.Min(startPosition.x, currentMousePosition.x), Mathf.Min(startPosition.y, currentMousePosition.y));
        Vector3 upperRight = new Vector3(Mathf.Max(startPosition.x, currentMousePosition.x), Mathf.Max(startPosition.y, currentMousePosition.y));

        boxVisual.position = lowerLeft;
        boxVisual.localScale = upperRight - lowerLeft;
    }

    private void SelectTiles() {

    }
}
