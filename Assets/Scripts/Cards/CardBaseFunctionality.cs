using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBaseFunctionality : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
	[HideInInspector] public Card card;
	private HandManager handManager;
	private BoardManager boardManager;
	private DiscardPileManager discardPileManager;
	private GameManager gameManager;
	private TurnStateController turnStateController;
	private StoreManager storeManager;
	private Canvas canvas;
	public TMP_Text cardName;
	public Image cardArt;
	public TMP_Text description;
	public TMP_Text buyCost;
	public TMP_Text playCost;
	public TMP_Text discardValueText;

	[SerializeField] UIManager uIManager;
	[SerializeField] GameObject cardPrefab;

	private ManagerReferences managerReferences;

	private CanvasGroup canvasGroup;
	private bool cardIsInStore = false;
	private bool cardIsOnBoard = false;
	private bool ownedByPlayer = true;

    Vector3 startPosition;

    private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(card.isCardBack() || cardIsOnBoard || !ownedByPlayer) {
			eventData.pointerDrag = null;
        }
        else {
			canvasGroup.alpha = 0.6f;
			canvasGroup.blocksRaycasts = false;
            PickCardUp();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData) {
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
        transform.position = startPosition;
		if(cardIsInStore) {
			uIManager.SetBuyAreaActiveStatus(false);
		}
        else {
			uIManager.SetPlayAreaActiveStatus(false, card.cardType);
		}
	}

    public void PickCardUp() {
		startPosition = transform.position;
        if(cardIsInStore) {
			uIManager.SetBuyAreaActiveStatus(true);
		}
        else {
			uIManager.SetPlayAreaActiveStatus(true, card.cardType);
		}
    }

	int cardPosInStore = 0;
    public void UpdateValuesInStore(ManagerReferences managerReferences, bool ownedByPlayer, int cardPosInStore) {
		this.managerReferences = managerReferences;
		storeManager = managerReferences.GetStoreManager();
		gameManager = managerReferences.GetGameManager();
		discardPileManager = managerReferences.GetDiscardPileManager();
		this.ownedByPlayer = ownedByPlayer;
		turnStateController = managerReferences.GetTurnStateController();
		uIManager = managerReferences.GetUIManager();
		this.cardPosInStore = cardPosInStore;
		cardIsInStore = true;
		canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
		card.AssignGameManager(managerReferences);
		cardArt.sprite = card.cardArt;
		cardName.text = card.cardName;
		description.text = card.description;
		buyCost.text = card.buyCost.ToString();
		playCost.text = card.playCost.ToString();
		discardValueText.text = card.discardValue.ToString();
	}

    public void UpdateValues(ManagerReferences managerReferences) {
		this.managerReferences = managerReferences;
		handManager = managerReferences.GetHandManager();
		boardManager = managerReferences.GetBoardManager();
		gameManager = managerReferences.GetGameManager();
		turnStateController = managerReferences.GetTurnStateController();
		uIManager = managerReferences.GetUIManager();
		canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        card.AssignGameManager(managerReferences);
        cardArt.sprite = card.cardArt;
        cardName.text = card.cardName;
        description.text = card.description;
		playCost.text = card.playCost.ToString();
        discardValueText.text = card.discardValue.ToString();
    }

	public void UpdateValueOnBoard(ManagerReferences managerReferences, bool ownedByPlayer) {
		this.managerReferences = managerReferences;
		handManager = managerReferences.GetHandManager();
		gameManager = managerReferences.GetGameManager();
		turnStateController = managerReferences.GetTurnStateController();
		this.ownedByPlayer = ownedByPlayer;
		card.AssignGameManager(managerReferences);
		cardArt.sprite = card.cardArt;
		cardName.text = card.cardName;
		description.text = card.description;
		playCost.text = card.playCost.ToString();
		discardValueText.text = card.discardValue.ToString();
		cardIsOnBoard = true;
	}

    public void BuyTheCard() {
		if(turnStateController.CheckIfItIsPlayersTurn() && cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.buyCost) {
				//gameManager.DecreasePlayerMoney(card.buyCost);
				card.OnBuy();
				uIManager.SetBuyAreaActiveStatus(false);
				discardPileManager.AddCardToDiscardPile(card);
                storeManager.CardIsBought(card, gameObject, cardPosInStore);
			}
		}
	}

	public void PlayToolCard(int toolSlotNumber) {
		if(turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.playCost) {
				card.OnPlay();
				handManager.RemoveCardFromHand(card, gameObject);
				boardManager.CardWasPlayedOnBoard(card, toolSlotNumber);
				uIManager.SetPlayAreaActiveStatus(false, card.cardType);
			}
		}
	}

	public void PlayLocationCard(Transform cardPlaceOnBoard) {
		if(turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.playCost) {
				card.OnPlay();
				GameObject cardThatWasPlayed;
				cardThatWasPlayed = Instantiate(cardPrefab, cardPlaceOnBoard);
				cardThatWasPlayed.GetComponent<CardBaseFunctionality>().card = card;
				cardThatWasPlayed.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(managerReferences, true);
				handManager.RemoveCardFromHand(card, gameObject);
				boardManager.CardWasPlayedOnBoard(card, 0);
				uIManager.SetPlayAreaActiveStatus(false, card.cardType);
			}
		}
	}

	public void PlayEventCard() {
        if (turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.playCost) {
				card.OnPlay();
				if(card.cardType==CardType.Event) {
					handManager.MoveCardToDiscardPile(card, gameObject);
					uIManager.SetPlayAreaActiveStatus(false, card.cardType);
				}
			}
		}
    }

    public void DiscardTheCard() {
        if(turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
            card.OnDiscard();
			handManager.MoveCardToDiscardPile(card, gameObject);
			uIManager.SetPlayAreaActiveStatus(false, card.cardType);
		}
    }
}
