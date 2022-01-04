using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellCard", menuName = "Cards/Spell", order = 1)]
public class SpellCard : Card
{
    public override void OnPlay() {
        PrintName();
        decreasePlayerMoney(goldCost);
    }
}