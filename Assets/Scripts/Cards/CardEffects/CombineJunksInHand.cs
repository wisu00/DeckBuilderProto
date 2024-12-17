using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombineJunksInHand", menuName = "Effects/CombineJunksInHand")]
public class CombineJunksInHand : CardEffect {
	public override void DoEffect(ManagerReferences managerReferences, Card card, CardBaseFunctionality cardBaseFunctionality) {
		int combinedLevel = 0;

		foreach (GameObject c in managerReferences.GetHandManager().GetCardsInHand()) {
			CardBaseFunctionality baseFunctionality = c.GetComponent<CardBaseFunctionality>();
			if (baseFunctionality.card.cardType == CardType.Junk) {
				combinedLevel += baseFunctionality.cardLevel;
			}
        }

		managerReferences.GetHandManager().RemoveJunksInHandFromTheGame();

		managerReferences.GetHandManager().CreateACardInHandWithLevel(card, combinedLevel);
	}
}
