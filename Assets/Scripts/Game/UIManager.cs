using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text playersName;
    public TMP_Text opponentsName;
 //   public PhotonView photonView;
    [SerializeField] private Button endTheTurnButton;

    void Start() {
        playersName.text = EditPlayerName.Instance.GetPlayerName();
        updateOpponentsNickName();
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
