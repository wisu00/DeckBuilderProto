using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocationCardPlayArea : MonoBehaviour, IDropHandler {
	[SerializeField] Transform cardPlaceOnBoard;

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("DroppedOnPlayArea");
        if(eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<CardBaseFunctionality>().PlayLocationCard(cardPlaceOnBoard);
        }
    }
}
