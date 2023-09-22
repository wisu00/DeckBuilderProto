using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class DeckManager : MonoBehaviour
{   
    [SerializeField]HandManager hand;
    [SerializeField]DiscardPileManager discardPile;
    [SerializeField]TMP_Text infoText;
    [SerializeField]List<Card> cardsInDeck;
    
    public PhotonView photonViewFromOpponentsDeck;
    PhotonView photonView;

	private void Awake() {
		photonView = GetComponent<PhotonView>();
	}

	private void Start() {
        UpdateInfoBox();
    }

    public void DrawCard(){
        if(cardsInDeck.Count > 0) {
            if(!cardsInDeck[0].isCardBack()) {
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

    public void OpponentDrawsACard() {
		photonView.RPC("OpponentDrawsACardRPC", RpcTarget.OthersBuffered);
	}

    [PunRPC]
    void OpponentDrawsACardRPC () {
        DrawCard();
    }

    public void ShuffleDiscardPileToDeck(List<Card> newDeck) {
        cardsInDeck = new List<Card>(newDeck);

        IListExtensions.Shuffle(cardsInDeck);

		UpdateInfoBox();
    }
}
