using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class CardFieldUpdatingForVisualsOnly : MonoBehaviour {
	[SerializeField] Card cardScriptableObject;
	[SerializeField] GameObject cardVisuals;
	public TMP_Text cardName;
	public Image cardArt;
	public TMP_Text cardType;
	public TMP_Text description;
	public TMP_Text buyCost;
	public TMP_Text playCost;
	public TMP_Text discardValue;

	public void OnValidate() {
		UpdateCardContent();
	}

	private void UpdateCardContent() {
		if(cardScriptableObject == null) return;
		cardArt.sprite = cardScriptableObject.cardArt;
		cardName.text = cardScriptableObject.cardName;
		description.text = cardScriptableObject.description;
		cardType.text = "Class - " + cardScriptableObject.cardType.ToString();
		buyCost.text = cardScriptableObject.buyCost.ToString();
		playCost.text = cardScriptableObject.playCost.ToString();
		discardValue.text = cardScriptableObject.discardValue.ToString();
	}

}
