using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionalEffect", menuName = "Effects/ConditionalEffect")]
public class ConditionalEffect : CardEffect {
	[SerializeField] EffectCondition condition;
	[SerializeField] CardEffect cardEffect;

	public override void DoEffect(ManagerReferences managerReferences, Card card) {
		if(condition.CheckCondition(managerReferences, card) || condition == null) {
			cardEffect.DoEffect(managerReferences, card);
		}
	}
}
