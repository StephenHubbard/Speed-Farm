using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    public MoveWindowOffScreen MoveWindowOffScreen => _moveWindowOffScreen;
    public GameObject ShopContainerToOpen => _shopContainerToOpen;

    [SerializeField] private GameObject _buildingOutline;
    [SerializeField] private GameObject _shopContainerToOpen;

    private IIBuilding _building;
    private MoveWindowOffScreen _moveWindowOffScreen;


    private void Awake() {
        _moveWindowOffScreen = _shopContainerToOpen.GetComponentInChildren<MoveWindowOffScreen>();
        _building = GetComponent<IIBuilding>();
    }

    private void OnMouseOver() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            _buildingOutline.SetActive(true);
        } else {
            _buildingOutline.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {

            if (!_shopContainerToOpen.gameObject.activeInHierarchy)
            {
                _shopContainerToOpen.gameObject.SetActive(true);
            }

            _moveWindowOffScreen.ToggleWindow();
        }
    }

    private void OnMouseExit() {
        _buildingOutline.SetActive(false);
    }
}
