using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CardType {Event, Tool, Location,Identity, Junk, CardBack}

public enum TargetTypes {OwnTool, OpponentTool, OwnLocation, OpponentLocation}

[CreateAssetMenu(fileName = "Card", menuName = "Card", order = 1)]
public class Card : ScriptableObject {
    public CardType cardType;
    public CharacterClasses characterClass;
    public string cardName;
    public Sprite cardArt;
    [TextArea(2,10)] public string description;
    public int buyCost;
    public int playCost;
    public int discardValue;
    [Range(1, 3)] public int tier;

    [HideInInspector] public int cardIndex;//identifier that is assigned during run time
    [HideInInspector] public ManagerReferences managerReferences;
	[HideInInspector] public GameManager gameManager;
	[HideInInspector] public HandManager handManager;

	[SerializeField] Condition[] additionalPlayConditions;

    //should be moved into event cards
	[HideInInspector] public bool cardIsTargeted;
	TargetTypes[] allowedTargets;

	[SerializeField] ConditionalEffect[] onBuyEffects;
	[SerializeField] ConditionalEffect[] onPlayEffects;
    [SerializeField] ConditionalEffect[] onDiscardEffects;
    

	[System.Serializable]
	public class ConditionalEffect {
		[SerializeField]
		public Condition condition;

		[SerializeField]
		public CardEffect cardEffect;

		public void DoEffectConditionally(ManagerReferences managerReferences, Card card, CardBaseFunctionality baseFunctionality) {
            if(condition == null) {
				cardEffect.DoEffect(managerReferences, card, baseFunctionality);
			} 
            else if (condition.CheckCondition(managerReferences, card)) {
				cardEffect.DoEffect(managerReferences, card, baseFunctionality);
			}
		}
	}

	public void AssignGameManager(ManagerReferences managerReferences) {
        this.managerReferences = managerReferences;
        gameManager = managerReferences.GetGameManager();
        handManager = managerReferences.GetHandManager();
    }

	public void OnBuy(CardBaseFunctionality baseFunctionality) { 
        foreach(ConditionalEffect effect in onBuyEffects) {
		    effect.DoEffectConditionally(managerReferences, this, baseFunctionality);
	    }
    }

    public bool ClearsAdditionalPlayConditions() {
        if(additionalPlayConditions == null) return true;
		
        foreach(Condition playCondition in additionalPlayConditions) {
            if(playCondition == null) Debug.LogError("card has empty play condition");
            if(!playCondition) return false;
		}

		return true;
    }

	public void OnPlay(CardBaseFunctionality baseFunctionality) {
		foreach(ConditionalEffect effect in onPlayEffects) {
			effect.DoEffectConditionally(managerReferences, this, baseFunctionality);
		}
	}

	public void OnDiscard(CardBaseFunctionality baseFunctionality) {
		foreach(ConditionalEffect effect in onDiscardEffects) {
			effect.DoEffectConditionally(managerReferences, this, baseFunctionality);
		}
	}

    [SerializeField] ConditionalEffect[] turnStartEffects;
    public void TurnStartEffects(CardBaseFunctionality baseFunctionality)
    {
        foreach (ConditionalEffect effect in turnStartEffects)
        {
            effect.DoEffectConditionally(managerReferences, this, baseFunctionality);
        }
    }

	[SerializeField] ConditionalEffect[] onHandChangesEffects;
    public void HandChangesEffects(CardBaseFunctionality baseFunctionality) {
		foreach(ConditionalEffect effect in onHandChangesEffects) {
			effect.DoEffectConditionally(managerReferences, this, baseFunctionality);
		}
	}
    public bool CardHasHandChangeEffects() {
        return onHandChangesEffects != null;
    }

	[SerializeField] ConditionalEffect[] turnEndEffects;
    public void TurnEndEffects(CardBaseFunctionality baseFunctionality) {
        foreach (ConditionalEffect effect in turnEndEffects) {
            effect.DoEffectConditionally(managerReferences, this, baseFunctionality);
        }
    }

    //gets override in cardBack
    public virtual bool isCardBack() {
        return false;
    }
}
