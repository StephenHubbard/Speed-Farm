using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject _buildingOutline;
    [SerializeField] private GameObject _shopContainerToOpen;

    private SpriteRenderer _spriteRenderer;
    private IIBuilding _building;

    private void Awake() {
        _building = GetComponent<IIBuilding>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter() {
        _buildingOutline.SetActive(true);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {

            CloseWindow[] allCloseWindows = FindObjectsOfType<CloseWindow>();

            foreach (CloseWindow closeWindow in allCloseWindows)
            {
                closeWindow.WindowClose();
            }

            _shopContainerToOpen.SetActive(!_shopContainerToOpen.activeInHierarchy);
            _building.OpenBuilding();
            InventoryManager.Instance.OpenBackPack();
        }
    }

    private void OnMouseExit() {
        _buildingOutline.SetActive(false);
    }

}
