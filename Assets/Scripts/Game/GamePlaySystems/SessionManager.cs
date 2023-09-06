using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SessionManager : NetworkBehaviour {
	[SerializeField] GameManager gameManager;
	[SerializeField] TurnStateController turnStateController;
	
	public ulong player1Id;
	public ulong player2Id;

	[ServerRpc(RequireOwnership = false)]
	public void UpdateClientIdServerRpc(ServerRpcParams serverRpcParams = default) {
		var clientId = serverRpcParams.Receive.SenderClientId;
		if(player1Id == 0) {
			player1Id = clientId;
		}
		else if(player2Id == 0) {
			player2Id = clientId;
			turnStateController.ConnectionStarted(player1Id, player2Id);
			UpdateClientIDClientRPC(player1Id, player2Id);
		}
	}

	[ClientRpc]
	public void UpdateClientIDClientRPC(ulong player1Id, ulong player2Id) {
		if(!IsHost) {
			this.player1Id = player1Id;
			this.player2Id = player2Id;
		}
		//turnStateController.ConnectionStarted(player1Id, player2Id);
	}

	public override void OnNetworkSpawn() {
		UpdateClientIdServerRpc();
	}
}
