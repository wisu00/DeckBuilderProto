using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour {
    [SerializeField]Deck deck;
    List<Card> cardsInDiscardPile = new List<Card> ();

    public void ShuffleDiscardPileToDeck(){
        deck.ShuffleDiscardPileToDeck(cardsInDiscardPile);
    }

    public void AddCardToDiscardPile(Card cardToBeAdded) {
        cardsInDiscardPile.Add(cardToBeAdded);
    }

    public int GetDiscardPileSize() {
        return cardsInDiscardPile.Count;
    }
}
