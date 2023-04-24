using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindow : MonoBehaviour
{
    [SerializeField] private bool isBackpack = false;

    private GameObject _windowToClose;

    private void Awake() {
        _windowToClose = transform.parent.gameObject;
    }

    public void WindowClose() {
        _windowToClose.SetActive(false);

        if (isBackpack) {
            CloseWindow[] allCloseWindows = FindObjectsOfType<CloseWindow>();

            foreach (CloseWindow closeWindow in allCloseWindows)
            {
                closeWindow.WindowClose();
            }
        }
    }
}
