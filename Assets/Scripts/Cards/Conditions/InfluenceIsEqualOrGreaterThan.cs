using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfluenceIsEqualOrGreaterThan", menuName = "EffectConditions/InfluenceIsEqualOrGreaterThan")]
public class InfluenceIsEqualOrGreaterThan : Condition {
	[SerializeField] int value = 15;

	public override bool CheckCondition(ManagerReferences managerReferences, Card card) {
		if(managerReferences.GetInfluenceBarManager().GetPlayerInfluence() > value) return true;
		else return false;
	}
}
