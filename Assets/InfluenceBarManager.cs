using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfluenceBarManager : MonoBehaviour
{
    [SerializeField] private Slider player1InfluenceBar;
    [SerializeField] private Slider player2InfluenceBar;

    public void increasePlayer1Influence() {
        if(player1InfluenceBar.value + player2InfluenceBar.value + 5 <= 100) {
            player1InfluenceBar.value += 5;
        }
        else {
            float stolenInfluence = player1InfluenceBar.value + player2InfluenceBar.value + 5 - 100 ;
            player1InfluenceBar.value += 5;
            player2InfluenceBar.value -= stolenInfluence;
        }
        
    }

    public void decreasePlayer1Influence() {
        player1InfluenceBar.value -= 5;
    }

    public void increasePlayer2Influence() {
        if(player2InfluenceBar.value + player1InfluenceBar.value + 5 <= 100) {
            player2InfluenceBar.value += 5;
        }
        else {
            float stolenInfluence = player2InfluenceBar.value + player1InfluenceBar.value + 5 -100;
            player2InfluenceBar.value += 5;
            player1InfluenceBar.value -= stolenInfluence;
        }
    }

    public void decreasePlayer2Influence() {
        player2InfluenceBar.value -= 5;
    }
}
