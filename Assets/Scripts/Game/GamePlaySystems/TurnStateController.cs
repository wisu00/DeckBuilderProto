using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;
using Unity.Netcode;
using UnityEditor.PackageManager;

public enum TurnState {StartOfTurn, Draw, Play, EndOfTurn, OpponentsTurn}

public class TurnStateController : NetworkBehaviour {

    public TurnState currentTurnState = TurnState.OpponentsTurn;
  //  public PhotonView photonView;
    public UIManager uIManager;
    [SerializeField] DeckManager deck;

    public void ConnectionStarted(ulong client1Id, ulong client2id) {
        if (!IsServer) return;

        ulong startingClientId;

        //min inclusive max exlusive
        if (Random.Range(1, 3) == 1)
			startingClientId = client1Id;
        else {
			startingClientId = client2id;
		}

		ClientRpcParams clientRpcParams = new ClientRpcParams {
			Send = new ClientRpcSendParams {
				TargetClientIds = new ulong[] { startingClientId }
			}
		};

		StartFirstTurnClientRpc(clientRpcParams);
	}

	[ServerRpc(RequireOwnership = false)]
	public void TurnPassedServerRpc(ServerRpcParams serverRpcParams = default) {
		var clientId = serverRpcParams.Receive.SenderClientId;
		
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

    [ClientRpc]
    public void TurnReceivedClientRpc(ClientRpcParams clientRpcParams = default) {
        ChangeState(TurnState.StartOfTurn);
    }

    [ClientRpc]
    public void StartFirstTurnClientRpc(ClientRpcParams clientRpcParams = default) {
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