using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks {
    public TMP_InputField createInput;
    public GameObject searchingpanel;
    public GameObject mainpanel;
    [SerializeField] CharacterSelection characterSelection;

    public void CreateRoom() {
        RoomOptions roomoptions = new RoomOptions() {
            IsOpen = true,
            IsVisible = false,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom(createInput.text, roomoptions);
    }
    
    public void JoinRoom() {
        PhotonNetwork.JoinRoom(createInput.text);
    }

    public void CancelSearch() {
        PhotonNetwork.LeaveRoom();
        searchingpanel.SetActive(false);
        mainpanel.SetActive(true);
		characterSelection.SetCharacterButtonsEnabled(true);
	}

    public override void OnJoinedRoom() {
        searchingpanel.SetActive(true);
        mainpanel.SetActive(false);

        Debug.Log("joined a room, players in room: " + PhotonNetwork.CurrentRoom.PlayerCount + "/2");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            Debug.Log("room full, loading game");
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public void FindMatch() {
        //disables character buttons
        characterSelection.SetCharacterButtonsEnabled(false);
        //saves selected class
		PlayerPrefs.SetString("SelectedClass", characterSelection.GetSelectedClass().ToString());
		RoomOptions roomoptions = new RoomOptions() {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 2
        };
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("Another player joined, players in room: " + PhotonNetwork.CurrentRoom.PlayerCount + "/2");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            Debug.Log("room full, loading game");
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
