using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolCardPlayArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] int toolSlotNumber;
	[SerializeField] UIManager uIManager;
	bool isOccupied = false;
	[SerializeField] Image darkTint;
	[SerializeField] BoardManager boardManager;

	public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag != null) {
			//check if card can be played
			if(!eventData.pointerDrag.GetComponent<CardBaseFunctionality>().CardCanBePlayed()) {
				SetDarkTintActive(false);
				uIManager.ShowCardCantBePlayedMessage();
				return;
			}
			//clear slot if already taken
			if(isOccupied) {
				Debug.Log("replacing tool");
				//uIManager.ShowToolSlotTakenMessage();
				boardManager.RemoveToolFromBoard(toolSlotNumber);
				isOccupied = false;
				SetDarkTintActive(false);
			}

			eventData.pointerDrag.GetComponent<CardBaseFunctionality>().PlayToolCard(toolSlotNumber);
			isOccupied = true;
			//keeps area interactable by moving it on top
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
