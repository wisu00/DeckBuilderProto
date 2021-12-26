using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject {
    public string cardName;
    [TextArea(2,10)] public string description;
    public int goldCost;
    public int discardValueText;

    public abstract void OnPlay();
}
