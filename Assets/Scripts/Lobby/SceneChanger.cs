using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public static class SceneChanger {

    public enum Scene {
        LobbyScene,
        GameScene
    }

    //private static Scene targetScene;

    //should be used when there is no connection
    public static void Load(Scene targetScene) {
        SceneManager.LoadScene(targetScene.ToString());
    }

    //moves all connected players at the same time
    public static void LoadNetWork(Scene targetScene) {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    //public static void SceneChangerCallBack() {
    //    SceneManager.LoadScene(targetScene.ToString());
    //}

    /*[SerializeField] private string gameSceneName;

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
    */
}
