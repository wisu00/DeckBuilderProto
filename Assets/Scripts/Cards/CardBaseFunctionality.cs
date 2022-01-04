using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBaseFunctionality : MonoBehaviour, IPointerClickHandler 
{
    public Card card;
    public HandManager hand;
    public TMP_Text cardName;
    public Image cardArt;
    public TMP_Text description;
    public TMP_Text goldCost;
    public TMP_Text discardValueText;

    public void UpdateValues(HandManager handScript) {
        card.FindReferences();
        hand = handScript;
        cardArt.sprite = card.cardArt;
        cardName.text = card.cardName;
        description.text = card.description;
        goldCost.text = card.goldCost.ToString();
        discardValueText.text = card.discardValueText.ToString();
        if(card.isCardBack()) {
            foreach (Transform child in cardArt.gameObject.transform) {
                child.gameObject.SetActive(false);
            }
        } 
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(!card.isCardBack()) {
            card.OnPlay();
            hand.CardGetsPlayed(card, gameObject);
        }
    }
}
