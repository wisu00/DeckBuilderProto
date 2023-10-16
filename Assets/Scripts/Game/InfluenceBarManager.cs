using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InfluenceBarManager : MonoBehaviour {
    [SerializeField] UIManager uiManager;
    [SerializeField] private Slider playerInfluenceBar;
    [SerializeField] private Slider opponentInfluenceBar;

    [SerializeField] Color influenceBarColorBankerPlayer;
	[SerializeField] Color influenceBarColorScrapperPlayer;
	[SerializeField] Color influenceBarColorBankerOpponent;
	[SerializeField] Color influenceBarColorScrapperOpponent;

	int influenceBarMaxValue;

    public PhotonView photonView;

	private void Start() {
        influenceBarMaxValue = (int)playerInfluenceBar.GetComponent<Slider>().maxValue;
	}

	public void UpdatePlayerInfluenceBarColor(CharacterClasses selectedClass) {
		switch(selectedClass) {
			case CharacterClasses.Banker:
				playerInfluenceBar.GetComponentInChildren<Image>().color = influenceBarColorBankerPlayer;
				break;
			case CharacterClasses.Scrapper:
				playerInfluenceBar.GetComponentInChildren<Image>().color = influenceBarColorScrapperPlayer;
				break;
			default:
				break;
		}
	}

	public void UpdateOpponentInfluenceBarColor(CharacterClasses selectedClass) {
		switch(selectedClass) {
			case CharacterClasses.Banker:
				opponentInfluenceBar.GetComponentInChildren<Image>().color = influenceBarColorBankerOpponent;
				break;
			case CharacterClasses.Scrapper:
				opponentInfluenceBar.GetComponentInChildren<Image>().color = influenceBarColorScrapperOpponent;
				break;
			default:
				break;
		}
	}

	void UpdateValuesOnOpponent() {
		checkForWinner();
		photonView.RPC("SyncValues", RpcTarget.OthersBuffered, playerInfluenceBar.value, opponentInfluenceBar.value);
    }

    [PunRPC]
    void SyncValues (float Opponent, float player) {
        playerInfluenceBar.value = player;
        opponentInfluenceBar.value = Opponent;
        checkForWinner();
	}

    private void StealPoints(bool player ,float amount) {
        if(player) {
            playerInfluenceBar.value += amount;
            opponentInfluenceBar.value -= amount;
        }
        else {
            playerInfluenceBar.value -= amount;
            opponentInfluenceBar.value += amount;
        }
    }

    public void IncreasePlayerInfluence(int amount) {
		IncreasePlayerInfluence(true, amount);
	}

    public void DecreasePlayerInfluence(int amount) {
        playerInfluenceBar.value -= amount;
        UpdateValuesOnOpponent();
    }

    public void IncreaseOpponentInfluence(int amount) {
        IncreasePlayerInfluence(false, amount);
	}

	public void DecreaseOpponentInfluence(int amount) {
		opponentInfluenceBar.value -= amount;
		UpdateValuesOnOpponent();
	}

	private void IncreasePlayerInfluence(bool increasingPlayersPoints, int amount) {
		if(playerInfluenceBar.value + opponentInfluenceBar.value > influenceBarMaxValue) { Debug.LogError("influence is over 100"); }
		//there is enough empty space in bar
		if(playerInfluenceBar.value + opponentInfluenceBar.value + amount <= influenceBarMaxValue) {
            if(increasingPlayersPoints) playerInfluenceBar.value += amount;
            else opponentInfluenceBar.value += amount;
		}
		//there is no empty space between players
		else if(playerInfluenceBar.value + opponentInfluenceBar.value == influenceBarMaxValue) {
			StealPoints(increasingPlayersPoints, amount);
		}
		//there is some space but not enough -> fill empty and steal rest
		else {
			float neutralLeft = influenceBarMaxValue - opponentInfluenceBar.value - playerInfluenceBar.value;
			float pointsToBeStealed = amount - neutralLeft;

			if(increasingPlayersPoints) playerInfluenceBar.value += neutralLeft;
			else opponentInfluenceBar.value += neutralLeft;

			StealPoints(increasingPlayersPoints, pointsToBeStealed);
		}
		UpdateValuesOnOpponent();
	}

    private void checkForWinner() {
        if(playerInfluenceBar.value == influenceBarMaxValue) {
            uiManager.ShowVictoryMessage();
		}
        else if(opponentInfluenceBar.value == influenceBarMaxValue) {
			uiManager.ShowDefeatMessage();
		}
    }

}
