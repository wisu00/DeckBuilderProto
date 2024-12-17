using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Takes care of board state related stuff
public class BoardManager : MonoBehaviour {
	[SerializeField] GameObject cardPrefab;
	[SerializeField] GameObject locationPrefab;
	[SerializeField] GameObject[] cardSlotsPlayer;
	[SerializeField] GameObject[] cardSlotsOpponent;
    [SerializeField] CardDataBase cardDataBase;
	[SerializeField] ManagerReferences managerReferences;

	public PhotonView OpponentsBoardManager;

	private CardBaseFunctionality[] playersCardsOnBoard = new CardBaseFunctionality[5];
	public List<CardBaseFunctionality> activeCardsOnBoard = new List<CardBaseFunctionality>();

	[SerializeField] Card startingLocationBanker;
    [SerializeField] Card startingLocationScrapper;
	[SerializeField] Card startingLocationCultist;

    public void CreateStartingLocationPlayer(CharacterClasses selectedClass) {
		if(!ClassHasStartingLocation(selectedClass)) return;

        GameObject createdLocation = Instantiate(locationPrefab, cardSlotsPlayer[4].transform);
        switch (selectedClass) {
            case CharacterClasses.Banker:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationBanker;
                break;
            case CharacterClasses.Scrapper:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationScrapper;
                break;
			case CharacterClasses.Cultist:
				createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationCultist;
				break;
			default:
                break;
        }
        createdLocation.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);
        activeCardsOnBoard.Add(createdLocation.GetComponent<CardBaseFunctionality>());
		playersCardsOnBoard[4] = createdLocation.GetComponent<CardBaseFunctionality>();

	}

	//used to check if class has assigned starting location
	private bool ClassHasStartingLocation(CharacterClasses selectedClass) {
		if(selectedClass == CharacterClasses.Banker && startingLocationBanker != null) return true;
        else if (selectedClass == CharacterClasses.Scrapper && startingLocationScrapper != null) return true;
        else if (selectedClass == CharacterClasses.Cultist && startingLocationCultist != null) return true;
		else return false;
	}

    public void CreateStartingLocationOpponent(CharacterClasses selectedClass) {
		if(!ClassHasStartingLocation(selectedClass)) return;

        GameObject createdLocation = Instantiate(locationPrefab, cardSlotsOpponent[4].transform);
        switch (selectedClass) {
            case CharacterClasses.Banker:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationBanker;
                break;
            case CharacterClasses.Scrapper:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationScrapper;
                break;
			case CharacterClasses.Cultist:
				createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationCultist;
				break;
			default:
                break;
        }
        createdLocation.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);
    }

    public void DoRoundStartEffects() {
        foreach (CardBaseFunctionality baseCard in activeCardsOnBoard) {
			baseCard.DoTurnStartEffects();
		}
    }

	private void UpdateCardsOnBoard() {
		activeCardsOnBoard.Clear();
		foreach (CardBaseFunctionality baseCard in playersCardsOnBoard) {
			if(baseCard != null) activeCardsOnBoard.Add(baseCard);
		}
    }

	public void CardWasPlayedOnBoard(CardBaseFunctionality cardThatGotPlayed, int cardSlot) {
		GameObject spawnedCard = Instantiate(cardPrefab, cardSlotsPlayer[cardSlot - 1].transform);
		spawnedCard.GetComponent<CardBaseFunctionality>().card = cardThatGotPlayed.card;
		spawnedCard.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);

		activeCardsOnBoard.Add(cardThatGotPlayed);
		UpdateCardsOnBoard();

		if(!cardThatGotPlayed.card.isCardBack()) {
			OpponentsBoardManager.RPC("OpponentsCardWasPlayedOnBoard", RpcTarget.OthersBuffered,
				cardSlot, spawnedCard.GetComponent<CardBaseFunctionality>().card.cardIndex);
		}
	}

	[PunRPC]
	void OpponentsCardWasPlayedOnBoard(int cardSlot, int cardIndex) {
		GameObject spawnedCard = Instantiate(cardPrefab, cardSlotsOpponent[cardSlot - 1].transform);
		spawnedCard.GetComponent<CardBaseFunctionality>().card = cardDataBase.GetCardWithIndex(cardIndex);
		spawnedCard.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, false);
	}

	public void RemoveCardFromBoard(int cardSlot) {
		GameObject cardToBeDestroyed = cardSlotsPlayer[cardSlot - 1].GetComponentInChildren<CardBaseFunctionality>().gameObject;
		playersCardsOnBoard[cardSlot - 1] = null;
		activeCardsOnBoard.Remove(cardToBeDestroyed.GetComponent<CardBaseFunctionality>());
		managerReferences.GetDiscardPileManager().AddCardToDiscardPile(cardToBeDestroyed.GetComponent<CardBaseFunctionality>().card);
		Destroy(cardToBeDestroyed);
		OpponentsBoardManager.RPC("RemoveCardFromBoardOpponent", RpcTarget.OthersBuffered, cardSlot);
	}
	[PunRPC]
	void RemoveCardFromBoardOpponent(int cardSlot) {
		Destroy(cardSlotsOpponent[cardSlot - 1].GetComponentInChildren<CardBaseFunctionality>().gameObject);
		managerReferences.GetDiscardPileManagerOpponent().AddCardBackToDiscardPile();
	}
}
