using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectCondition : ScriptableObject {
	public abstract bool CheckCondition(ManagerReferences managerReferences, Card card);
}