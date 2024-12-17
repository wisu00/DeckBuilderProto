using UnityEngine;

[CreateAssetMenu(fileName = "MultipleJunksInHand", menuName = "EffectConditions/MultipleJunksInHand")]
public class MultipleJunksInHand : Condition {

	public override bool CheckCondition(ManagerReferences managerReferences, Card card) {
		int numberOfJunksInHand = 0;

		foreach(GameObject c in managerReferences.GetHandManager().GetCardsInHand()) {
			if(c.GetComponent<CardBaseFunctionality>().card.cardType == CardType.Junk) numberOfJunksInHand++;
		}

		return numberOfJunksInHand > 1;
	}
}
