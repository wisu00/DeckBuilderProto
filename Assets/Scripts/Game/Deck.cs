using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]List<Card> cardsInDeck;
    [SerializeField]Hand hand;

    public void DrawCard(){
        hand.DrawCard(cardsInDeck[0]);
        cardsInDeck.RemoveAt(0);
    }

    public void ShuffleDiscardPileToDeck(List<Card> newDeck) {
        cardsInDeck = new List<Card>(newDeck);
    }
}
