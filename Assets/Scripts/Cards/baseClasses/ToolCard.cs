using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolCard", menuName = "Cards/Tool", order = 2)]
public class ToolCard : CardThatStaysOnBoard {
	[SerializeField] CardEffect[] turnStartEffects;

	public override void TurnStartEffects() {
        foreach (CardEffect effect in turnStartEffects) {
			effect.DoEffect(managerReferences, this);
		}
    }
}