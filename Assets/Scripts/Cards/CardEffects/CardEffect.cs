using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject {
    public abstract void DoEffect(ManagerReferences managerReferences, Card card, CardBaseFunctionality cardBaseFunctionality);
}