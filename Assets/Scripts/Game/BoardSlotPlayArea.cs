using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardSlotPlayArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] int slotNumber;
	[SerializeField] UIManager uIManager;
	bool isOccupied = false;
	[SerializeField] Image darkTint;
	[SerializeField] BoardManager boardManager;

	public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag != null) {
			
			CardBaseFunctionality cardBeingPlayed = eventData.pointerDrag.GetComponent<CardBaseFunctionality>();

            //check if card can be played
            if (!cardBeingPlayed.ItIsPlayersTurn() && cardBeingPlayed.PlayerHasEnoughMoneyToPlayTheCard()) {
                SetDarkTintActive(false);
                if (!cardBeingPlayed.ItIsPlayersTurn()) uIManager.ShowNotYourTurnMessage();
                else if (!cardBeingPlayed.PlayerHasEnoughMoneyToPlayTheCard()) uIManager.ShowLackingRecources();
                return;
            }

            //clear slot if already taken
            if (isOccupied) {
                Debug.Log("replacing card on board");
                //uIManager.ShowToolSlotTakenMessage();
                boardManager.RemoveCardFromBoard(slotNumber);
                isOccupied = false;
				SetDarkTintActive(false);
			}

			cardBeingPlayed.PlayCardOnBoard(slotNumber);
			isOccupied = true;
			//keeps area interactable by moving it on top in hierarchy
			transform.SetAsLastSibling();
		}
    }

	public void OnPointerEnter(PointerEventData eventData) {
		//activates the dark tint only when tool is on board and getting hovered when holding another tool card
		if(isOccupied && eventData.pointerDrag != null) {
			if(eventData.pointerDrag.GetComponent<CardBaseFunctionality>().card.cardType == CardType.Tool) SetDarkTintActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if(isOccupied && eventData.pointerDrag != null) {
			if(eventData.pointerDrag.GetComponent<CardBaseFunctionality>().card.cardType == CardType.Tool) SetDarkTintActive(false);
		}
	}

	public void SetDarkTintActive(bool state) {
		darkTint.gameObject.SetActive(state);
	}
}
