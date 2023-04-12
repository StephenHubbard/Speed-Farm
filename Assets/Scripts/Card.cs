using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text cardAmountText;

    private Slider slider;
    private int cardWorthAmount;
    private float timeLeft;

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        timeLeft = Random.Range(10f, 30f);
        cardWorthAmount = Random.Range(100, 500);
        cardAmountText.text = "$" + cardWorthAmount.ToString();
        slider.maxValue = timeLeft;
    }

    private void Update() {
        DetectCardTime();
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

    private void DetectCardTime()
    {
        timeLeft -= Time.deltaTime;
        slider.value = timeLeft;

        if (timeLeft <= 0f)
        {
            CardManager.Instance.CardCompletion(this);
            Destroy(gameObject);
        }
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
