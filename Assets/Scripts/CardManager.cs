using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    public Card CurrentCard { get; private set; }

    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform selectionOutline;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float timeBetweenCardSpawn = 3f;
    [SerializeField] private int maxAmountOfCards = 5;
    [SerializeField] private PlacedObjectTypeSO[] availableCardCrops;

    private int currentAmountOfCards;
    private List<Card> allCards = new List<Card>();
    private Coroutine spawnCardRoutine;

    private void Start()
    {
        FindStartingCards();
        currentAmountOfCards = cardContainer.childCount;
        spawnCardRoutine = StartCoroutine(SpawnCardsRoutine());
    }

    public void SetCurrentCard(Card currentCard)
    {
        this.CurrentCard = currentCard;
        StartCoroutine(SetSelectionOutlineRoutine());
    }
    
    private IEnumerator SetSelectionOutlineRoutine() {
        yield return null;
        selectionOutline.transform.position = CurrentCard.transform.position;
    }

    public PlacedObjectTypeSO[] GetAvailableCardCrops() {
        return availableCardCrops;
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
        allCards.Remove(cardCompleted);
        currentAmountOfCards--;

        if (currentAmountOfCards > 0 && CurrentCard == cardCompleted) {
            SetCurrentCard(allCards[0]);
        } else if (currentAmountOfCards > 0) {
            StartCoroutine(SetSelectionOutlineRoutine());
        } else if (currentAmountOfCards == 0) {
            CurrentCard = null;
        }

        if (currentAmountOfCards < maxAmountOfCards) {
            if (spawnCardRoutine != null) {
                StopCoroutine(spawnCardRoutine);
            }
            spawnCardRoutine = StartCoroutine(SpawnCardsRoutine());
        }

        Destroy(cardCompleted.gameObject);
    }

    private void FindStartingCards()
    {
        foreach (Card card in cardContainer.GetComponentsInChildren<Card>())
        {
            allCards.Add(card);
        }

        SetCurrentCard(allCards[0]);
    }
    

    private IEnumerator SpawnCardsRoutine() {
        while (currentAmountOfCards < maxAmountOfCards)
        {
            yield return new WaitForSeconds(timeBetweenCardSpawn);
            currentAmountOfCards++;
            Card newCard = Instantiate(cardPrefab, cardContainer.transform).GetComponent<Card>();
            allCards.Add(newCard);
        }
    }
}
