using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum CharacterClasses {Banker, Scrapper, Cultist};

public class CharacterSelection : MonoBehaviour {
    [SerializeField] Button changeClassBanker;
	[SerializeField] Button changeClassScrapper;
	[SerializeField] Button changeClassCultist;

	CharacterClasses selectedClass;

	private void Start() {
		BankerSelected();
	}

	public CharacterClasses GetSelectedClass() {
		return selectedClass;
	}

	public void SetCharacterButtonsEnabled(bool areActive) {
		changeClassBanker.gameObject.SetActive(areActive);
		changeClassScrapper.gameObject.SetActive(areActive);
		changeClassCultist.gameObject.SetActive(areActive);
	}

	public void BankerSelected() {
		changeClassScrapper.image.color = Color.white;
		changeClassCultist.image.color = Color.white;
		changeClassBanker.image.color = Color.gray;
		selectedClass = CharacterClasses.Banker;
	}

	public void ScrapperSelected() {
		changeClassBanker.image.color = Color.white;
		changeClassCultist.image.color = Color.white;
		changeClassScrapper.image.color = Color.gray;
		selectedClass = CharacterClasses.Scrapper;
	}

	public void CultistSelected() {
		changeClassBanker.image.color = Color.white;
		changeClassScrapper.image.color = Color.white;
		changeClassCultist.image.color = Color.gray;
		selectedClass = CharacterClasses.Cultist;
	}
}
