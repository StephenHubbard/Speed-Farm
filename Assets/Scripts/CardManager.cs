using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    public Card CurrentCard { get; private set; }

    [SerializeField] private Transform cardContainer;

    private List<Card> allCards;

    private void Start() {
        foreach (Transform card in cardContainer)
        {
            allCards.Add(card.GetComponent<Card>());
        }

        CurrentCard = allCards[0];
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

        if (allCards.Count > 0) {
            CurrentCard = allCards[0];
        } else {
            CurrentCard = null;
        }
    }
}
