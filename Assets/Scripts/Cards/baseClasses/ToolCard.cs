using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolCard", menuName = "Cards/Tool", order = 2)]
public class ToolCard : CardThatStaysOnBoard {
	[SerializeField] ConditionalEffect[] turnStartEffects;

	public override void TurnStartEffects() {
        foreach (ConditionalEffect effect in turnStartEffects) {
			effect.DoEffectConditionally(managerReferences, this);
		}
    }
}