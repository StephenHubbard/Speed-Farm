using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public bool IsFullyGrown { get; private set; }

    private int cropStage = 0;

    private SpriteRenderer spriteRenderer;
    private PlacedObjectTypeSO placedObjectTypeSO;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    private void Start() {
        IsFullyGrown = false;

        placedObjectTypeSO = GetComponent<PlacedObject_Done>().PlacedObjectTypeSO;

        StartCoroutine(GrowCropRoutine());
    }

    public void SellCrop() {
        CardManager.Instance.CropHarvested(placedObjectTypeSO);
    }

    private IEnumerator GrowCropRoutine() {
        while (cropStage < placedObjectTypeSO.spriteLifeCycle.Length - 1)
        {
            spriteRenderer.sprite = placedObjectTypeSO.spriteLifeCycle[cropStage];
            cropStage++;

            if (cropStage < placedObjectTypeSO.spriteLifeCycle.Length - 1) {
                yield return new WaitForSeconds(1f);
            } else {
                yield return null;
            }
        }

        IsFullyGrown = true;
    }
}
