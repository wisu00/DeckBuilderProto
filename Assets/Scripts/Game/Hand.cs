using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]List<Card> hand;

    public void DrawCard(Card drawnCard){
        hand.Add(drawnCard);
    }
}
