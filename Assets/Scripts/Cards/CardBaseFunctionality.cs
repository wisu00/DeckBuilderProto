using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBaseFunctionality : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Card card;
    public HandManager hand;
    private GameManager gameManager;
    private Canvas canvas;
    public TMP_Text cardName;
    public Image cardArt;
    public TMP_Text description;
    public TMP_Text goldCost;
    public TMP_Text discardValueText;

    Vector3 startPosition;

    public void OnBeginDrag(PointerEventData eventData) {
        if(card.isCardBack()) {
            eventData.pointerDrag = null;
        }
        else {
            PickCardUp();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if(gameManager.getMoneyPlayer()>=card.goldCost) {
            PlayTheCard();
        } else {
            transform.position = startPosition;
        }
    }

    public void PickCardUp() {
        startPosition = transform.position;
    } 

    public void UpdateValues(HandManager handScript, GameManager gameManagerScript) {
        hand = handScript;
        gameManager = gameManagerScript;
        canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        card.AssignGameManager(gameManager, handScript);
        cardArt.sprite = card.cardArt;
        cardName.text = card.cardName;
        description.text = card.description;
        goldCost.text = card.goldCost.ToString();
        discardValueText.text = card.discardValueText.ToString();
        if(card.isCardBack()) {
            foreach (Transform child in cardArt.gameObject.transform) {
                child.gameObject.SetActive(false);
            }
        } 
    }

    public void PlayTheCard() {
        card.OnPlay();
        if(!card.goesOnBoard()) {
            hand.MoveCardToDiscardPile(card, gameObject);
        }
        else {
            //play card on board
        }
    }
}
