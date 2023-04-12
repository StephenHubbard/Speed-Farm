using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    public Card CurrentCard { get; private set; }

    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float timeBetweenCardSpawn = 3f;
    [SerializeField] private int maxAmountOfCards = 5;

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
    }

    public void CropHarvested(PlacedObjectTypeSO placedObjectTypeSO)
    {
        if (CurrentCard == null) { return; }

        CurrentCard.CropHarvested(placedObjectTypeSO);
    }

    public void CardCompletion(Card cardCompleted) {
        allCards.Remove(cardCompleted);
        currentAmountOfCards--;

        if (allCards.Count > 0) {
            CurrentCard = allCards[0];
        } else {
            CurrentCard = null;
        }

        if (currentAmountOfCards < maxAmountOfCards) {
            if (spawnCardRoutine != null) {
                StopCoroutine(spawnCardRoutine);
            }
            spawnCardRoutine = StartCoroutine(SpawnCardsRoutine());
        }
    }

    private void FindStartingCards()
    {
        foreach (Card card in cardContainer.GetComponentsInChildren<Card>())
        {
            allCards.Add(card);
        }

        CurrentCard = allCards[0];
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
