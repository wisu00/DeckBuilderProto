using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecreasePlayerMoneyEqualToPlayCost", menuName = "Effects/DecreasePlayerMoneyEqualToPlayCost")]
public class DecreasePlayerMoneyEqualToPlayCost : CardEffect {
	public override void DoEffect(ManagerReferences managerReferences, Card card) {
		managerReferences.GetGameManager().DecreasePlayerMoney(card.playCost);
	}
}