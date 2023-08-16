using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

public enum TurnState {StartOfTurn, Draw, Play, EndOfTurn, OpponentsTurn}

public class TurnStateController : MonoBehaviour {

    public TurnState currentTurnState = TurnState.OpponentsTurn;
  //  public PhotonView photonView;
    public UIManager uIManager;
    [SerializeField] DeckManager deck;

    private void Start() {
 //      if(PhotonNetwork.IsMasterClient) {
 //          //min inclusive max exlusive
 //          if(Random.Range(1, 3) == 1) startTheGame();
 //          else photonView.RPC("TurnReceived", RpcTarget.OthersBuffered);
 //      }
    }

    public void ChangeState(TurnState newState) {
        currentTurnState = newState;
        switch (currentTurnState) {
            case TurnState.StartOfTurn: StartOfTurnStateEntered(); break;
            case TurnState.Draw: DrawStateEntered(); break;
            case TurnState.Play: PlayStateEntered(); break;
            case TurnState.EndOfTurn: EndOfTurnStateEntered(); break;
            case TurnState.OpponentsTurn: break;
            default: break;
        }
    }

 //  [PunRPC]
 //  public void TurnReceived() {
 //      ChangeState(TurnState.StartOfTurn);
 //  }

    public void startTheGame() {
        ChangeState(TurnState.StartOfTurn);
    }

    public void StartOfTurnStateEntered() {
        //refress cards in store
        //start of turn effects
        ChangeState(TurnState.Draw);
    }

    public void DrawStateEntered() {
        //draw turns cards (default 2) if hand has space (5)
        for(int i=0; i<2; i++) {
            deck.DrawCard();
        }
        
        ChangeState(TurnState.Play);
    }

    public void PlayStateEntered() {
        //the phase where actions are enabled
        //enable end turn button
        uIManager.InPlayTurnState(true);
    }

    public void EndTurnButtonPressed() {
        //disable end turn button
        uIManager.InPlayTurnState(false);
        ChangeState(TurnState.EndOfTurn);
    }

    public void EndOfTurnStateEntered() {
        //end of turn effects
        TurnPassed();
    }

    public void TurnPassed() {
        ChangeState(TurnState.OpponentsTurn);
 //       photonView.RPC("TurnReceived", RpcTarget.OthersBuffered);
        //Start opponents turn by calling TurnReceived() for them;
    }
}