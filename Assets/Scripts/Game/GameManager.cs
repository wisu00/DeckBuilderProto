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
		photonView.RPC("UpdatePlayerClassForOpponent", RpcTarget.OthersBuffered, playerCharacter);

		moneyPlayer = startingMoney;
		moneyOpponent = startingMoney;
		UpdateMoneyText();
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

    #region Card management

    public bool isOnTopOfPlayArea(Transform cardPos) {
        return true;
    }

	#endregion Card management

	

	
}
