using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecreasePlayerMoneyEqualToPlayCost", menuName = "Cards/Effects/DecreasePlayerMoneyEqualToPlayCost", order = 2)]
public class DecreasePlayerMoneyEqualToPlayCost : CardEffect {
	public override void DoEffect(GameManager gameManager, Card card) {
		gameManager.DecreasePlayerMoney(card.playCost);
	}
}