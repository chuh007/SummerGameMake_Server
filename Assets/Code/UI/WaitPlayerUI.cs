using System;
using Code.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class WaitPlayerUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI joinCodeText;
        [SerializeField] private Image startBtnImg;
        
        private int _playerCount = 0;
        
        private void Start()
        {
            joinCodeText.text = HostSingleton.Instance.GameManager.JoinCode;
            if (IsServer)
            {
                NetworkManager.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
                startBtnImg.color = Color.red;
            }
        }
        
        private void OnClientConnected(ulong clientId)
        {
            _playerCount++;
            if (_playerCount >= 1)
            {
                startBtnImg.color = Color.green;
            }
            else
            {
                startBtnImg.color = Color.red;
            }
        }

        private void OnClientDisconnected(ulong clientId)
        {
            _playerCount--;
        }
    }
}