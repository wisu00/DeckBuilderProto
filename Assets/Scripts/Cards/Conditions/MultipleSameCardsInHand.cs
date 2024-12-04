using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultipleSameCardsInHand", menuName = "EffectConditions/MultipleSameCardsInHand")]
public class MultipleSameCardsInHand : Condition {
	public override bool CheckCondition(ManagerReferences managerReferences, Card card) {

		List<Card> cardsInHand = managerReferences.GetHandManager().GetCardsInHand();
		List<int> cardIndexesInHand = new List<int>();
		
		foreach(Card handCard in cardsInHand) {
			cardIndexesInHand.Add(handCard.cardIndex);
		}

		//if(managerReferences.GetGameManager().getMoneyPlayer() < value) return true;
		//else return false;
		return false;
	}
}
