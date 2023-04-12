using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text cardAmountText;

    private int cardWorthAmount;

    private void Start() {
        cardWorthAmount = Random.Range(100, 500);
        cardAmountText.text = "$" + cardWorthAmount.ToString();
    }

    public void SetCurrentCard()
    {
        CardManager.Instance.SetCurrentCard(this);
    }
    
    public void CropHarvested(PlacedObjectTypeSO placedObjectTypeSO) {
        CardCrop[] cropsOnCard = GetComponentsInChildren<CardCrop>();

        foreach (CardCrop cardCrop in cropsOnCard)
        {
            if (cardCrop.GetComponent<CardCrop>().GetPlacedObjectTypeSO() == placedObjectTypeSO) {
                cardCrop.GetComponent<CardCrop>().CropCollected();
            }
        }

        CheckCardCompletion();
    }
     
    private void CheckCardCompletion() {
        CardCrop[] cropsOnCard = GetComponentsInChildren<CardCrop>();


        foreach (CardCrop cardCrop in cropsOnCard)
        {
            if (cardCrop.GetComponent<CardCrop>().AmountToCollect > 0) {
                return;
            }
        }

        EconomyManager.Instance.UpdateCurrentCoinAmount(cardWorthAmount);
        CardManager.Instance.CardCompletion(this);
        Destroy(gameObject);
    }

}
