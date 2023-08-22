using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkVariables : NetworkBehaviour {
    private NetworkVariable<int> player1Money = new NetworkVariable<int>(); 
    private NetworkVariable<int> player2Money = new NetworkVariable<int>();
    private NetworkVariable<int> player1Influence = new NetworkVariable<int>();
    private NetworkVariable<int> player2Influence = new NetworkVariable<int>();
}
