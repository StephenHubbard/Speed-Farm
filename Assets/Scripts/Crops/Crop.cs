using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public bool IsFullyGrown { get; private set; }
    public PlacedObjectTypeSO PlacedObjectTypeSO => _placedObjectTypeSO;

    [SerializeField] private float _stageGrowTime = 0.5f;

    private int _cropStage = 0;

    private SpriteRenderer _spriteRenderer;
    private PlacedObjectTypeSO _placedObjectTypeSO;

    private void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        IsFullyGrown = false;
    }
    
    private void Start() {
        _placedObjectTypeSO = GetComponent<PlacedObject_Done>().PlacedObjectTypeSO;

        if (!IsFullyGrown) {
            StartCoroutine(GrowCropRoutine());
        }
    }

    // public void SellCrop() {
    //     CardManager.Instance.CropHarvested(_placedObjectTypeSO);
    // }

    public void StraightToFullyGrown() {
        IsFullyGrown = true;
    }

    private IEnumerator GrowCropRoutine() {
        while (_cropStage < _placedObjectTypeSO.SpriteLifeCycle.Length)
        {
            _spriteRenderer.sprite = _placedObjectTypeSO.SpriteLifeCycle[_cropStage];
            _cropStage++;

            if (_cropStage < _placedObjectTypeSO.SpriteLifeCycle.Length) {
                yield return new WaitForSeconds(_stageGrowTime);
            } else {
                yield return null;
            }
        }

        IsFullyGrown = true;
    }
}
