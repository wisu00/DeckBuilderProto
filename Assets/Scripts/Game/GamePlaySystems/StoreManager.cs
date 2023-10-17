using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour {
	List<Card> cardsBankerTier1;
	List<Card> cardsBankerTier2;
	List<Card> cardsBankerTier3;
	List<Card> cardsScrapperTier1;
	List<Card> cardsScrapperTier2;
	List<Card> cardsScrapperTier3;
	List<Card> cardsCultistTier1;
	List<Card> cardsCultistTier2;
	List<Card> cardsCultistTier3;

	[SerializeField] GameObject[] cardStoreDisplayAreas; //5 slots for cards

	[SerializeField] int numberOfCardsInStoreTier1 = 2;
	[SerializeField] int numberOfCardsInStoreTier2 = 2;
	[SerializeField] int numberOfCardsInStoreTier3 = 1;

	public PhotonView photonView;
	[SerializeField] CardDataBase cardDataBase;
	[SerializeField] GameObject cardPrefab;
	[SerializeField] ManagerReferences managerReferences;

	[SerializeField] List<Card> displayedCardsTier1;
	[SerializeField] List<Card> displayedCardsTier2;
	[SerializeField] List<Card> displayedCardsTier3;

	private List<GameObject> physicalCardsInStore = new List<GameObject>();

	CharacterClasses playersCharacterClass;

	private void Awake() {
		cardsBankerTier1 = cardDataBase.cardsBankerTier1;
		cardsBankerTier2 = cardDataBase.cardsBankerTier2;
		cardsBankerTier3 = cardDataBase.cardsBankerTier3;
		cardsScrapperTier1 = cardDataBase.cardsScrapperTier1;
		cardsScrapperTier2 = cardDataBase.cardsScrapperTier2;
		cardsScrapperTier3 = cardDataBase.cardsScrapperTier3;
		cardsCultistTier1 = cardDataBase.cardsCultistTier1;
		cardsCultistTier2 = cardDataBase.cardsCultistTier2;
		cardsCultistTier3 = cardDataBase.cardsCultistTier3;
	}

	public void SetPlayerCharacter(CharacterClasses playerCharacter) {
		playersCharacterClass = playerCharacter;
		Debug.Log("character set to " + playersCharacterClass);
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
			case CharacterClasses.Cultist:
				IListExtensions.Shuffle(cardsCultistTier1);
				IListExtensions.Shuffle(cardsCultistTier2);
				IListExtensions.Shuffle(cardsCultistTier3);
				break;
			default:
				break;
		}
	}

	public void LoadStore() {
		int cardPos = 0; //used to determine where card will spawn
		switch(playersCharacterClass) {
			case CharacterClasses.Banker:
				//Debug.Log("loading banker cards");
				for(int i = 0; i < numberOfCardsInStoreTier1; i++) {
					displayedCardsTier1.Add(cardsBankerTier1[0]);
					SpawnCardInStore(cardsBankerTier1[0], cardPos);
					cardPos++;
					cardsBankerTier1.RemoveAt(0);
				}
				for(int i = 0; i < numberOfCardsInStoreTier2; i++) {
					displayedCardsTier2.Add(cardsBankerTier2[0]);
					SpawnCardInStore(cardsBankerTier2[0], cardPos);
					cardPos++;
					cardsBankerTier2.RemoveAt(0);
				}
				for(int i = 0; i < numberOfCardsInStoreTier3; i++) {
					displayedCardsTier3.Add(cardsBankerTier3[0]);
					SpawnCardInStore(cardsBankerTier3[0], cardPos);
					cardPos++;
					cardsBankerTier3.RemoveAt(0);
				}
				break;
			case CharacterClasses.Scrapper:
				//Debug.Log("loading scrapper cards");
				for(int i = 0; i < numberOfCardsInStoreTier1; i++) {
					if(cardsScrapperTier1.Count > 0) {// doesn't create cards in store if store decks are empty
						displayedCardsTier1.Add(cardsScrapperTier1[0]);
						SpawnCardInStore(cardsScrapperTier1[0], cardPos);
						cardsScrapperTier1.RemoveAt(0);
					} 
					cardPos++;
				}
				for(int i = 0; i < numberOfCardsInStoreTier2; i++) {
					if(cardsScrapperTier2.Count > 0) {
						displayedCardsTier2.Add(cardsScrapperTier2[0]);
						SpawnCardInStore(cardsScrapperTier2[0], cardPos);
						cardsScrapperTier2.RemoveAt(0);
					}
					cardPos++;
				}
				for(int i = 0; i < numberOfCardsInStoreTier3; i++) {
					if(cardsScrapperTier3.Count > 0) {
						displayedCardsTier3.Add(cardsScrapperTier3[0]);
						SpawnCardInStore(cardsScrapperTier3[0], cardPos);
						cardsScrapperTier3.RemoveAt(0);
					}
					cardPos++;

				}
				break;
			case CharacterClasses.Cultist:
				//Debug.Log("loading cultist cards");
				for(int i = 0; i < numberOfCardsInStoreTier1; i++) {
					if(cardsCultistTier1.Count > 0) {// doesn't create cards in store if store decks are empty
						displayedCardsTier1.Add(cardsCultistTier1[0]);
						SpawnCardInStore(cardsCultistTier1[0], cardPos);
						cardsCultistTier1.RemoveAt(0);
					}
					cardPos++;
				}
				for(int i = 0; i < numberOfCardsInStoreTier2; i++) {
					if(cardsCultistTier2.Count > 0) {
						displayedCardsTier2.Add(cardsCultistTier2[0]);
						SpawnCardInStore(cardsCultistTier2[0], cardPos);
						cardsCultistTier2.RemoveAt(0);
					}
					cardPos++;
				}
				for(int i = 0; i < numberOfCardsInStoreTier3; i++) {
					if(cardsCultistTier3.Count > 0) {
						displayedCardsTier3.Add(cardsCultistTier3[0]);
						SpawnCardInStore(cardsCultistTier3[0], cardPos);
						cardsCultistTier3.RemoveAt(0);
					}
					cardPos++;

				}
				break;
			default: Debug.Log("character class not specified"); break;
		}
	}

	public void EmptyStore() {
		//needs functionality that adds cards to store piles
		switch(playersCharacterClass) {
			case CharacterClasses.Banker:
				cardsBankerTier1.AddRange(displayedCardsTier1);
				cardsBankerTier2.AddRange(displayedCardsTier2);
				cardsBankerTier3.AddRange(displayedCardsTier3);
				break;
			case CharacterClasses.Scrapper:
				cardsScrapperTier1.AddRange(displayedCardsTier1);
				cardsScrapperTier2.AddRange(displayedCardsTier2);
				cardsScrapperTier3.AddRange(displayedCardsTier3);
				break;
			case CharacterClasses.Cultist:
				cardsCultistTier1.AddRange(displayedCardsTier1);
				cardsCultistTier2.AddRange(displayedCardsTier2);
				cardsCultistTier3.AddRange(displayedCardsTier3);
				break;
			default: Debug.Log("character class not specified"); break;
		}
		displayedCardsTier1.Clear();
		displayedCardsTier2.Clear();
		displayedCardsTier3.Clear();

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

	private void SpawnCardInStore(Card card, int cardPos) {
		photonView.RPC("SpawnCardInOpponentsStore", RpcTarget.OthersBuffered, card.cardIndex, cardPos);
		GameObject cardThatWasDrawn;
		cardThatWasDrawn = Instantiate(cardPrefab, cardStoreDisplayAreas[cardPos].transform); 
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().card = card;
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().UpdateValuesInStore(managerReferences, true, cardPos);
		physicalCardsInStore.Add(cardThatWasDrawn);
	}

	[PunRPC]
	void SpawnCardInOpponentsStore(int cardIndex, int cardPos) {
		GameObject cardThatWasDrawn;
		cardThatWasDrawn = Instantiate(cardPrefab, cardStoreDisplayAreas[cardPos].transform);
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().card = cardDataBase.GetCardWithIndex(cardIndex);
		cardThatWasDrawn.GetComponent<CardBaseFunctionality>().UpdateValuesInStore(managerReferences, false, cardPos);
		physicalCardsInStore.Add(cardThatWasDrawn);
	}

	public void CardIsBought(Card card, GameObject physicalCard, int cardPosInStore) {
		switch(card.tier) {
			case 1: displayedCardsTier1.Remove(card); break;
			case 2: displayedCardsTier2.Remove(card); break;
			case 3: displayedCardsTier3.Remove(card); break;
			default: Debug.Log(card.name + " doesn't have specified tier"); break;
		}
		Destroy(physicalCard);
		photonView.RPC("RemoveCardFromOpponentsStore", RpcTarget.OthersBuffered, cardPosInStore);
	}

	[PunRPC]
	void RemoveCardFromOpponentsStore(int cardPosInStore) {
		Destroy(physicalCardsInStore[cardPosInStore]);
	}
}
