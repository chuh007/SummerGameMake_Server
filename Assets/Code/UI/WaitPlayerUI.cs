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
        [SerializeField] private TextMeshProUGUI playerCountText;
        [SerializeField] private GameObject joinCodeObj;
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

        public override void OnNetworkSpawn()
        {
            if(!IsHost) joinCodeObj.SetActive(false);
        }

        private void OnClientConnected(ulong clientId)
        {
            _playerCount++;
            playerCountText.text = (_playerCount + 1).ToString() + "/2";
            startBtnImg.color = _playerCount >= 1 ? Color.green : Color.red;
        }

        private void OnClientDisconnected(ulong clientId)
        {
            _playerCount--;
        }
    }
}