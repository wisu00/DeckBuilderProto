using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {
	Event, Tool, Location
}

public abstract class Card : ScriptableObject {
    [HideInInspector]public int cardIndex;//identifier that is assigned during run time
    public string cardName;
    public Sprite cardArt;
    [TextArea(2,10)] public string description;
    public int buyCost;
    public int playCost;
    public int discardValue;
    public int tier;
	public CardType cardType;

    [HideInInspector] public ManagerReferences managerReferences;
	[HideInInspector] public GameManager gameManager;
	[HideInInspector] public HandManager handManager;

	[SerializeField] ConditionalEffect[] onBuyEffects;
	[SerializeField] ConditionalEffect[] onPlayEffects;
    [SerializeField] ConditionalEffect[] onDiscardEffects;

	[System.Serializable]
	public class ConditionalEffect {
		[SerializeField]
		public EffectCondition condition;

		[SerializeField]
		public CardEffect cardEffect;

		public void DoEffectConditionally(ManagerReferences managerReferences, Card card) {
            if(condition == null) {
				cardEffect.DoEffect(managerReferences, card);
			} 
            else if (condition.CheckCondition(managerReferences, card)) {
				cardEffect.DoEffect(managerReferences, card);
			}
		}
	}

	public void AssignGameManager(ManagerReferences managerReferences) {
        this.managerReferences = managerReferences;
        gameManager = managerReferences.GetGameManager();
        handManager = managerReferences.GetHandManager();
    }

	public void OnBuy() { 
        foreach(ConditionalEffect effect in onBuyEffects) {
		    effect.DoEffectConditionally(managerReferences, this);
	    }
    }

	public void OnPlay() {
		foreach(ConditionalEffect effect in onPlayEffects) {
			effect.DoEffectConditionally(managerReferences, this);
		}
	}

	public void OnDiscard() {
		foreach(ConditionalEffect effect in onDiscardEffects) {
			effect.DoEffectConditionally(managerReferences, this);
		}
	}

    //gets override in cardBack
    public virtual bool isCardBack() {
        return false;
    }
    /*

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
    */
}
