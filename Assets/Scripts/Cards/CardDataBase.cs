using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CardDataBase : MonoBehaviour {
	//keeps list of cards up to date. However requires additions with each new folder that needs tracking
#if UNITY_EDITOR
	public string cardFolderTestCards = "ScriptableObjects/Cards/TestCards";
	public string cardFolderBanker = "ScriptableObjects/Cards/Banker";
	public string cardFolderScrapper = "ScriptableObjects/Cards/Scrapper";
	public string cardFolderCultist = "ScriptableObjects/Cards/Cultist";

	void OnValidate() {
		if(!System.IO.Directory.Exists($"{Application.dataPath}/{cardFolderBanker}") ||
		!System.IO.Directory.Exists($"{Application.dataPath}/{cardFolderScrapper}") ||
		!System.IO.Directory.Exists($"{Application.dataPath}/{cardFolderCultist}") ||
		!System.IO.Directory.Exists($"{Application.dataPath}/{cardFolderTestCards}")) {
			return;
		}

		var guidsTestCards = AssetDatabase.FindAssets("t:Card", new string[] { $"Assets/{cardFolderTestCards}" });
		var guidsBanker = AssetDatabase.FindAssets("t:Card", new string[] { $"Assets/{cardFolderBanker}" });
		var guidsScrapper = AssetDatabase.FindAssets("t:Card", new string[] { $"Assets/{cardFolderScrapper}" });
		var guidsCultist = AssetDatabase.FindAssets("t:Card", new string[] { $"Assets/{cardFolderCultist}" });

		var newCards = new Card[guidsTestCards.Length + guidsBanker.Length + guidsScrapper.Length + guidsCultist.Length];

		bool mismatch;
		if(allCards == null) {
			mismatch = true;
			allCards = newCards;
		}
		else {
			mismatch = newCards.Length != allCards.Length;
		}

		for(int i = 0; i < newCards.Length; i++) {
			for(int j = 0; j < guidsTestCards.Length; j++) {
				var path = AssetDatabase.GUIDToAssetPath(guidsTestCards[j]);
				newCards[i] = AssetDatabase.LoadAssetAtPath<Card>(path);
				mismatch |= (i < allCards.Length && allCards[i] != newCards[i]);
				i++;
			}
			for(int j = 0; j < guidsBanker.Length; j++) {
				var path = AssetDatabase.GUIDToAssetPath(guidsBanker[j]);
				newCards[i] = AssetDatabase.LoadAssetAtPath<Card>(path);
				mismatch |= (i < allCards.Length && allCards[i] != newCards[i]);
				i++;
			}
			for(int j = 0; j < guidsScrapper.Length; j++) {
				var path = AssetDatabase.GUIDToAssetPath(guidsScrapper[j]);
				newCards[i] = AssetDatabase.LoadAssetAtPath<Card>(path);
				mismatch |= (i < allCards.Length && allCards[i] != newCards[i]);
				i++;
			}
			for(int j = 0; j < guidsCultist.Length; j++) {
				var path = AssetDatabase.GUIDToAssetPath(guidsCultist[j]);
				newCards[i] = AssetDatabase.LoadAssetAtPath<Card>(path);
				mismatch |= (i < allCards.Length && allCards[i] != newCards[i]);
				i++;
			}
		}

		if(mismatch) {
		allCards = newCards;
			Debug.Log($"{name} sprite list updated.");
		}
	}
#endif

	[SerializeField] Card[] allCards;

	public List<Card> cardsBankerTier1;
	public List<Card> cardsBankerTier2;
	public List<Card> cardsBankerTier3;

	public List<Card> cardsScrapperTier1;
	public List<Card> cardsScrapperTier2;
	public List<Card> cardsScrapperTier3;

	public List<Card> cardsCultistTier1;
	public List<Card> cardsCultistTier2;
	public List<Card> cardsCultistTier3;

	private void Awake() {
		for (int i = 0; i < allCards.Length; i++) {
			allCards[i].cardIndex = i;
		}
	}

	public void UpdateCardTiers() {
        cardsBankerTier1.Clear();
		cardsBankerTier2.Clear();
		cardsBankerTier3.Clear();
		cardsScrapperTier1.Clear();
		cardsScrapperTier2.Clear();
		cardsScrapperTier3.Clear();
        cardsCultistTier1.Clear();
		cardsCultistTier2.Clear();
		cardsCultistTier3.Clear();

        foreach (Card card in allCards) {
			// checks that card belongs to one of the types with tiers
			if (!(card.cardType == CardType.Event || card.cardType == CardType.Tool || card.cardType == CardType.Location)) break;
			// assigns card to a list based on its class and tier

			switch (card.characterClass) {
				case CharacterClasses.Banker:
					if (card.tier == 1) cardsBankerTier1.Add(card);
                    else if (card.tier == 2) cardsBankerTier2.Add(card);
                    else cardsBankerTier3.Add(card);
                    break;
				case CharacterClasses.Scrapper:
                    if (card.tier == 1) cardsScrapperTier1.Add(card);
                    else if (card.tier == 2) cardsScrapperTier2.Add(card);
                    else cardsScrapperTier3.Add(card);
                    break;
                case CharacterClasses.Cultist:
                    if (card.tier == 1) cardsCultistTier1.Add(card);
                    else if (card.tier == 2) cardsCultistTier2.Add(card);
                    else cardsCultistTier3.Add(card);
                    break;
                default:
					Debug.LogError(card.cardName + "card's character class is unassigned");
					break;
			}
		}
    }

	public Card GetCardWithIndex(int index) {
		return allCards[index];
	}
}
