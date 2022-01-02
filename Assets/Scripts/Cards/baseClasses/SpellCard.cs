using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellCard", menuName = "Cards/Spell", order = 1)]
public class SpellCard : Card
{
    public string objName;
    public override void OnPlay() {
        Debug.Log("derived name: " + objName);
    }
}