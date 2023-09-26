using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject {
    public abstract void DoEffect(GameManager gameManager, Card card);
}