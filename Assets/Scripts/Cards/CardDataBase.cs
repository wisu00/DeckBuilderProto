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

	public Card GetCardWithIndex(int index) {
		return allCards[index];
	}
}
