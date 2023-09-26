using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Takes care of board state related stuff
public class BoardManager : MonoBehaviour {
	[SerializeField] GameObject cardPrefab;
	[SerializeField] GameObject[] toolSlotsPlayer;
	[SerializeField] GameObject[] toolSlotsOpponent;
	[SerializeField] CardDataBase cardDataBase;
	[SerializeField] ManagerReferences managerReferences;

	public PhotonView OpponentsBoardManager;

	public static List<CardThatStaysOnBoard> activeCardsOnBoard = new List<CardThatStaysOnBoard>();

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
}
