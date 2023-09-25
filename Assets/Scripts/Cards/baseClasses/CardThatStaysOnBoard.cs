using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardThatStaysOnBoard : Card {
	public abstract override void OnBuy();

	public abstract override void OnPlay();

	public abstract override void OnDiscard();

	public abstract void TurnStartEffects();
	
}
