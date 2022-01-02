using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Deck : MonoBehaviour
{   
    [SerializeField]Hand hand;
    [SerializeField]DiscardPile discardPile;
    [SerializeField]List<Card> cardsInDeck;
    
    public PhotonView photonViewFromOpponentsDeck;

    public void DrawCard(){
        if(cardsInDeck.Count > 0) {
            if(!cardsInDeck[0].isCardBack()) {
                photonViewFromOpponentsDeck.RPC("OpponentDrawsACard", RpcTarget.OthersBuffered);
            }
            hand.DrawCard(cardsInDeck[0]);
            cardsInDeck.RemoveAt(0);
        }
        else if(discardPile.GetDiscardPileSize() > 0) {
           discardPile.ShuffleDiscardPileToDeck();
           DrawCard();
        }
    }

    [PunRPC]
    void OpponentDrawsACard () {
        DrawCard();
    }

    public void ShuffleDiscardPileToDeck(List<Card> newDeck) {
        cardsInDeck = new List<Card>(newDeck);
    }
}
