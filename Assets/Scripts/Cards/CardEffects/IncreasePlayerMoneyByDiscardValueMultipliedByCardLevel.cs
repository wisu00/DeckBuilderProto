using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasePlayerMoneyByDiscardValueMultipliedByCardLevel", menuName = "Effects/IncreasePlayerMoneyByDiscardValueMultipliedByCardLevel")]
public class IncreasePlayerMoneyByDiscardValueMultipliedByCardLevel : CardEffect {

	public override void DoEffect(ManagerReferences managerReferences, Card card, CardBaseFunctionality cardBaseFunctionality) {
		int increaseAmount = card.discardValue * cardBaseFunctionality.cardLevel;

		managerReferences.GetGameManager().IncreasePlayerMoney(increaseAmount);
	}
}