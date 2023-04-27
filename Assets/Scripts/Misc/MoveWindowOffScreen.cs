using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindowOffScreen : MonoBehaviour
{   
    public bool IsBackpack => _isBackPack;

    [SerializeField] private bool _isBackPack;

    private Transform _windowContainer;
    private Vector2 _startingAnchoredPosition;

    private void Awake() {
        _windowContainer = transform.parent;
        _startingAnchoredPosition = _windowContainer.GetComponent<RectTransform>().anchoredPosition;
    }

    public void OpenWindow() {
        CloseAllWindowsButBackpack();

        if (!_isBackPack) {
            Backpack.Instance.OpenBackPack();
        }

        _windowContainer.GetComponent<RectTransform>().anchoredPosition = _startingAnchoredPosition;
    }

    public void CloseWindow() {
        _windowContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(_startingAnchoredPosition.x + 3000f, _startingAnchoredPosition.y);

        if (_isBackPack) {
            CloseAllWindowsButBackpack();
        }
    }

    private void CloseAllWindowsButBackpack() {
        MoveWindowOffScreen[] allWindows = FindObjectsOfType<MoveWindowOffScreen>();

        foreach (MoveWindowOffScreen window in allWindows)
        {
            if (!window.IsBackpack)
            {
                window.CloseWindow();
            }
        }
    }
}
