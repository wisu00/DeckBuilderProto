using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject {
    public string cardName;
    [TextArea(2,10)] public string description;
    public int goldCost;
    public int discardValueText;

    public void OnPlay() {
         
    }
}
