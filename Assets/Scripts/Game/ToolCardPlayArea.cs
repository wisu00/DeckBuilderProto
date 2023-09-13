using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolCardPlayArea : MonoBehaviour, IDropHandler {
	[SerializeField] int toolSlotNumber;
	[SerializeField] UIManager uIManager;
	bool isOccupied = false;

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("DroppedOnPlayArea");
        if(eventData.pointerDrag != null) {
            if(!isOccupied) {
				eventData.pointerDrag.GetComponent<CardBaseFunctionality>().PlayToolCard(toolSlotNumber);
				isOccupied = true;
			}
            else {
				uIManager.ShowToolSlotTakenMessage();
			}
		}
    }
}
