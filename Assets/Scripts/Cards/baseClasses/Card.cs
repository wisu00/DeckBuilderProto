using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject {
    public int cardIndex;//identifier
    public string cardName;
    public Sprite cardArt;
    [TextArea(2,10)] public string description;
    public int buyCost;
    public int playCost;
    public int discardValue;
    public int tier;

    private GameManager gameManager;
    private HandManager handManager;

    public void AssignGameManager(GameManager manager, HandManager hand) {
        gameManager = manager;
        handManager = hand;
    }

	public abstract void OnBuy();

	public abstract void OnPlay();

    public abstract void OnDiscard();

    //gets override in cardBack
    public virtual bool isCardBack() {
        return false;
    }

    //gets override in buildingCards
    public virtual bool goesOnBoard() {
        return false;
    }

    #region card effects that can be used from other methods (like OnPlay()). 

    protected virtual void PrintName() {
        Debug.Log("derived name: " + cardName);
    }

    protected virtual void increasePlayerMoney(int amount) {
        gameManager.IncreasePlayerMoney(amount);
    }

    protected virtual void decreasePlayerMoney(int amount) {
        gameManager.DecreasePlayerMoney(amount);
    }

    protected virtual void increaseOpponentMoney(int amount) {
        gameManager.IncreaseOpponentMoney(amount);
    }

    protected virtual void decreaseOpponentMoney(int amount) {
        gameManager.DecreaseOpponentMoney(amount);
    }

    #endregion
}
