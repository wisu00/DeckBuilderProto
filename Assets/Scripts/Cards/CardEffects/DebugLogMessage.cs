using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogMessage", menuName = "Cards/Effects/LogMessage", order = 1)]
public class DebugLogMessage : CardEffect {
	[SerializeField] string message;

	public override void DoEffect(GameManager gameManager) {
		Debug.Log(message);
	}
}
