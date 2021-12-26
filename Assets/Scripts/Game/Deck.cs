using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{   
    [SerializeField]Hand hand;
    [SerializeField]DiscardPile discardPile;
    [SerializeField]List<Card> cardsInDeck;
    

    public void DrawCard(){
        if(cardsInDeck.Count > 0) {
            hand.DrawCard(cardsInDeck[0]);
            cardsInDeck.RemoveAt(0);
        }
        else if(discardPile.GetDiscardPileSize() > 0) {
           discardPile.ShuffleDiscardPileToDeck();
           DrawCard();
        }
    }

    public void ShuffleDiscardPileToDeck(List<Card> newDeck) {
        cardsInDeck = new List<Card>(newDeck);
    }
}
