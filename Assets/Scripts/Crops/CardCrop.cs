using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardCrop : MonoBehaviour
{
    public int AmountToCollect { get; private set; }
    public PlacedObjectTypeSO PlacedObjectTypeSO => _placedObjectTypeSO;

    [SerializeField] private PlacedObjectTypeSO _placedObjectTypeSO;

    private TMP_Text _amountToCollectText;
    private Image _image;

    private void Awake() {
        _image = GetComponent<Image>();
        _amountToCollectText = GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
        AmountToCollect += Random.Range(1, 5);
        _amountToCollectText.text = AmountToCollect.ToString();
    }

    private void Update() {
        _amountToCollectText.text = AmountToCollect.ToString();
    }

    public void IncreaseAmountToCollect(int amount) {
        AmountToCollect += amount;
    }

    public void SetPlacedObjectTypeSO(PlacedObjectTypeSO placedObjectTypeSO) {
        this._placedObjectTypeSO = placedObjectTypeSO;
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return _placedObjectTypeSO;
    }

    public void SetImageSprite() {
        _image.sprite = _placedObjectTypeSO.SpriteLifeCycle[_placedObjectTypeSO.SpriteLifeCycle.Length - 1];
    }

    public void CropCollected() {
        if (AmountToCollect <= 0) { return; }
        
        AmountToCollect--;
        _amountToCollectText.text = AmountToCollect.ToString();
    }
}