using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class DiscardPileManager : MonoBehaviour {
    [SerializeField]DeckManager deck;
    [SerializeField]TMP_Text infoText;
    List<Card> cardsInDiscardPile = new List<Card> ();
    [SerializeField] Card cardBack;

    private void Start() {
        UpdateInfoBox();
    }

    public void ShuffleDiscardPileToDeck(){
        deck.ShuffleDiscardPileToDeck(cardsInDiscardPile);
        cardsInDiscardPile.Clear();
        UpdateInfoBox();
    }

    public void AddCardToDiscardPile(Card cardToBeAdded) {
        cardsInDiscardPile.Add(cardToBeAdded);
        UpdateInfoBox();
    }

	public void AddCardBackToDiscardPile() {
        cardsInDiscardPile.Add(cardBack);
		UpdateInfoBox();
	}

    public void UpdateInfoBox() {        
        infoText.text = "Cards in pile: " + cardsInDiscardPile.Count;
    }

    public int GetDiscardPileSize() {
        return cardsInDiscardPile.Count;
    }
}
