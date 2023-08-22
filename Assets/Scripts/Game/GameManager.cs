using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using TMPro;
using Unity.Netcode;

public class GameManager : NetworkBehaviour {
    [SerializeField] DeckManager deck;
    [SerializeField] HandManager hand;
    [SerializeField] DiscardPileManager discardPile;
    
 //   public PhotonView photonView;
    
    #region Money management

    [SerializeField] TMP_Text moneyPlayerTxt;
    [SerializeField] TMP_Text moneyOpponentTxt;
    [SerializeField] int startingMoney;
    private NetworkVariable<int> player1Money = new NetworkVariable<int>();
    private NetworkVariable<int> player2Money = new NetworkVariable<int>();
    int moneyPlayer;
    int moneyOpponent;




    //updates values on server
    [ServerRpc(RequireOwnership = false)]
    public void UpdateMoneyForPlayerServerRpc(ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId)) {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
        }
    }

    //server updates values on clients
    [ClientRpc]
    void UpdateValuesClientRpc(int moneyAmount, string sometext) { 
    
    }

    public int getMoneyPlayer() {
        return moneyPlayer;
    }

    void updateValuesOnOpponent(){
 //       photonView.RPC("SyncValues", RpcTarget.OthersBuffered, moneyPlayer, moneyOpponent);
    }

 //   [PunRPC]
 //   private void SyncValues (int amountPlayer, int amountOpponent) {
 //       moneyPlayer = amountOpponent;
 //       moneyOpponent = amountPlayer;
 //       UpdateMoneyText();
 //   }

    private void UpdateMoneyText() {

        moneyPlayerTxt.text = "" + playerMoney.Value;
        moneyOpponentTxt.text = "" + moneyOpponent;
    }

    private void UpdateMoneyText(bool isOwner) {
        if (isOwner) {
            moneyPlayerTxt.text = "" + playerMoney.Value;
        }
        else {
            moneyOpponentTxt.text = "" + playerMoney.Value;
        }
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

    #region Card management

    public bool isOnTopOfPlayArea(Transform cardPos) {
        return true;
    }

    #endregion Card management

    public override void OnNetworkSpawn() {
        if() {

        }
        player1Money.OnValueChanged += (int previousValue, int newValue) => {
            UpdateMoneyText(IsOwner);
        };
        player2Money.OnValueChanged += (int previousValue, int newValue) => {
            UpdateMoneyText(IsOwner);
        };
    }

    private void Start() {
        moneyPlayer = startingMoney;
        playerMoney.Value = startingMoney;
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
