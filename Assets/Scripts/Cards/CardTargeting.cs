using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTargeting : MonoBehaviour {
	[SerializeField] CardBaseFunctionality cardBaseFunctionality;
	[SerializeField] GameObject cardVisuals;
	[SerializeField] GameObject targetVisuals;

	public void TurnTargetModeOn() {
		cardBaseFunctionality.SetTargetState(true);
		//cardVisuals.SetActive(false);
		targetVisuals.SetActive(true);
	}

	public void TurnTargetModeOff() {
		cardBaseFunctionality.SetTargetState(false);
		//cardVisuals.SetActive(true);
		targetVisuals.SetActive(false);
	}
}
