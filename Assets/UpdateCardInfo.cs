using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateCardInfo : MonoBehaviour
{
    public Card card;
    public TMP_Text cardName;
    public TMP_Text description;
    public TMP_Text goldCost;
    public TMP_Text discardValueText;
    // Start is called before the first frame update
    void Start()
    {
        cardName.text = card.cardName;
        description.text = card.description;
        goldCost.text = card.goldCost.ToString();
        discardValueText.text = card.discardValueText.ToString();
    }
}
