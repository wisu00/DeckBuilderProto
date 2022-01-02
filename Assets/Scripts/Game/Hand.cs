using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject handArea;
    [SerializeField] DiscardPile discardPile;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> hand;

    private int maxHandSize = 5;
    private List<GameObject> physicalCardsInHand = new List<GameObject>();

    public void DrawCard(Card drawnCard) {
        if(hand.Count == maxHandSize) {
            discardPile.AddCardToDiscardPile(drawnCard);
            Debug.Log(drawnCard.cardName + " was put to discard pile");
        }
        else {
            hand.Add(drawnCard);
            Debug.Log(drawnCard.cardName + " was drawn");
            
            GameObject cardThatWasDrawn = Instantiate(cardPrefab, handArea.transform);
            cardThatWasDrawn.GetComponent<CardBaseFunctionality>().card = drawnCard;
            cardThatWasDrawn.GetComponent<CardBaseFunctionality>().UpdateValues(this);
            physicalCardsInHand.Add(cardThatWasDrawn);
            OrganiseHand();
        }
    }

    public void OrganiseHand() {
        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width; //returns 0 for some reason
        cardWidth = 78.05f;
        float xOffSetWhenCardCountIsEven = 0f;
        if(physicalCardsInHand.Count % 2 == 0) {
            xOffSetWhenCardCountIsEven = cardWidth/2;
        }
        for(int i=0; i<physicalCardsInHand.Count; i++) {
            RectTransform picture = physicalCardsInHand[i].GetComponent<RectTransform>();
            picture.anchoredPosition = new Vector2(cardWidth*(i-physicalCardsInHand.Count/2) + xOffSetWhenCardCountIsEven, picture.anchoredPosition.y); 
        }
    }

    public void CardGetsPlayed(Card cardThatGotPlayed, GameObject PhysicalCard) {
        discardPile.AddCardToDiscardPile(cardThatGotPlayed);
        hand.Remove(cardThatGotPlayed);
        physicalCardsInHand.Remove(PhysicalCard);
        OrganiseHand();
    }

}
