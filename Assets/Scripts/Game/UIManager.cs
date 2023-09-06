using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public class UIManager : NetworkBehaviour {
    private string playersName;
    public TMP_Text playersNameTxt;
    public TMP_Text opponentsNameTxt;
 //   public PhotonView photonView;
    [SerializeField] private Button endTheTurnButton;

    private NetworkVariable<FixedString64Bytes> player1Name = new NetworkVariable<FixedString64Bytes>();
    private NetworkVariable<FixedString64Bytes> player2Name = new NetworkVariable<FixedString64Bytes>();

    public override void OnNetworkSpawn() {
        playersName = EditPlayerName.Instance.GetPlayerName();
        playersNameTxt.text = playersName;

        ChangePlayerNameServerRpc(playersName);
        UpdateNameText();

        player1Name.OnValueChanged += (FixedString64Bytes previousValue, FixedString64Bytes newValue) => {
            UpdateNameText();
        };
        player2Name.OnValueChanged += (FixedString64Bytes previousValue, FixedString64Bytes newValue) => {
            UpdateNameText();
        };
    }

    private void UpdateNameText() {
        if (IsOwner) {
            opponentsNameTxt.text = player2Name.Value.ToString();
        }
        else {
            opponentsNameTxt.text = player1Name.Value.ToString();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePlayerNameServerRpc(FixedString64Bytes amount, ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (clientId == 0) {
            player1Name.Value = amount;
        }
        else {
            player2Name.Value = amount;
        }
    }

    void updateOpponentsNickName(){
  //      photonView.RPC("SyncValues", RpcTarget.OthersBuffered, playersName.text);
    }

 //   [PunRPC]
 //   void SyncValues (string name) {
 //       opponentsName.text = name;
 //   }

    public void InPlayTurnState(bool b) {
        endTheTurnButton.interactable = b;
    }
}
