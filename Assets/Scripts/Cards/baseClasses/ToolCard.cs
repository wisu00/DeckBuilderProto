using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolCard", menuName = "Cards/Tool", order = 2)]
public class ToolCard : Card {
	public override void OnBuy() {
		
	}

	public override void OnPlay() {
        decreasePlayerMoney(playCost);
    }

    public override void OnDiscard() {
        increasePlayerMoney(discardValue);
    }
}