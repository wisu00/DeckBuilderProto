using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    //public GameObject card;
    List<Card> deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = new List<Card>();
    }
}
