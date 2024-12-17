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

	[SerializeField] private Card cardBack;

	public static List<CardBaseFunctionality> cardsThatTrackChangesInHand = new List<CardBaseFunctionality>();
	bool cardsThatTrackChangesInHandHaveBeenChanged = false;
	private void OnHandChange() {
		cardsThatTrackChangesInHandHaveBeenChanged = false;
		//updates list of tracking cards
		cardsThatTrackChangesInHand.Clear();
		foreach (GameObject physicalCard in physicalCardsInHand) {
			CardBaseFunctionality baseCard = physicalCard.GetComponent<CardBaseFunctionality>();
			if(baseCard.card.CardHasHandChangeEffects()) cardsThatTrackChangesInHand.Add(baseCard);
		}
        foreach (CardBaseFunctionality baseCard in managerReferences.GetBoardManager().activeCardsOnBoard) {
			if(baseCard.card.CardHasHandChangeEffects()) cardsThatTrackChangesInHand.Add(baseCard);
		}
		//trigger all tracking cards
		//for (int i = 0; i < cardsThatTrackChangesInHand.Count; i++) {
		//	card.HandChangesEffects();
		//}

		
		for(int i=0; i<cardsThatTrackChangesInHand.Count; i++) {
			if(cardsThatTrackChangesInHandHaveBeenChanged) break;
			else cardsThatTrackChangesInHand[i].DoHandChangeEffects();
		}
		if(cardsThatTrackChangesInHandHaveBeenChanged) OnHandChange();
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
			Debug.Log("Drawing a card");
            hand.Add(drawnCard);
            //Debug.Log(drawnCard.cardName + " was drawn");
            
            GameObject cardThatWasDrawn = Instantiate(cardPrefab, handArea.transform);
			physicalCardsInHand.Add(cardThatWasDrawn);
			
			if(!drawnCard.isCardBack()) {
				cardThatWasDrawn.GetComponent<CardBaseFunctionality>().card = drawnCard;
				cardThatWasDrawn.GetComponent<CardBaseFunctionality>().UpdateValues(managerReferences);
				OnHandChange();
			}
			
			OrganiseHand();
		}
    }

	public List<GameObject> GetCardsInHand() {
		return physicalCardsInHand;
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
		OnHandChange();
		OrganiseHand();
	}

	//can later be replaced with something using card id
	public void RemoveJunksInHandFromTheGame() {
		cardsThatTrackChangesInHandHaveBeenChanged = true;

		for(int i = 0; i < physicalCardsInHand.Count; i++) {
			if(physicalCardsInHand[i].GetComponent<CardBaseFunctionality>().card.cardType == CardType.Junk) {
				Destroy(physicalCardsInHand[i]);
				physicalCardsInHand.Remove(physicalCardsInHand[i]);
				i--;
			}
		}
		List<Card> newHand = new List<Card>();
        foreach (Card card in hand) {
            if(card.cardType == CardType.Junk) {
				int cardPlaceInHand = hand.IndexOf(card);
				photonViewFromOpponentsHand.RPC("RemoveCardFromHandOpponent", RpcTarget.OthersBuffered, cardPlaceInHand);
			}
			else newHand.Add(card);
		}
		hand = newHand;
		OrganiseHand();
	}

	public void CreateACardInHandWithLevel(Card createdCard, int level) {
		hand.Add(createdCard);

		GameObject cardThatWasCreated = Instantiate(cardPrefab, handArea.transform);
		cardThatWasCreated.GetComponent<CardBaseFunctionality>().cardLevel = level;
		cardThatWasCreated.GetComponent<CardBaseFunctionality>().card = createdCard;
		cardThatWasCreated.GetComponent<CardBaseFunctionality>().UpdateValues(managerReferences);

		physicalCardsInHand.Add(cardThatWasCreated);
		photonViewFromOpponentsHand.RPC("OpponentCreatesCardInHand", RpcTarget.OthersBuffered);
		OnHandChange();
		OrganiseHand();
	}

	[PunRPC]
	void OpponentCreatesCardInHand() {
		//not set to an instance of a object???
		hand.Add(cardBack);
		GameObject cardThatWasCreated = Instantiate(cardPrefab, handArea.transform);
		cardThatWasCreated.GetComponent<CardBaseFunctionality>().card = cardBack;
		cardThatWasCreated.GetComponent<CardBaseFunctionality>().UpdateValues(managerReferences);
		physicalCardsInHand.Add(cardThatWasCreated);
		OrganiseHand();
	}

	public void RemoveCardFromHand(Card cardThatGotPlayed, GameObject physicalCard) {
		int cardPlaceInHand = hand.IndexOf(cardThatGotPlayed);
		photonViewFromOpponentsHand.RPC("RemoveCardFromHandOpponent", RpcTarget.OthersBuffered, cardPlaceInHand);
		Destroy(physicalCard);
		hand.Remove(cardThatGotPlayed);
		physicalCardsInHand.Remove(physicalCard);
		OrganiseHand();
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
