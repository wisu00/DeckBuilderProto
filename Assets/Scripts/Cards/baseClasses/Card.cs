using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject {
    public string cardName;
    public Sprite cardArt;
    [TextArea(2,10)] public string description;
    public int goldCost;
    public int discardValueText;

    public abstract void OnPlay();

    //gets override in cardBack
    public virtual bool isCardBack() {
        return false;
    }
}
