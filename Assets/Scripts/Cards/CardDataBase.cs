using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour {
	[SerializeField] List<Card> allCards;

	public List<Card> cardsBankerTier1;
	public List<Card> cardsBankerTier2;
	public List<Card> cardsBankerTier3;
	public List<Card> cardsScrapperTier1;
	public List<Card> cardsScrapperTier2;
	public List<Card> cardsScrapperTier3;

	private void Awake() {
		for (int i = 0; i < allCards.Count; i++) {
			allCards[i].cardIndex = i;
		}
	}

	public Card GetCardWithIndex(int index) {
		return allCards[index];
	}
}
