using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] DeckManager deck;
    [SerializeField] HandManager hand;
    [SerializeField] DiscardPileManager discardPile;
    [SerializeField] UIManager uiManager;

    CharacterClasses playerCharacter;
    CharacterClasses opponentCharacter;

	public PhotonView photonView;

	#region Setting Up

	private void Start() {
		//update character selections for both players
		playerCharacter = (CharacterClasses)System.Enum.Parse(typeof(CharacterClasses), PlayerPrefs.GetString("SelectedClass"));
		uiManager.UpdatePlayerCharacterPortrait(playerCharacter);
		photonView.RPC("UpdatePlayerClassForOpponent", RpcTarget.OthersBuffered, playerCharacter);

		moneyPlayer = startingMoney;
		moneyOpponent = startingMoney;
		UpdateMoneyText();
	}

	[PunRPC]
	private void UpdatePlayerClassForOpponent(CharacterClasses opponentChar) {
        opponentCharacter = opponentChar;
		uiManager.UpdateOpponentCharacterPortrait(opponentCharacter);
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

    #region Card management

    public bool isOnTopOfPlayArea(Transform cardPos) {
        return true;
    }

	#endregion Card management

	

	
}
