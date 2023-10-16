using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationCard", menuName = "Cards/Location")]
public class LocationCard : CardThatStaysOnBoard {
	[SerializeField] ConditionalEffect[] turnStartEffects;

	public override void TurnStartEffects() {
        foreach (ConditionalEffect effect in turnStartEffects) {
			effect.DoEffectConditionally(managerReferences, this);
		}
    }
}