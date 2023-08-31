using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using TMPro;
using Unity.Netcode;
using Newtonsoft.Json.Linq;

public class GameManager : NetworkBehaviour {
    private uint playerID;
    private uint opponentID;

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

    //updates values on server
    [ServerRpc(RequireOwnership = false)]
    public void UpdateMoneyForPlayerServerRpc(ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId)) {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
        }
    }

    public int getMoneyPlayer() {
        if(playerID == 0) return player1Money.Value;
        else return player2Money.Value;
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
        if(IsOwner) {
            moneyPlayerTxt.text = "" + player1Money.Value;
            moneyOpponentTxt.text = "" + player2Money.Value;
        }
        else {
            moneyPlayerTxt.text = "" + player2Money.Value;
            moneyOpponentTxt.text = "" + player1Money.Value;
        }
    }

    public void IncreasePlayerMoney(int amount) {
        IncreasePlayerMoneyServerRpc(amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreasePlayerMoneyServerRpc(int amount, ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (clientId == 0) {
            player1Money.Value += amount;
        }
        else {
            player2Money.Value += amount;
        }
    }

    public void DecreasePlayerMoney(int amount) {
        DecreasePlayerMoneyServerRpc(amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DecreasePlayerMoneyServerRpc(int amount, ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (clientId == 0) {
            player1Money.Value -= amount;
        }
        else {
            player2Money.Value -= amount;
        }
    }

    public void IncreaseOpponentMoney(int amount) {
        IncreaseOpponentMoneyServerRpc(amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreaseOpponentMoneyServerRpc(int amount, ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (clientId == 0) {
            player2Money.Value += amount;
        }
        else {
            player1Money.Value += amount;
        }
    }

    public void DecreaseOpponentMoney(int amount) {
        DecreaseOpponentMoneyServerRpc(amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DecreaseOpponentMoneyServerRpc(int amount, ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (clientId == 0) {
            player2Money.Value -= amount;
        }
        else {
            player1Money.Value -= amount;
        }
    }
    
    #endregion money management

    #region Card management

    public bool isOnTopOfPlayArea(Transform cardPos) {
        return true;
    }

    #endregion Card management



    public override void OnNetworkSpawn() {
        IncreasePlayerMoneyServerRpc(startingMoney);
        UpdateMoneyText();

        player1Money.OnValueChanged += (int previousValue, int newValue) => {
            UpdateMoneyText();
        };
        player2Money.OnValueChanged += (int previousValue, int newValue) => {
            UpdateMoneyText();
        };
    }
    void Update() {
        if(Input.GetKeyDown("space")) {
            deck.DrawCard();
        }
    }
}
