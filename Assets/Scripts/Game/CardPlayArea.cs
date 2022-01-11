using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlayArea : MonoBehaviour, IDropHandler {
    
    public void OnDrop(PointerEventData eventData) {
        Debug.Log("DroppedOnPlayArea");
        if(eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<CardBaseFunctionality>().PlayTheCard();
        }
    }

}
