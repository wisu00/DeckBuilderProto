using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBaseFunctionality : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
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
	public CardType cardType;

	[SerializeField] UIManager uIManager;
	[SerializeField] GameObject cardPrefab;

	private CanvasGroup canvasGroup;
	private bool cardIsInStore = false;
	private bool ownedByPlayer = true;

    Vector3 startPosition;

    private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(card.isCardBack() || !ownedByPlayer) {
			Debug.Log("cant drag");
			eventData.pointerDrag = null;
        }
        else {
			Debug.Log("begin drag");
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
			uIManager.SetPlayAreaActiveStatus(false, cardType);
		}
	}

    public void PickCardUp() {
		Debug.Log("picked card up");
		startPosition = transform.position;
        if(cardIsInStore) {
			uIManager.SetBuyAreaActiveStatus(true);
		}
        else {
			uIManager.SetPlayAreaActiveStatus(true, cardType);
		}
    }

    public void UpdateValuesInStore(StoreManager storeManager, GameManager gameManager, DiscardPileManager discardPileManager, UIManager uIManager, TurnStateController turnStateController, bool ownedByPlayer) {
		this.storeManager = storeManager;
		this.gameManager = gameManager;
		this.discardPileManager = discardPileManager;
		this.ownedByPlayer = ownedByPlayer;
		this.turnStateController = turnStateController;
		this.uIManager = uIManager;
		cardIsInStore = true;
		canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
		cardArt.sprite = card.cardArt;
		cardName.text = card.cardName;
		description.text = card.description;
		buyCost.text = card.buyCost.ToString();
		playCost.text = card.playCost.ToString();
		discardValueText.text = card.discardValue.ToString();
		cardType = card.cardType;
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
		cardType = card.cardType;

		if(card.isCardBack()) {
            foreach (Transform child in cardArt.gameObject.transform) {
                child.gameObject.SetActive(false);
            }
        } 
    }

	public void UpdateValueOnBoard(HandManager handManager, GameManager gameManager, TurnStateController turnStateController, UIManager uIManager) {
		this.handManager = handManager;
		this.gameManager = gameManager;
		this.turnStateController = turnStateController;
		card.AssignGameManager(gameManager, handManager);
		cardArt.sprite = card.cardArt;
		cardName.text = card.cardName;
		description.text = card.description;
		playCost.text = card.playCost.ToString();
		discardValueText.text = card.discardValue.ToString();
		cardType = card.cardType;
	}

    public void BuyTheCard() {
		if(turnStateController.CheckIfItIsPlayersTurn() && cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.buyCost) {
				gameManager.DecreasePlayerMoney(card.buyCost);
				card.OnBuy();
				uIManager.SetBuyAreaActiveStatus(false);
				discardPileManager.AddCardToDiscardPile(card);
                storeManager.CardIsBought(card);
                Destroy(gameObject);
			}
		}
	}

	public void PlayToolCard(int toolSlotNumber) {
		if(turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.playCost) {
				card.OnPlay();
				handManager.CardWasPlayedOnBoard(card, gameObject, toolSlotNumber);
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
				cardThatWasPlayed.GetComponent<CardBaseFunctionality>().UpdateValueOnBoard(handManager, gameManager, turnStateController, uIManager);
				handManager.CardWasPlayedOnBoard(card, gameObject, 0);
			}
		}
	}

	public void PlayEventCard() {
        if (turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.playCost) {
				card.OnPlay();
				if(card.cardType==CardType.Event) {
					handManager.MoveCardToDiscardPile(card, gameObject);
				}
			}
		}
    }

    public void DiscardTheCard() {
        if(turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
            card.OnDiscard();
			handManager.MoveCardToDiscardPile(card, gameObject);
        }
    }
}
