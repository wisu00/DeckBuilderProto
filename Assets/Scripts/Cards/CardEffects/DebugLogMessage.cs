using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogMessage", menuName = "Effects/LogMessage", order = 1)]
public class DebugLogMessage : CardEffect {
	[SerializeField] string message;

	public override void DoEffect(ManagerReferences managerReferences, Card card) {
		Debug.Log(message);
	}
}
