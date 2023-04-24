using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject _buildingOutline;

    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter() {
        _buildingOutline.SetActive(true);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("clicked building");
        }
    }

    private void OnMouseExit() {
        _buildingOutline.SetActive(false);
    }

}
