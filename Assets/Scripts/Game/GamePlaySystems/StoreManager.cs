using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour {
	[SerializeField] List<Card> cardsBankerTier1;
	[SerializeField] List<Card> cardsBankerTier2;
	[SerializeField] List<Card> cardsBankerTier3;
	[SerializeField] List<Card> cardsScrapperTier1;
	[SerializeField] List<Card> cardsScrapperTier2;
	[SerializeField] List<Card> cardsScrapperTier3;

	[SerializeField] int numberOfCardsInStoreTier1 = 3; 
	[SerializeField] int numberOfCardsInStoreTier2 = 2; 
	[SerializeField] int numberOfCardsInStoreTier3 = 1;

	public PhotonView photonView;
	[SerializeField] CardDataBase cardDataBase;
	[SerializeField] GameObject cardPrefab;
	[SerializeField] GameObject storeArea;
	[SerializeField] GameObject cardBuyArea;
	[SerializeField] DiscardPileManager discardPileManager;

	[SerializeField] List<Card> displayedCards;

	private List<GameObject> physicalCardsInStore = new List<GameObject>();

	CharacterClasses playersCharacterClass;

	public void SetPlayerCharacter(CharacterClasses playerCharacter) {
		playersCharacterClass = playerCharacter;
	}

	public void ShuffleStorePiles() {
		switch(playersCharacterClass) {
			case CharacterClasses.Banker:
				IListExtensions.Shuffle(cardsBankerTier1);
				IListExtensions.Shuffle(cardsBankerTier2);
				IListExtensions.Shuffle(cardsBankerTier3);
				break;
			case CharacterClasses.Scrapper:
				IListExtensions.Shuffle(cardsScrapperTier1);
				IListExtensions.Shuffle(cardsScrapperTier2);
				IListExtensions.Shuffle(cardsScrapperTier3);
				break;
			default:
				break;
		}
	}

	public void LoadStore() {
		switch(playersCharacterClass) {
			case CharacterClasses.Banker:
				for(int i = 0; i < numberOfCardsInStoreTier1; i++) {
					displayedCards.Add(cardsBankerTier1[0]);
					SpawnCardInStore(cardsBankerTier1[0]);
					cardsBankerTier1.RemoveAt(0);
				}
				for(int i = 0; i < numberOfCardsInStoreTier2; i++) {
					displayedCards.Add(cardsBankerTier2[0]);
					SpawnCardInStore(cardsBankerTier2[0]);
					cardsBankerTier2.RemoveAt(0);
				}
				for(int i = 0; i < numberOfCardsInStoreTier3; i++) {
					displayedCards.Add(cardsBankerTier3[0]);
					SpawnCardInStore(cardsBankerTier3[0]);
					cardsBankerTier3.RemoveAt(0);
				}
				break;
			case CharacterClasses.Scrapper:
				for(int i = 0; i < numberOfCardsInStoreTier1; i++) {
					displayedCards.Add(cardsScrapperTier1[0]);
					SpawnCardInStore(cardsScrapperTier1[0]);
					cardsScrapperTier1.RemoveAt(0);
				}
				for(int i = 0; i < numberOfCardsInStoreTier2; i++) {
					displayedCards.Add(cardsScrapperTier2[0]);
					SpawnCardInStore(cardsScrapperTier2[0]);
					cardsScrapperTier2.RemoveAt(0);
				}
				for(int i = 0; i < numberOfCardsInStoreTier3; i++) {
					displayedCards.Add(cardsScrapperTier3[0]);
					SpawnCardInStore(cardsScrapperTier3[0]);
					cardsScrapperTier3.RemoveAt(0);
				}
				break;
			default:
				break;
		}
	}

	public void EmptyStore() {
		//needs functionality that adds cards to store piles

		photonView.RPC("EmptyOpponentsStore", RpcTarget.OthersBuffered);
		foreach (GameObject physicalCard in physicalCardsInStore) {
			Destroy(physicalCard);
		}
		physicalCardsInStore.Clear();
	}

	[PunRPC]
	public void EmptyOpponentsStore() {
		foreach(GameObject physicalCard in physicalCardsInStore) {
			Destroy(physicalCard);
		}
		physicalCardsInStore.Clear();
	}

	private void SpawnCardInStore(Card card) {
		photonView.RPC("SpawnCardInOpponentsStore", RpcTarget.OthersBuffered, card.cardIndex);
		GameObject cardThatWasDrawn = Instantiate(cardPrefab, storeArea.transform);
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().card = card;
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().UpdateValuesInStore(this, discardPileManager, cardBuyArea, true);
		physicalCardsInStore.Add(cardThatWasDrawn);
	}

	[PunRPC]
	void SpawnCardInOpponentsStore(int cardIndex) {
		GameObject cardThatWasDrawn = Instantiate(cardPrefab, storeArea.transform);
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().card = cardDataBase.GetCardWithIndex(cardIndex);
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().UpdateValuesInStore(this, discardPileManager, cardBuyArea, false);
		physicalCardsInStore.Add(cardThatWasDrawn);
	}

	public void CardIsBought(Card card) {
		displayedCards.Remove(card);
	}
}
