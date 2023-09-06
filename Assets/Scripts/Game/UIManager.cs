using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text playersName;
    public TMP_Text opponentsName;
    public PhotonView photonView;
    [SerializeField] private Button endTheTurnButton;

    bool messageIsActive = false;
    [SerializeField] private Image popUpMessageBox;
    [SerializeField] private GameObject notYourTurnMessage;
    
    private void Awake() {
        playersName.text = PhotonNetwork.LocalPlayer.NickName;
    }

    void Start() {
        updateOpponentsNickName();
    }

    public void ShowNotYourTurnMessage() {
        if(!messageIsActive) {
			messageIsActive = true;
			popUpMessageBox.gameObject.SetActive(true);
			notYourTurnMessage.SetActive(true);
			Invoke("DisableMessage", 1.5f);
		}
	}

    private void DisableMessage() {
		messageIsActive = false;
		popUpMessageBox.gameObject.SetActive(false);
		notYourTurnMessage.SetActive(false);
	}

    void updateOpponentsNickName(){
        photonView.RPC("SyncValues", RpcTarget.OthersBuffered, playersName.text);
    }

    [PunRPC]
    void SyncValues (string name) {
        opponentsName.text = name;
    }

    public void InPlayTurnState(bool b) {
        endTheTurnButton.interactable = b;
    }
}
