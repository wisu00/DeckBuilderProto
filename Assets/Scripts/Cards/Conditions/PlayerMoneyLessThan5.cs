using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoneyLessThan5", menuName = "EffectConditions/PlayerMoneyLessThan5")]
public class PlayerMoneyLessThan5 : EffectCondition {
	public override bool CheckCondition(ManagerReferences managerReferences, Card card) {
		if(managerReferences.GetGameManager().getMoneyPlayer() < 5) return true;
		else return false;
	}
}
