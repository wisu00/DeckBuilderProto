using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerReferences : MonoBehaviour {
    [SerializeField] UIManager uIManager;
	[SerializeField] GameManager gameManager;
	[SerializeField] MenuManager menuManager;
    [SerializeField] TurnStateController turnStateController;
	[SerializeField] StoreManager storeManager;
    [SerializeField] HandManager handManager;
    [SerializeField] HandManager handManagerOpponent;
	[SerializeField] DiscardPileManager discardPileManager;
	[SerializeField] DiscardPileManager discardPileManagerOpponent;
	[SerializeField] DeckManager deckManager;
    [SerializeField] BoardManager boardManager;
    [SerializeField] BoardManager boardManagerOpponent;

    public UIManager GetUIManager() { 
		return uIManager;
	}
	public GameManager GetGameManager() {
		return gameManager;
	}
	public MenuManager GetMenuManager() {
		return menuManager;
	}
	public TurnStateController GetTurnStateController() {
		return turnStateController;
	}
	public StoreManager GetStoreManager() {
		return storeManager;
	}
	public HandManager GetHandManager() {
		return handManager;
	}
	public HandManager GetHandManagerOpponent() {
		return handManagerOpponent;
	}
	public DiscardPileManager GetDiscardPileManager() {
		return discardPileManager;
	}
	public DiscardPileManager GetDiscardPileManagerOpponent() {
		return discardPileManagerOpponent;
	}
	public DeckManager GetDeckManager() {
		return deckManager;
	}
	public BoardManager GetBoardManager() {
		return boardManager;
	}
	public BoardManager GetBoardManagerOpponent() {
		return boardManagerOpponent;
	}
}
