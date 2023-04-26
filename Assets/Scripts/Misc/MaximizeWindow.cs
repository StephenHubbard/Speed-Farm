using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaximizeWindow : MonoBehaviour
{
    [SerializeField] private GameObject _windowToOpen;

    public void MaximizeThisWindow() {
        _windowToOpen.SetActive(true);
        CardManager.Instance.SetSelectionOutline();
        this.gameObject.SetActive(false);
    }
}
