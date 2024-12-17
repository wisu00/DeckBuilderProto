using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoneyLessThan", menuName = "EffectConditions/PlayerMoneyLessThan")]
public class PlayerMoneyLessThan : Condition {
	[SerializeField] int moneyTreshold = 5;

	public override bool CheckCondition(ManagerReferences managerReferences, Card card) {
		if(managerReferences.GetGameManager().getMoneyPlayer() < moneyTreshold) {
			return true;
		}
		else return false;
	}
}
