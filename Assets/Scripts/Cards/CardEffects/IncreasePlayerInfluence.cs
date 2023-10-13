using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasePlayerInfluence", menuName = "Effects/IncreasePlayerInfluence")]
public class IncreasePlayerInfluence : CardEffect {

	[Range(0, 30)] [SerializeField] int increaseAmount = 0;

	public override void DoEffect(ManagerReferences managerReferences, Card card) {
		managerReferences.GetInfluenceBarManager().IncreasePlayerInfluence(increaseAmount);
	}
}