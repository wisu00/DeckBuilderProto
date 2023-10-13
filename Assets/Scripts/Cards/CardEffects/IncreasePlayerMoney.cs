using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasePlayerMoney", menuName = "Effects/IncreasePlayerMoney")]
public class IncreasePlayerMoney : CardEffect {

	[Range(0, 20)] [SerializeField] int increaseAmount = 0;

	public override void DoEffect(ManagerReferences managerReferences, Card card) {
		managerReferences.GetGameManager().IncreasePlayerMoney(increaseAmount);
	}
}