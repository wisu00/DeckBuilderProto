using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Takes care of board state related stuff
public class BoardManager : MonoBehaviour {
	[SerializeField] GameObject cardPrefab;
	[SerializeField] GameObject locationPrefab;
	[SerializeField] GameObject[] toolSlotsPlayer;
	[SerializeField] GameObject[] toolSlotsOpponent;
	[SerializeField] GameObject locationSlotPlayer;
    [SerializeField] GameObject locationSlotOpponent;
    [SerializeField] CardDataBase cardDataBase;
	[SerializeField] ManagerReferences managerReferences;

	public PhotonView OpponentsBoardManager;

	public static List<CardThatStaysOnBoard> activeCardsOnBoard = new List<CardThatStaysOnBoard>();

	[SerializeField] LocationCard startingLocationBanker;
    [SerializeField] LocationCard startingLocationScrapper;

    public void CreateStartingLocationPlayer(CharacterClasses selectedClass) {
        GameObject createdLocation = Instantiate(locationPrefab, locationSlotPlayer.transform);
        switch (selectedClass) {
            case CharacterClasses.Banker:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationBanker;
                break;
            case CharacterClasses.Scrapper:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationScrapper;
                break;
            default:
                break;
        }
        createdLocation.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);
        activeCardsOnBoard.Add((CardThatStaysOnBoard)createdLocation.GetComponent<CardBaseFunctionality>().card);
    }

    public void CreateStartingLocationOpponent(CharacterClasses selectedClass) {
        GameObject createdLocation = Instantiate(locationPrefab, locationSlotOpponent.transform);
        switch (selectedClass) {
            case CharacterClasses.Banker:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationBanker;
                break;
            case CharacterClasses.Scrapper:
                createdLocation.GetComponent<CardBaseFunctionality>().card = startingLocationScrapper;
                break;
            default:
                break;
        }
        createdLocation.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);
    }

    public void DoRoundStartEffects() {
        foreach (CardThatStaysOnBoard card in activeCardsOnBoard) {
			card.TurnStartEffects();
		}
    }

	public void CardWasPlayedOnBoard(Card cardThatGotPlayed, int toolSlot) {
		GameObject spawnedCard = Instantiate(cardPrefab, toolSlotsPlayer[toolSlot - 1].transform);
		spawnedCard.GetComponent<CardBaseFunctionality>().card = cardThatGotPlayed;
		spawnedCard.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);

		activeCardsOnBoard.Add((CardThatStaysOnBoard)spawnedCard.GetComponent<CardBaseFunctionality>().card);

		if(!cardThatGotPlayed.isCardBack()) {
			OpponentsBoardManager.RPC("OpponentsCardWasPlayedOnBoard", RpcTarget.OthersBuffered,
				toolSlot, spawnedCard.GetComponent<CardBaseFunctionality>().card.cardIndex);
		}
	}

	[PunRPC]
	void OpponentsCardWasPlayedOnBoard(int toolSlot, int cardIndex) {
		GameObject spawnedCard = Instantiate(cardPrefab, toolSlotsOpponent[toolSlot - 1].transform);
		spawnedCard.GetComponent<CardBaseFunctionality>().card = cardDataBase.GetCardWithIndex(cardIndex);
		spawnedCard.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, false);
	}

	public void RemoveToolFromBoard(int toolSlot) {
		GameObject cardToBeDestroyed = toolSlotsPlayer[toolSlot - 1].GetComponentInChildren<CardBaseFunctionality>().gameObject;
		managerReferences.GetDiscardPileManager().AddCardToDiscardPile(cardToBeDestroyed.GetComponent<CardBaseFunctionality>().card);
		Destroy(cardToBeDestroyed);
		OpponentsBoardManager.RPC("RemoveToolFromBoardOpponent", RpcTarget.OthersBuffered, toolSlot);
	}
	[PunRPC]
	void RemoveToolFromBoardOpponent(int toolSlot) {
		Destroy(toolSlotsOpponent[toolSlot - 1].GetComponentInChildren<CardBaseFunctionality>().gameObject);
		managerReferences.GetDiscardPileManagerOpponent().AddCardBackToDiscardPile();
	}
}
