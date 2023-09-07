using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour {
	[SerializeField] List<Card> allCards;

	private void Awake() {
		for (int i = 0; i < allCards.Count; i++) {
			allCards[i].cardIndex = i;
		}
	}

	public Card GetCardWithIndex(int index) {
		return allCards[index];
	}
}
