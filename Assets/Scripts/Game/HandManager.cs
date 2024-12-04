using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class HandManager : MonoBehaviour {
    [SerializeField] GameObject handArea;
    [SerializeField] DiscardPileManager discardPile;
    [SerializeField] GameObject cardPrefab;
	[SerializeField] ManagerReferences managerReferences;
	[SerializeField] GameObject[] toolSlotsPlayer;
	[SerializeField] GameObject[] toolSlotsOpponent;

	[SerializeField] CardDataBase cardDataBase;

	private List<Card> hand = new List<Card>();

	public PhotonView photonViewFromOpponentsHand;

    private int maxHandSize = 5;
    private List<GameObject> physicalCardsInHand = new List<GameObject>();

	public static List<Card> cardsThatTrackChangesInHand = new List<Card>();
	private void OnHandChange() {
		//updates list of tracking cards
		cardsThatTrackChangesInHand.Clear();
		foreach (Card card in hand) {
			if(card.CardHasHandChangeEffects()) cardsThatTrackChangesInHand.Add(card);
		}
        foreach (Card card in managerReferences.GetBoardManager().activeCardsOnBoard) {
			if(card.CardHasHandChangeEffects()) cardsThatTrackChangesInHand.Add(card);
		}
		//trigger all tracking cards
        foreach (Card card in cardsThatTrackChangesInHand) {
			card.HandChangesEffects();
		}
	}

	public GameObject GetHandArea() {
        return handArea;
	}

	public void DrawCard(Card drawnCard) {
		//hand is full -> card goes to discard
        if(hand.Count == maxHandSize) {
            discardPile.AddCardToDiscardPile(drawnCard);
            //Debug.Log(drawnCard.cardName + " was put to discard pile");
        }
        else {
            hand.Add(drawnCard);
            //Debug.Log(drawnCard.cardName + " was drawn");
            
            GameObject cardThatWasDrawn = Instantiate(cardPrefab, handArea.transform);
            cardThatWasDrawn.GetComponent<CardBaseFunctionality>().card = drawnCard;
			if(!drawnCard.isCardBack()) {
				cardThatWasDrawn.GetComponent<CardBaseFunctionality>().UpdateValues(managerReferences);
				OnHandChange();
			}
			
            physicalCardsInHand.Add(cardThatWasDrawn);
			OrganiseHand();
		}
    }

	public List<Card> GetCardsInHand() {
		return hand;
	}

    public void OrganiseHand() {
        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width; //returns 0 for some reason
        cardWidth = 78.05f;
        float xOffSetWhenCardCountIsEven = 0f;
        if(physicalCardsInHand.Count % 2 == 0) {
            xOffSetWhenCardCountIsEven = cardWidth/2;
        }
        for(int i=0; i<physicalCardsInHand.Count; i++) {
            RectTransform picture = physicalCardsInHand[i].GetComponent<RectTransform>();
            picture.anchoredPosition = new Vector2(cardWidth*(i-physicalCardsInHand.Count/2) + xOffSetWhenCardCountIsEven, picture.anchoredPosition.y); 
        }
	}

    [PunRPC]
    void OpponentsCardGoesToDiscardPile (int cardPlaceInHand) {
        Card cardThatGotPlayed = hand[cardPlaceInHand];
        GameObject physicalCard = physicalCardsInHand[cardPlaceInHand];
		Debug.Log(cardThatGotPlayed.cardType);
		Destroy(physicalCard);
		discardPile.AddCardToDiscardPile(cardThatGotPlayed);
		hand.Remove(cardThatGotPlayed);
		physicalCardsInHand.Remove(physicalCard);
		OrganiseHand();
	}

    public void MoveCardToDiscardPile(Card cardThatGotPlayed, GameObject physicalCard) {
        int cardPlaceInHand = hand.IndexOf(cardThatGotPlayed);
        photonViewFromOpponentsHand.RPC("OpponentsCardGoesToDiscardPile", RpcTarget.OthersBuffered, cardPlaceInHand);
        Destroy(physicalCard);
        discardPile.AddCardToDiscardPile(cardThatGotPlayed);
        hand.Remove(cardThatGotPlayed);
        physicalCardsInHand.Remove(physicalCard);
        OrganiseHand();
		OnHandChange();
	}

    public void RemoveCardFromHand(Card cardThatGotPlayed, GameObject physicalCard) {
		int cardPlaceInHand = hand.IndexOf(cardThatGotPlayed);
		photonViewFromOpponentsHand.RPC("RemoveCardFromHandOpponent", RpcTarget.OthersBuffered, cardPlaceInHand);
		Destroy(physicalCard);
		hand.Remove(cardThatGotPlayed);
		physicalCardsInHand.Remove(physicalCard);
		OrganiseHand();
		OnHandChange();
	}

	[PunRPC]
	void RemoveCardFromHandOpponent(int cardPlaceInHand) {
		Card cardThatGotPlayed = hand[cardPlaceInHand];
		GameObject physicalCard = physicalCardsInHand[cardPlaceInHand];
		hand.Remove(cardThatGotPlayed);
		physicalCardsInHand.Remove(physicalCard);
		Destroy(physicalCard);
		OrganiseHand();
	}

	public void CardWasPlayedOnBoard(Card cardThatGotPlayed, GameObject physicalCard, int toolSlot) {
		GameObject spawnedCard = Instantiate(cardPrefab, toolSlotsPlayer[toolSlot - 1].transform);
        spawnedCard.GetComponent<CardBaseFunctionality>().card = cardThatGotPlayed;
		spawnedCard.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);

		if(!cardThatGotPlayed.isCardBack()) {
			int cardPlaceInHand = hand.IndexOf(cardThatGotPlayed);
			photonViewFromOpponentsHand.RPC("OpponentsCardWasPlayedOnBoard", RpcTarget.OthersBuffered, cardPlaceInHand, 
                toolSlot, spawnedCard.GetComponent<CardBaseFunctionality>().card.cardIndex);
		}

		Destroy(physicalCard);
		hand.Remove(cardThatGotPlayed);
		physicalCardsInHand.Remove(physicalCard);
		OrganiseHand();
		OnHandChange();
	}

	[PunRPC]
	void OpponentsCardWasPlayedOnBoard(int cardPlaceInHand, int toolSlot, int cardIndex) {
		GameObject spawnedCard = Instantiate(cardPrefab, toolSlotsOpponent[toolSlot-1].transform);
        spawnedCard.GetComponent<CardBaseFunctionality>().card = cardDataBase.GetCardWithIndex(cardIndex);
		spawnedCard.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, false);

		Card cardThatGotPlayed = hand[cardPlaceInHand];
		GameObject physicalCard = physicalCardsInHand[cardPlaceInHand];
		hand.Remove(cardThatGotPlayed);
		physicalCardsInHand.Remove(physicalCard);
		Destroy(physicalCard);
		OrganiseHand();
	}
}
