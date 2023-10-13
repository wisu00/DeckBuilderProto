using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardBack", menuName = "Cards/CardBack", order = 9)]
public class CardBack : Card
{
    public override bool isCardBack() {
        return true;
    }
}