using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCrop : MonoBehaviour
{
    public int AmountToCollect { get; private set; }

    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSO;

    private TMP_Text amountToCollectText;

    private void Awake() {
        amountToCollectText = GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
        AmountToCollect = Random.Range(1, 5);
        amountToCollectText.text = AmountToCollect.ToString();
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

    public void CropCollected() {
        if (AmountToCollect <= 0) { return; }
        
        AmountToCollect--;
        amountToCollectText.text = AmountToCollect.ToString();
    }
}
