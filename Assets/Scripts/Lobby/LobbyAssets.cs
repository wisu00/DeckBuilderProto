using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAssets : MonoBehaviour {



    public static LobbyAssets Instance { get; private set; }

    [SerializeField] private Sprite bankerSprite;
    [SerializeField] private Sprite politicianSprite;
    [SerializeField] private Sprite scrapperSprite;


    private void Awake() {
        Instance = this;
    }

    public Sprite GetSprite(LobbyManager.PlayerCharacter playerCharacter) {
        switch (playerCharacter) {
            default:
            case LobbyManager.PlayerCharacter.Banker:   return bankerSprite;
            case LobbyManager.PlayerCharacter.Politician:    return politicianSprite;
            case LobbyManager.PlayerCharacter.Scrapper:   return scrapperSprite;
        }
    }

}