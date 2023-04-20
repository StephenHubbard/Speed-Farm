using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text _cardAmountText;
    [SerializeField] private Transform _cardCropLayoutGroup;
    [SerializeField] private GameObject _cardCropPrefab;

    private List<CardCrop> allCardCrops = new List<CardCrop>();
    private Slider _slider;
    private int _cardWorthAmount;
    private float _timeLeft;

    private void Awake() {
        _slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        DetermineCardCrops();
        _timeLeft = Random.Range(30f, 60f);
        _cardWorthAmount = Random.Range(1, 100);
        _cardAmountText.text = _cardWorthAmount.ToString("D2");
        _slider.maxValue = _timeLeft;
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
            bool cardCropExistsOnCardAlready = false;

            foreach (CardCrop cardCrop in allCardCrops)
            {
                if (cardCrop.PlacedObjectTypeSO == availableCardCrops[randomIndex]) {
                    cardCrop.IncreaseAmountToCollect(Random.Range(1, 4));
                    cardCropExistsOnCardAlready = true;
                }
            }

            if (cardCropExistsOnCardAlready) { continue; }

            CardCrop newCardCrop = Instantiate(_cardCropPrefab, _cardCropLayoutGroup.transform).gameObject.GetComponent<CardCrop>();
            newCardCrop.SetPlacedObjectTypeSO(availableCardCrops[randomIndex]);
            newCardCrop.SetImageSprite();
            allCardCrops.Add(newCardCrop);
        }
    }

    private void DetectCardTime()
    {
        _timeLeft -= Time.deltaTime;
        _slider.value = _timeLeft;

        if (_timeLeft <= 0f)
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

        EconomyManager.Instance.UpdateCurrentCoinAmount(_cardWorthAmount);
        CardManager.Instance.CardCompletion(this);
        Destroy(gameObject);
    }

}
