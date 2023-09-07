using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellCard", menuName = "Cards/Spell", order = 1)]
public class SpellCard : Card {
	public override void OnBuy() {
		decreasePlayerMoney(buyCost);
	}

	public override void OnPlay() {
        decreasePlayerMoney(playCost);
    }

    public override void OnDiscard() {
        increasePlayerMoney(discardValue);
    }
}