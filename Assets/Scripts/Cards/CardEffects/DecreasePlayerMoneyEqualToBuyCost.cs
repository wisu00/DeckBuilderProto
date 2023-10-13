using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecreasePlayerMoneyEqualToBuyCost", menuName = "Effects/DecreasePlayerMoneyEqualToBuyCost")]
public class DecreasePlayerMoneyEqualToBuyCost : CardEffect {
	public override void DoEffect(ManagerReferences managerReferences, Card card) {
		managerReferences.GetGameManager().DecreasePlayerMoney(card.buyCost);
	}
}