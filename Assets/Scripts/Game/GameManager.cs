using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] ManagerReferences managerReferences;

	CharacterClasses playerCharacter;
    CharacterClasses opponentCharacter;

	public PhotonView photonView;

	#region Setting Up

	private void Start() {
		//update character selections for both players
		playerCharacter = (CharacterClasses)System.Enum.Parse(typeof(CharacterClasses), PlayerPrefs.GetString("SelectedClass"));
        managerReferences.GetUIManager().UpdatePlayerCharacterPortrait(playerCharacter);
        managerReferences.GetInfluenceBarManager().UpdatePlayerInfluenceBarColor(playerCharacter);
        managerReferences.GetBoardManager().CreateStartingLocationPlayer(playerCharacter);
        managerReferences.GetStoreManager().SetPlayerCharacter(playerCharacter);
		managerReferences.GetDeckManager().SetPlayerCharacter(playerCharacter);
		photonView.RPC("UpdatePlayerClassForOpponent", RpcTarget.OthersBuffered, playerCharacter);

		moneyPlayer = startingMoney;
		moneyOpponent = startingMoney;
		UpdateMoneyText();
		notorietyPlayer = 0;
		notorietyOpponent = 0;
		UpdateNotorietyText();
        managerReferences.GetTurnStateController().StartTheGame();
	}

	[PunRPC]
	private void UpdatePlayerClassForOpponent(CharacterClasses opponentChar) {
        opponentCharacter = opponentChar;
        managerReferences.GetUIManager().UpdateOpponentCharacterPortrait(opponentCharacter);
        managerReferences.GetInfluenceBarManager().UpdateOpponentInfluenceBarColor(opponentCharacter);
        managerReferences.GetBoardManager().CreateStartingLocationOpponent(opponentCharacter);
    }

	#endregion Setting Up

	#region Money management

	[SerializeField] TMP_Text moneyPlayerTxt;
    [SerializeField] TMP_Text moneyOpponentTxt;
    [SerializeField] int startingMoney;
    int moneyPlayer;
    int moneyOpponent;

    public int getMoneyPlayer() {
        return moneyPlayer;
    }

    void UpdateValuesOnOpponent(){
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
        UpdateValuesOnOpponent();
    }

    public void DecreasePlayerMoney(int amount) {
        moneyPlayer -= amount;
        UpdateMoneyText();
        UpdateValuesOnOpponent();
    }

    public void IncreaseOpponentMoney(int amount) {
        moneyOpponent += amount;
        UpdateMoneyText();
        UpdateValuesOnOpponent();
    }

    public void DecreaseOpponentMoney(int amount) {
        moneyOpponent -= amount;
        UpdateMoneyText();
        UpdateValuesOnOpponent();
    }

	#endregion money management

	#region Notoriety management

	[SerializeField] TMP_Text notorietyPlayerTxt;
	[SerializeField] TMP_Text notorietyOpponentTxt;
	int notorietyPlayer;
	int notorietyOpponent;

	public int GetPlayerNotoriety() {
		return notorietyPlayer;
	}

	public int GetOpponentNotoriety() {
		return notorietyOpponent;
	}

	void UpdateNotorietyValuesOnOpponent() {
		photonView.RPC("SyncNotorietyValues", RpcTarget.OthersBuffered, notorietyPlayer, notorietyOpponent);
	}

	[PunRPC]
	private void SyncNotorietyValues(int amountPlayer, int amountOpponent) {
		notorietyPlayer = amountOpponent;
		notorietyOpponent = amountPlayer;
		UpdateNotorietyText();
	}

	private void UpdateNotorietyText() {
		notorietyPlayerTxt.text = "" + notorietyPlayer;
		notorietyOpponentTxt.text = "" + notorietyOpponent;
	}

	public void IncreasePlayerNotoriety(int amount) {
		notorietyPlayer += amount;
		UpdateNotorietyText();
		UpdateNotorietyValuesOnOpponent();
		if(managerReferences.GetTurnStateController().CheckIfItIsPlayersTurn()) {
			managerReferences.GetStoreManager().UpdateCardPrizesInStore();
			managerReferences.GetStoreManager().UpdateCardPrizesInStoreForOpponent();
		}
	}

	public void DecreasePlayerNotoriety(int amount) {
		notorietyPlayer -= amount;
		UpdateNotorietyText();
		UpdateNotorietyValuesOnOpponent();
	}

	public void IncreaseOpponentNotoriety(int amount) {
		notorietyOpponent += amount;
		UpdateNotorietyText();
		UpdateNotorietyValuesOnOpponent();
	}

	#endregion notoriety management

	#region Card management

	public bool isOnTopOfPlayArea(Transform cardPos) {
        return true;
    }

	#endregion Card management
}
