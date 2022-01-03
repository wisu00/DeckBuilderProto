using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] DeckManager deck;
    [SerializeField] HandManager hand;
    [SerializeField] DiscardPileManager discardPile;

    #region Photon Callbacks

    // Called when the local player left the room. We need to load the launcher scene.
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    #endregion

    #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")) {
            deck.DrawCard();
        }
    }
}
