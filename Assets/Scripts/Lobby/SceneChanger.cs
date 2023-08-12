using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class SceneChanger : NetworkBehaviour {
    [SerializeField] private string gameSceneName;

    [ServerCallback]
    void Update()
    {
        // engine invoked callback - will only run on server
    }

    public override void OnClientConnected() {
        //Change the text to show the connection on the client side
        m_ClientText.text =  " " + connection.connectionId + " Connected!";
        if(IsServer && !string.IsNullOrEmpty(gameSceneName));
    }

    private void ChangeSceneToGame() {
        var status = NetworkManager.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Additive);
        if (status != SceneEventProgressStatus.Started) {
            Debug.LogWarning($"Failed to load {gameSceneName} " +
                $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }
}
