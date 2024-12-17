using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDiscardArea : MonoBehaviour, IDropHandler {
    
    public void OnDrop(PointerEventData eventData) {
        //Debug.Log("DroppedOnDiscardArea");
        if(eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<CardBaseFunctionality>().DiscardTheCard();
        }
    }
}
