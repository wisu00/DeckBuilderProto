using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBaseFunctionality : MonoBehaviour
    , IPointerClickHandler
{
    public Card card;
    public Hand hand;
    public TMP_Text cardName;
    public TMP_Text description;
    public TMP_Text goldCost;
    public TMP_Text discardValueText;

    public void UpdateValues(Hand handScript) {
        hand = handScript;
        cardName.text = card.cardName;
        description.text = card.description;
        goldCost.text = card.goldCost.ToString();
        discardValueText.text = card.discardValueText.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        card.OnPlay();
        hand.CardGetsPlayed(card, gameObject);
        Destroy(gameObject);
    }
}
