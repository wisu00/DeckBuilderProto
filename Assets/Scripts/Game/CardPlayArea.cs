using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlayArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("DroppedOnPlayArea");
        if(eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<CardBaseFunctionality>().PlayEventCard();
        }
    }

	public void OnPointerEnter(PointerEventData eventData) {
		if(eventData.pointerDrag != null) {
			if(eventData.pointerDrag.GetComponent<CardBaseFunctionality>().card.cardIsTargeted)
				eventData.pointerDrag.GetComponent<CardTargeting>().TurnTargetModeOn();
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if(eventData.pointerDrag != null) {
			if(eventData.pointerDrag.GetComponent<CardBaseFunctionality>().card.cardIsTargeted)
				eventData.pointerDrag.GetComponent<CardTargeting>().TurnTargetModeOff();
		}
	}
}
