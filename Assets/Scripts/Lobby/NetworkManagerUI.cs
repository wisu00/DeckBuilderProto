using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour {

    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clienBtn;

    private void Awake() {
        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        clienBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
