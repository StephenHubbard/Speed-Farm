using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public bool IsFullyGrown { get; private set; }

    [SerializeField] private float stageGrowTime = 0.5f;

    private int cropStage = 0;

    private SpriteRenderer spriteRenderer;
    private PlacedObjectTypeSO placedObjectTypeSO;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        IsFullyGrown = false;
    }
    
    private void Start() {
        placedObjectTypeSO = GetComponent<PlacedObject_Done>().PlacedObjectTypeSO;

        if (!IsFullyGrown) {
            StartCoroutine(GrowCropRoutine());
        }
    }

    public void SellCrop() {
        CardManager.Instance.CropHarvested(placedObjectTypeSO);
    }

    public void StraightToFullyGrown() {
        IsFullyGrown = true;
    }

    private IEnumerator GrowCropRoutine() {
        while (cropStage < placedObjectTypeSO.spriteLifeCycle.Length - 1)
        {
            spriteRenderer.sprite = placedObjectTypeSO.spriteLifeCycle[cropStage];
            cropStage++;

            if (cropStage < placedObjectTypeSO.spriteLifeCycle.Length - 1) {
                yield return new WaitForSeconds(stageGrowTime);
            } else {
                yield return null;
            }
        }

        IsFullyGrown = true;
    }
}
