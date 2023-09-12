using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardBack", menuName = "Cards/CardBack", order = 9)]
public class CardBack : Card
{
	public override void OnBuy() {
	}

	public override void OnPlay() {
        Debug.Log("Opponent played a card");
    }

    public override void OnDiscard() {
        Debug.Log("Opponent discarded a card");
    }

    public override bool isCardBack() {
        return true;
    }
}