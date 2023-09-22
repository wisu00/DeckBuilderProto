using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBaseFunctionality : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler {
	[HideInInspector] public Card card;
	private HandManager handManager;
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
	[SerializeField] Image darkTint;

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
    public void UpdateValuesInStore(StoreManager storeManager, GameManager gameManager, DiscardPileManager discardPileManager, UIManager uIManager, TurnStateController turnStateController, bool ownedByPlayer, int cardPosInStore) {
		this.storeManager = storeManager;
		this.gameManager = gameManager;
		this.discardPileManager = discardPileManager;
		this.ownedByPlayer = ownedByPlayer;
		this.turnStateController = turnStateController;
		this.uIManager = uIManager;
		this.cardPosInStore = cardPosInStore;
		cardIsInStore = true;
		canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
		cardArt.sprite = card.cardArt;
		cardName.text = card.cardName;
		description.text = card.description;
		buyCost.text = card.buyCost.ToString();
		playCost.text = card.playCost.ToString();
		discardValueText.text = card.discardValue.ToString();
	}

    public void UpdateValues(HandManager handManager, GameManager gameManager, TurnStateController turnStateController, UIManager uIManager) {
		this.handManager = handManager;
		this.gameManager = gameManager;
		this.turnStateController = turnStateController;
		this.uIManager = uIManager;
		canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        card.AssignGameManager(gameManager, handManager);
        cardArt.sprite = card.cardArt;
        cardName.text = card.cardName;
        description.text = card.description;
		playCost.text = card.playCost.ToString();
        discardValueText.text = card.discardValue.ToString();

		if(card.isCardBack()) {
            foreach (Transform child in cardArt.gameObject.transform) {
                child.gameObject.SetActive(false);
            }
        } 
    }

	public void UpdateValueOnBoard(HandManager handManager, GameManager gameManager, TurnStateController turnStateController, bool ownedByPlayer, UIManager uIManager) {
		this.handManager = handManager;
		this.gameManager = gameManager;
		this.turnStateController = turnStateController;
		this.ownedByPlayer = ownedByPlayer;
		card.AssignGameManager(gameManager, handManager);
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
				gameManager.DecreasePlayerMoney(card.buyCost);
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
				handManager.CardWasPlayedOnBoard(card, gameObject, toolSlotNumber);
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
				cardThatWasPlayed.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(handManager, gameManager, turnStateController, true, uIManager);
				handManager.CardWasPlayedOnBoard(card, gameObject, 0);
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

	public void OnPointerEnter(PointerEventData eventData) {
		//activates the dark tint only when tool is on board and getting hovered when holding another tool card
		if(cardIsOnBoard && card.cardType ==CardType.Tool && eventData.pointerDrag != null && ownedByPlayer) {
			if(eventData.pointerDrag.GetComponent<CardBaseFunctionality>().card.cardType == CardType.Tool) setDarkTintActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if(cardIsOnBoard && card.cardType == CardType.Tool) {
			setDarkTintActive(false);
		}
	}

	public void setDarkTintActive(bool state) {
		darkTint.gameObject.SetActive(state);
	}
}
