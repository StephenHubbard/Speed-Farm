using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text cardAmountText;
    [SerializeField] private Transform cardCropLayoutGroup;
    [SerializeField] private GameObject cardCropPrefab;

    private Slider slider;
    private int cardWorthAmount;
    private float timeLeft;

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        DetermineCardCrops();
        timeLeft = Random.Range(10f, 30f);
        cardWorthAmount = Random.Range(1, 100);
        cardAmountText.text = cardWorthAmount.ToString("D2");
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

    private void DetermineCardCrops() {
        PlacedObjectTypeSO[] availableCardCrops = CardManager.Instance.GetAvailableCardCrops();

        int amountOfCropsOnCard = Random.Range(1, 4);

        for (int i = 0; i < amountOfCropsOnCard; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableCardCrops.Length);
            CardCrop newCardCrop = Instantiate(cardCropPrefab, cardCropLayoutGroup.transform).gameObject.GetComponent<CardCrop>();
            newCardCrop.SetPlacedObjectTypeSO(availableCardCrops[randomIndex]);
            newCardCrop.SetImageSprite();
        }
    }

    private void DetectCardTime()
    {
        timeLeft -= Time.deltaTime;
        slider.value = timeLeft;

        if (timeLeft <= 0f)
        {
            CardManager.Instance.CardCompletion(this);
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
