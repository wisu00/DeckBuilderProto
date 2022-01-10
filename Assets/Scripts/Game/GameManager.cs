using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] DeckManager deck;
    [SerializeField] HandManager hand;
    [SerializeField] DiscardPileManager discardPile;
    [SerializeField] TMP_Text moneyPlayerTxt;
    [SerializeField] TMP_Text moneyOpponentTxt;

    [SerializeField] int startingMoney;
    public PhotonView photonView;
    int moneyPlayer;
    int moneyOpponent;

    #region Money management
    public int getMoneyPlayer() {
        return moneyPlayer;
    }

    void updateValuesOnOpponent(){
        photonView.RPC("SyncValues", RpcTarget.OthersBuffered, moneyPlayer, moneyOpponent);
    }

    [PunRPC]
    private void SyncValues (int amountPlayer, int amountOpponent) {
        moneyPlayer = amountOpponent;
        moneyOpponent = amountPlayer;
        UpdateMoneyText();
    }

    private void UpdateMoneyText() {
        moneyPlayerTxt.text = "" + moneyPlayer;
        moneyOpponentTxt.text = "" + moneyOpponent;
    }

    public void IncreasePlayerMoney(int amount) {
        moneyPlayer += amount;
        UpdateMoneyText();
        updateValuesOnOpponent();
    }

    public void DecreasePlayerMoney(int amount) {
        moneyPlayer -= amount;
        UpdateMoneyText();
        updateValuesOnOpponent();
    }

    public void IncreaseOpponentMoney(int amount) {
        moneyOpponent += amount;
        UpdateMoneyText();
        updateValuesOnOpponent();
    }

    public void DecreaseOpponentMoney(int amount) {
        moneyOpponent -= amount;
        UpdateMoneyText();
        updateValuesOnOpponent();
    }
    #endregion money management

    private void Start() {
        moneyPlayer = startingMoney;
        moneyOpponent = startingMoney;
        UpdateMoneyText();
    }

    void Update()
    {
        if(Input.GetKeyDown("space")) {
            deck.DrawCard();
        }
    }
}
