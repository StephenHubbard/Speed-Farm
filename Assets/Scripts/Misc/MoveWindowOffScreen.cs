using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindowOffScreen : MonoBehaviour
{   
    [SerializeField] private Vector3 _moveLocation;

    private Transform _backPackContainer;

    private void Awake() {
        _backPackContainer = transform.parent;
    }

    public void OpenWindow() {
        _backPackContainer.position = new Vector3(_backPackContainer.position.x - _moveLocation.x, _backPackContainer.position.y, 0);
    }

    public void CloseWindow() {
        _backPackContainer.position = new Vector3(_backPackContainer.position.x + _moveLocation.x, _backPackContainer.position.y, 0);

        CloseWindow[] allCloseWindows = FindObjectsOfType<CloseWindow>();

        foreach (CloseWindow closeWindow in allCloseWindows)
        {
            closeWindow.WindowClose();
        }
    }
}
