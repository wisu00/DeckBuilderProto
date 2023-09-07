using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBaseFunctionality : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
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
    private CanvasGroup canvasGroup;
    private bool cardIsInStore = false;
    private GameObject cardBuyArea;
    private bool ownedByPlayer = true;

    Vector3 startPosition;

    private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(card.isCardBack() || !ownedByPlayer) {
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
			cardBuyArea.SetActive(false);
		}
	}

    public void PickCardUp() {
        startPosition = transform.position;
        if(cardIsInStore) {
			cardBuyArea.SetActive(true);
		}
    }

    public void UpdateValuesInStore(StoreManager storeManager, DiscardPileManager discardPileManager, GameObject cardBuyArea, bool ownedByPlayer) {
		this.storeManager = storeManager;
        this.cardBuyArea = cardBuyArea;
		this.discardPileManager = discardPileManager;
		this.ownedByPlayer = ownedByPlayer;
        cardIsInStore = true;
		canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
		cardArt.sprite = card.cardArt;
		cardName.text = card.cardName;
		description.text = card.description;
		buyCost.text = card.buyCost.ToString();
		playCost.text = card.playCost.ToString();
		discardValueText.text = card.discardValue.ToString();
	}

    public void UpdateValues(HandManager handManager, GameManager gameManagerScript, TurnStateController turnStateControllerScript) {
		this.handManager = handManager;
        gameManager = gameManagerScript;
        turnStateController = turnStateControllerScript;
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

    public void BuyTheCard() {
		if(turnStateController.CheckIfItIsPlayersTurn() && cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.buyCost) {
				cardBuyArea.SetActive(false);
				discardPileManager.AddCardToDiscardPile(card);
                storeManager.CardIsBought(card);
                Destroy(card);
			}
		}
	}

    public void PlayTheCard() {
        if (turnStateController.CheckIfItIsPlayersTurn() && !cardIsInStore) {
			if(gameManager.getMoneyPlayer() >= card.playCost) {
				card.OnPlay();
				if(!card.goesOnBoard()) {
					handManager.MoveCardToDiscardPile(card, gameObject);
				}
				else {
					//play card on board
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
