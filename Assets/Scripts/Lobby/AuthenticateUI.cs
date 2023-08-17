using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {


    [SerializeField] private Button authenticateButton;
    [SerializeField] private Button editPlayerNameButton; 


    private void Awake() {
        authenticateButton.onClick.AddListener(() => {
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
        });
    }

    private void Hide() {
        editPlayerNameButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}