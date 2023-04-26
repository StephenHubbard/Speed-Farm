using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : Singleton<CardManager>
{
    public Card CurrentCard { get; private set; }

    [SerializeField] private TMP_Text _cardToBuyAmountText;
    [SerializeField] private Transform _cardContainer;
    [SerializeField] private Transform _selectionOutline;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private float _timeBetweenCardSpawn = 3f;
    [SerializeField] private float _timeBetweenDecreaseCardAmount = 3f;
    [SerializeField] private int _maxAmountOfCards = 5;
    [SerializeField] private PlacedObjectTypeSO[] _availableCardCrops;

    private int _currentBuyCardAmount = 5;
    private int _currentAmountOfCards;
    private List<Card> _allCards = new List<Card>();

    private Coroutine _decreaseCardAmountRoutine;

    protected override void Awake() {
        base.Awake();
    }

    private void Start()
    {
        _currentAmountOfCards = _cardContainer.childCount;
        UpdateCardToBuyAmountText();
        _decreaseCardAmountRoutine = StartCoroutine(DecreaseCardCostAmountRoutine());
        _selectionOutline.gameObject.SetActive(false);
    }

    public void SetCurrentCard(Card currentCard)
    {
        this.CurrentCard = currentCard;
        StartCoroutine(SetSelectionOutlineRoutine());
    }

    public void BuyCardButton() {
        if (_currentAmountOfCards >= _maxAmountOfCards || EconomyManager.Instance.CurrentCoinAmount < _currentBuyCardAmount) { return; }

        _selectionOutline.gameObject.SetActive(true);
        EconomyManager.Instance.UpdateCurrentCoinAmount(-_currentBuyCardAmount);
        Card newCard = Instantiate(_cardPrefab, _cardContainer.transform).GetComponent<Card>();
        _currentAmountOfCards = _cardContainer.childCount;
        _allCards.Add(newCard);
        _currentBuyCardAmount = Mathf.RoundToInt(_currentBuyCardAmount * 1.5f);
        UpdateCardToBuyAmountText();

        if (CurrentCard == null)
        {
            SetCurrentCard(_allCards[0]);
        }

        if (_decreaseCardAmountRoutine != null) { StopCoroutine(_decreaseCardAmountRoutine); }

        _decreaseCardAmountRoutine = StartCoroutine(DecreaseCardCostAmountRoutine());

    }

    private void UpdateCardToBuyAmountText() {
        _cardToBuyAmountText.text = _currentBuyCardAmount.ToString("D2");
    }

    public PlacedObjectTypeSO[] GetAvailableCardCrops() {
        return _availableCardCrops;
    }

    public void CropHarvested(PlacedObjectTypeSO placedObjectTypeSO)
    {
        if (CurrentCard == null) { return; }

        CurrentCard.CropHarvested(placedObjectTypeSO);
    }

    public void SetCurrentCardNull() {
        CurrentCard = null;
    }

    public void CardCompletion(Card cardCompleted) {
        _allCards.Remove(cardCompleted);
        _currentAmountOfCards--;

        if (_currentAmountOfCards > 0 && CurrentCard == cardCompleted) {
            SetCurrentCard(_allCards[0]);
        } else if (_currentAmountOfCards > 0) {

            StartCoroutine(SetSelectionOutlineRoutine());
        } else if (_currentAmountOfCards == 0) {
            _selectionOutline.gameObject.SetActive(false);
            CurrentCard = null;
        }

        Destroy(cardCompleted.gameObject);
    }

    private IEnumerator DecreaseCardCostAmountRoutine() {
        while (_currentBuyCardAmount > 5)
        {
            yield return new WaitForSeconds(_timeBetweenDecreaseCardAmount);
            _currentBuyCardAmount--;
            UpdateCardToBuyAmountText();
        }
    }

    public void SetSelectionOutline() {
        StartCoroutine(SetSelectionOutlineRoutine());
    }

    private IEnumerator SetSelectionOutlineRoutine()
    {
        yield return null;
        
        if (_selectionOutline.gameObject.activeInHierarchy) {
            _selectionOutline.transform.position = CurrentCard.transform.position;
        }
    }

    // inactive script
    private void FindStartingCards()
    {
        foreach (Card card in _cardContainer.GetComponentsInChildren<Card>())
        {
            _allCards.Add(card);
        }

        SetCurrentCard(_allCards[0]);
    }
    

    private IEnumerator SpawnCardsRoutine() {
        while (_currentAmountOfCards < _maxAmountOfCards)
        {
            yield return new WaitForSeconds(_timeBetweenCardSpawn);
            _currentAmountOfCards++;
            Card newCard = Instantiate(_cardPrefab, _cardContainer.transform).GetComponent<Card>();
            _allCards.Add(newCard);
        }
    }
}
