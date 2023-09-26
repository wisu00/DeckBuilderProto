using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasePlayerMoney", menuName = "Cards/Effects/IncreasePlayerMoney", order = 2)]
public class IncreasePlayerMoney : CardEffect {

	[Range(0, 20)] [SerializeField] int increaseAmount = 0;

	public override void DoEffect(GameManager gameManager, Card card) {
		gameManager.IncreasePlayerMoney(increaseAmount);
	}
}