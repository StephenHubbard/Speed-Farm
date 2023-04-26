using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimizeWindow : MonoBehaviour
{
    [SerializeField] private GameObject _maximizeButton;

    private GameObject _windowToMinimize;

    private void Awake()
    {
        _windowToMinimize = transform.parent.gameObject;
    }

    public void WindowClose()
    {
        _windowToMinimize.SetActive(false);
        _maximizeButton.SetActive(true);
    }
}
