using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    public GameObject ShopContainerToOpen => _shopContainerToOpen;

    [SerializeField] private GameObject _buildingOutline;
    [SerializeField] private GameObject _shopContainerToOpen;

    private IIBuilding _building;

    private void Awake() {
        _building = GetComponent<IIBuilding>();
    }

    private void OnMouseOver() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            _buildingOutline.SetActive(true);
        } else {
            _buildingOutline.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {

            CloseWindow[] allCloseWindows = FindObjectsOfType<CloseWindow>();

            foreach (CloseWindow closeWindow in allCloseWindows)
            {
                closeWindow.WindowClose();
            }

            _shopContainerToOpen.SetActive(!_shopContainerToOpen.activeInHierarchy);
            if (_building != null) {
                _building.OpenBuilding();
            }
            Backpack.Instance.OpenBackPack();
        }
    }

    private void OnMouseExit() {
        _buildingOutline.SetActive(false);
    }
}
