using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoneyLessThan", menuName = "EffectConditions/PlayerMoneyLessThan")]
public class PlayerMoneyLessThan : EffectCondition {
	[SerializeField] int value = 5;

	public override bool CheckCondition(ManagerReferences managerReferences, Card card) {
		if(managerReferences.GetGameManager().getMoneyPlayer() < value) return true;
		else return false;
	}
}
