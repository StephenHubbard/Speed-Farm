using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardCrop : MonoBehaviour
{
    public int AmountToCollect { get; private set; }

    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSO;

    private TMP_Text amountToCollectText;
    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
        amountToCollectText = GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
        AmountToCollect = Random.Range(1, 5);
        amountToCollectText.text = AmountToCollect.ToString();
    }

    public void SetPlacedObjectTypeSO(PlacedObjectTypeSO placedObjectTypeSO) {
        this.placedObjectTypeSO = placedObjectTypeSO;
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

    public void SetImageSprite() {
        image.sprite = placedObjectTypeSO.harvestedCropSprite;
    }

    public void CropCollected() {
        if (AmountToCollect <= 0) { return; }
        
        AmountToCollect--;
        amountToCollectText.text = AmountToCollect.ToString();
    }
}
