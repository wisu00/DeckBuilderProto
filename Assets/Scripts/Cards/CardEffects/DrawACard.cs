using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawACard", menuName = "Effects/DrawACard")]
public class DrawACard : CardEffect {
	public override void DoEffect(ManagerReferences managerReferences, Card card, CardBaseFunctionality cardBaseFunctionality) {
        managerReferences.GetDeckManager().DrawCard();
    }
}
