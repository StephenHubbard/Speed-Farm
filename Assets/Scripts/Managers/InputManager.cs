using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Camera cam;

    private void Awake() {
        cam = Camera.main;
    }

    private void Update() {
        // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        // if (hit.collider != null)
        // {
        //     Debug.Log(hit.collider.gameObject.name);
        // }

        ExitApplication();
    }

    private void ExitApplication() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
