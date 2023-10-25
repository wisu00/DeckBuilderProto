using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasePlayerNotoriety", menuName = "Effects/IncreasePlayerNotoriety")]
public class IncreasePlayerNotoriety : CardEffect {

	[Range(0, 10)] [SerializeField] int increaseAmount = 0;

	public override void DoEffect(ManagerReferences managerReferences, Card card) {
		managerReferences.GetGameManager().IncreasePlayerNotoriety(increaseAmount);
	}
}