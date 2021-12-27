using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InfluenceBarManager : MonoBehaviour {
    [SerializeField] private Slider player1InfluenceBar;
    [SerializeField] private Slider player2InfluenceBar;

    private int changeAmount = 5; //will get replaced with parameters

    public PhotonView photonView;

    void updateValuesOnOpponent(){
        photonView.RPC("SyncValues", RpcTarget.OthersBuffered, player1InfluenceBar.value, player2InfluenceBar.value);
    }

    [PunRPC]
    void SyncValues (float player1, float player2) {
        player1InfluenceBar.value = player1;
        player2InfluenceBar.value = player2;
    }


    private void checkVariableAssignments() {
        if(!player1InfluenceBar || !player2InfluenceBar) {
            GameObject influenceBar = GameObject.FindWithTag("InfluenceBar");
            player1InfluenceBar = influenceBar.transform.GetChild(1).GetComponent<Slider>();
            player2InfluenceBar = influenceBar.transform.GetChild(2).GetComponent<Slider>();
        }
    }

    private void StealPoints(bool player1 ,float amount) {
        if(player1) {
            player1InfluenceBar.value += amount;
            player2InfluenceBar.value -= amount;
        }
        else {
            player1InfluenceBar.value -= amount;
            player2InfluenceBar.value += amount;
        }
    }

    public void IncreasePlayer1Influence() {
        if(player1InfluenceBar.value + player2InfluenceBar.value > 100){ Debug.Log("over 100");}
        if(player1InfluenceBar.value + player2InfluenceBar.value + 2*changeAmount <= 100) {
            player1InfluenceBar.value += 2*changeAmount;
        }
        else if(player1InfluenceBar.value + player2InfluenceBar.value == 100) {
            StealPoints(true, changeAmount);
        }
        else {
            float neutralLeft = 100 - player2InfluenceBar.value - player1InfluenceBar.value;
            float pointsToBeStealed = (changeAmount*2 - neutralLeft)/2;

            player1InfluenceBar.value += neutralLeft;
            StealPoints(true, pointsToBeStealed);
        }
        updateValuesOnOpponent();
    }

    public void DecreasePlayer1Influence() {
        player1InfluenceBar.value -= changeAmount;
        updateValuesOnOpponent();
    }

    public void IncreasePlayer2Influence() {
        if(player1InfluenceBar.value + player2InfluenceBar.value > 100){ Debug.Log("over 100");}
        if(player2InfluenceBar.value + player1InfluenceBar.value + changeAmount*2 <= 100) {
            player2InfluenceBar.value += changeAmount*2;
        }
        else if(player1InfluenceBar.value + player2InfluenceBar.value == 100) {
            StealPoints(false, changeAmount);
        }
        else {
            float neutralLeft = 100 - player2InfluenceBar.value - player1InfluenceBar.value;
            float pointsToBeStealed = (changeAmount*2 - neutralLeft)/2;

            player2InfluenceBar.value += neutralLeft;
            StealPoints(false, pointsToBeStealed);
        }
        updateValuesOnOpponent();
    }

    public void DecreasePlayer2Influence() {
        player2InfluenceBar.value -= changeAmount;
        updateValuesOnOpponent();
    }
}
