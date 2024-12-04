using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class DeckManager : MonoBehaviour {   

    [SerializeField]HandManager hand;
    [SerializeField]DiscardPileManager discardPile;
    [SerializeField]TMP_Text infoText;
    [SerializeField] Card cardBack;

    [SerializeField] List<Card> startingCardsBanker;
    [SerializeField] List<Card> startingCardsCultist;
    [SerializeField] List<Card> startingCardsScrapper;

    List<Card> cardsInDeck = new List<Card>();
    
    public PhotonView photonViewFromOpponentsDeck;
    PhotonView photonView;

    CharacterClasses playersCharacterClass;

    public void SetPlayerCharacter(CharacterClasses playerCharacter) {
        playersCharacterClass = playerCharacter;
        InitializePlayerDeck();
    }

    private void Awake() {
		photonView = GetComponent<PhotonView>();
	}

	private void InitializePlayerDeck() {
        switch (playersCharacterClass) {
            case CharacterClasses.Banker:
                cardsInDeck = startingCardsBanker;
                break;
            case CharacterClasses.Scrapper:
                cardsInDeck = startingCardsScrapper;
                break;
            case CharacterClasses.Cultist:
                cardsInDeck = startingCardsCultist;
                break;
            default:
                Debug.LogError("class not specified");
                break;
        }

        UpdateInfoBox();

        photonViewFromOpponentsDeck.RPC("InitializeOpponentsDeckRPC", RpcTarget.OthersBuffered, cardsInDeck.Count);
    }

    [PunRPC]
    void InitializeOpponentsDeckRPC(int amountOfStartingCards) {
        //cardsInDeck.Clear();
        for(int i=0; i<amountOfStartingCards; i++) {
            cardsInDeck.Add(cardBack);
        }
        UpdateInfoBox();
    }

    public void DrawCard(){
        if(cardsInDeck.Count > 0) {
            if(cardsInDeck[0].cardType != CardType.CardBack) {
                photonViewFromOpponentsDeck.RPC("OpponentDrawsACardRPC", RpcTarget.OthersBuffered);
            }
            hand.DrawCard(cardsInDeck[0]);
            cardsInDeck.RemoveAt(0);
        }
        else if(discardPile.GetDiscardPileSize() > 0) {
           discardPile.ShuffleDiscardPileToDeck();
           DrawCard();
        }
        UpdateInfoBox();
    }

    public void UpdateInfoBox() {        
        infoText.text = "Cards in deck: " + cardsInDeck.Count;
    }

    //called once on the first turn if opponent is the starting player
    public void OpponentDrawsACard() {
		photonView.RPC("OpponentDrawsACardRPC", RpcTarget.OthersBuffered);
	}

    [PunRPC]
    void OpponentDrawsACardRPC() {
        DrawCard();
    }

    public void ShuffleDiscardPileToDeck(List<Card> newDeck) {
        cardsInDeck = new List<Card>(newDeck);

        IListExtensions.Shuffle(cardsInDeck);

		UpdateInfoBox();
    }
}
