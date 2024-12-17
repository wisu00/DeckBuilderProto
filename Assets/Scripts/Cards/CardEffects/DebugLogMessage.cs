using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogMessage", menuName = "Effects/LogMessage")]
public class DebugLogMessage : CardEffect {
	[SerializeField] string message;

	public override void DoEffect(ManagerReferences managerReferences, Card card, CardBaseFunctionality cardBaseFunctionality) {
		Debug.Log(message);
	}
}
