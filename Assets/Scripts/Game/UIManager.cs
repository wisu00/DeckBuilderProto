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
	[SerializeField] Sprite[] characterSprites;
    [SerializeField] Image playerCharacterPortrait;
	[SerializeField] Image opponentCharacterPortrait;
	[SerializeField] GameObject cardBuyArea;
	[SerializeField] GameObject cardPlayArea;
	[SerializeField] GameObject[] toolPlayAreas;
	[SerializeField] GameObject locationPlayArea;

	public PhotonView photonView;
    [SerializeField] private Button endTheTurnButton;

    bool messageIsActive = false;
    [SerializeField] private Image popUpMessageBox;
    [SerializeField] private GameObject notYourTurnMessage;
	[SerializeField] private GameObject toolSlotTakenMessage;
	[SerializeField] private GameObject victoryMessage;
	[SerializeField] private GameObject defeatMessage;

	private void Awake() {
        playersName.text = PhotonNetwork.LocalPlayer.NickName;
    }

    void Start() {
        updateOpponentsNickName();
    }

    public void UpdatePlayerCharacterPortrait(CharacterClasses selectedClass) {
        playerCharacterPortrait.sprite = characterSprites[(int)selectedClass];
	}

	public void UpdateOpponentCharacterPortrait(CharacterClasses selectedClass) {
		opponentCharacterPortrait.sprite = characterSprites[(int)selectedClass];
	}

	public void ShowNotYourTurnMessage() {
        if(!messageIsActive) {
			messageIsActive = true;
			popUpMessageBox.gameObject.SetActive(true);
			notYourTurnMessage.SetActive(true);
			Invoke("DisableMessage", 1.5f);
		}
	}

	public void ShowToolSlotTakenMessage() {
		if(!messageIsActive) {
			messageIsActive = true;
			popUpMessageBox.gameObject.SetActive(true);
			toolSlotTakenMessage.SetActive(true);
			Invoke("DisableMessage", 1.5f);
		}
	}

	public void ShowVictoryMessage() {
		DisableMessage();
		messageIsActive = true;
		popUpMessageBox.gameObject.SetActive(true);
		victoryMessage.SetActive(true);
	}

	public void ShowDefeatMessage() {
		DisableMessage();
		messageIsActive = true;
		popUpMessageBox.gameObject.SetActive(true);
		defeatMessage.SetActive(true);
	}

	private void DisableMessage() {
		messageIsActive = false;
		popUpMessageBox.gameObject.SetActive(false);
		notYourTurnMessage.SetActive(false);
		toolSlotTakenMessage.SetActive(false);
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

	public void SetBuyAreaActiveStatus(bool active) {
		cardBuyArea.SetActive(active);
	}

    public void SetPlayAreaActiveStatus(bool activeStatus, CardType cardType) {
		if(cardType == CardType.Tool) {
			foreach(GameObject area in toolPlayAreas) area.SetActive(activeStatus);
		}
		else if(cardType == CardType.Location) locationPlayArea.SetActive(activeStatus);
		else cardPlayArea.SetActive(activeStatus);

	}
}
