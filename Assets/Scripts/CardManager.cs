using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    public Card CurrentCard { get; private set; }

    [SerializeField] private Transform _cardContainer;
    [SerializeField] private Transform _selectionOutline;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private float _timeBetweenCardSpawn = 3f;
    [SerializeField] private int _maxAmountOfCards = 5;
    [SerializeField] private PlacedObjectTypeSO[] _availableCardCrops;

    private int _currentAmountOfCards;
    private List<Card> _allCards = new List<Card>();
    private Coroutine _spawnCardRoutine;

    private void Start()
    {
        FindStartingCards();
        _currentAmountOfCards = _cardContainer.childCount;
        _spawnCardRoutine = StartCoroutine(SpawnCardsRoutine());
    }

    public void SetCurrentCard(Card currentCard)
    {
        this.CurrentCard = currentCard;
        StartCoroutine(SetSelectionOutlineRoutine());
    }
    
    private IEnumerator SetSelectionOutlineRoutine() {
        yield return null;
        _selectionOutline.transform.position = CurrentCard.transform.position;
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
            CurrentCard = null;
        }

        if (_currentAmountOfCards < _maxAmountOfCards) {
            if (_spawnCardRoutine != null) {
                StopCoroutine(_spawnCardRoutine);
            }
            _spawnCardRoutine = StartCoroutine(SpawnCardsRoutine());
        }

        Destroy(cardCompleted.gameObject);
    }

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
