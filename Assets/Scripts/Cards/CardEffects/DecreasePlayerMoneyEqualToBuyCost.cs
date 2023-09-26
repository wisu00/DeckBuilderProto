using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecreasePlayerMoneyEqualToBuyCost", menuName = "Cards/Effects/DecreasePlayerMoneyEqualToBuyCost", order = 2)]
public class DecreasePlayerMoneyEqualToBuyCost : CardEffect {
	public override void DoEffect(GameManager gameManager, Card card) {
		gameManager.DecreasePlayerMoney(card.buyCost);
	}
}